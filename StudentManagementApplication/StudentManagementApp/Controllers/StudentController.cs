using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClassRepository _classRepository;

        public StudentController(IStudentRepository studentRepository, IClassRepository classRepository)
        {
            _studentRepository = studentRepository;
            _classRepository = classRepository;
        }

        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentRepository.GetAllAsync(URL.StudentAPIPath);

            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            var classDictionary = classes.ToDictionary(c => c.ClassId, c => c.ClassName);

            var result = students.Select(s => new
            {
                s.StudentId,
                s.Name,
                Dob = s.Dob?.ToString("yyyy-MM-dd"),
                s.Gender,
                s.Mobile,
                s.RollNo,
                s.Address,
                ClassName = s.ClassId.HasValue ? classDictionary.GetValueOrDefault(s.ClassId.Value, "N/A") : "N/A"
            });

            return Json(new { data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _studentRepository.GetAsync(URL.StudentAPIPath, id);
            if (student == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _studentRepository.DeleteAsync(URL.StudentAPIPath, id);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveOrUpdate(int? id)
        {
            Student student = new Student();
            if (id != null)
            {
                student = await _studentRepository.GetAsync(URL.StudentAPIPath, id.Value);
                if (student == null)
                    return NotFound();
            }

            ViewBag.Classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(Student student)
        {
            if (ModelState.IsValid)
            {
                if (student.StudentId == 0)
                    await _studentRepository.CreateAsync(URL.StudentAPIPath, student);
                else
                    await _studentRepository.UpdateAsync(URL.StudentAPIPath, student);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
                return View(student);
            }
        }
    }
}
