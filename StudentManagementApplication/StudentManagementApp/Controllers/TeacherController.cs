using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.Threading.Tasks;

namespace StudentManagementApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherController(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _teacherRepository.GetAllAsync(URL.TeacherAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int teacherId)
        {
            var teacher = await _teacherRepository.GetAsync(URL.TeacherAPIPath, teacherId);
            if (teacher == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _teacherRepository.DeleteAsync(URL.TeacherAPIPath, teacherId);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveOrUpdate(int? teacherId)
        {
            Teacher teacher = new Teacher();
            if (teacherId != null)
            {
                teacher = await _teacherRepository.GetAsync(URL.TeacherAPIPath, teacherId.Value);
                if (teacher == null)
                    return NotFound();
            }
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                if (teacher.TeacherId == 0)
                    await _teacherRepository.CreateAsync(URL.TeacherAPIPath, teacher);
                else
                    await _teacherRepository.UpdateAsync(URL.TeacherAPIPath, teacher);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(teacher);
            }
        }
    }
}
