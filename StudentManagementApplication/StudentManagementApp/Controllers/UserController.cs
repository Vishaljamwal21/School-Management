using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace StudentManagementApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public UserController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public IActionResult Register()
        {
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Admin admin)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _adminRepository.CreateAsync(URL.RegisterAPIPath, admin);
                    if (result)
                    {
                        return RedirectToAction("Index", "Home"); 
                    }
                    ModelState.AddModelError(string.Empty, "Failed to register admin.");
                }
                return View(admin); 
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while registering admin.");
                return View(admin);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userObject = await _adminRepository.Authenticate(loginVM.Email, loginVM.Password);

                    if (userObject == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                        return View(loginVM);
                    }

                    string token = null;
                    string role = null;

                    // Extract token and email from the user object
                    if (userObject is Admin admin)
                    {
                        token = admin.Token;
                    }
                    else if (userObject is Teacher teacher)
                    {
                        token = teacher.Token;
                    }
                    else
                    {
                        throw new Exception("Unknown user type.");
                    }

                    // Decode the token to extract the role
                    var jwtHandler = new JwtSecurityTokenHandler();
                    var jwtToken = jwtHandler.ReadJwtToken(token);

                    role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
                    if (string.IsNullOrEmpty(role))
                    {
                        throw new Exception("Role not found in token.");
                    }

                    // Continue if the token is not empty
                    if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(role))
                    {
                        SessionHandler.SetToken(HttpContext, token);
                        SessionHandler.SetUserRole(HttpContext, role);

                        if (SessionHandler.GetToken(HttpContext) != null &&
                            SessionHandler.GetUserRole(HttpContext) != null)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            throw new Exception("Token, email, or role is not stored in session.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
                return View(loginVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred during authentication: " + ex.Message);
                return View(loginVM);
            }
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
