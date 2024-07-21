using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.Threading.Tasks;

namespace StudentManagementApp.Controllers
{
    public class ClassController : Controller
    {
        private readonly IClassRepository _classRepository;

        public ClassController(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _classRepository.GetAllAsync(URL.ClassAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int classId)
        {
            var class1 = await _classRepository.GetAsync(URL.ClassAPIPath, classId);
            if (class1 == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _classRepository.DeleteAsync(URL.ClassAPIPath, classId);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveOrUpdate(int? classId)
        {
            Class class1 = new Class();
            if (classId != null) 
            {
                class1 = await _classRepository.GetAsync(URL.ClassAPIPath,classId.Value);
                if (class1 == null)
                    return NotFound();
            }
            return View(class1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(Class class1) 
        {
            if (ModelState.IsValid)
            {
                //// Use the generic ExistAsync method
                //bool isDuplicate = await _classRepository.ExistAsync(URL.ClassAPIPath, class1.ClassName);

                //// Check for duplicate class name
                //if (isDuplicate && class1.ClassId == 0) // New class with duplicate name
                //{
                //    ModelState.AddModelError("", "Class with this name already exists.");
                //    return View(class1);
                //}

                if (class1.ClassId == 0) 
                    await _classRepository.CreateAsync(URL.ClassAPIPath, class1);
                else
                    await _classRepository.UpdateAsync(URL.ClassAPIPath, class1);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(class1);
            }
        }
    }
}
