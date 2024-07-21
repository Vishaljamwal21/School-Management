using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StudentManagementApp.Controllers
{
    public class TeacherAttendanceController : Controller
    {
        private readonly ITeacherAttendanceRepository _teacherAttendanceRepository;
        private readonly ITeacherRepository _teacherRepository;

        public TeacherAttendanceController(ITeacherAttendanceRepository teacherAttendanceRepository, ITeacherRepository teacherRepository)
        {
            _teacherAttendanceRepository = teacherAttendanceRepository;
            _teacherRepository = teacherRepository;
        }

        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _teacherAttendanceRepository.GetAllAsync(URL.TeacherAttendanceAPIPath);
            var teachers = await _teacherRepository.GetAllAsync(URL.TeacherAPIPath);
            var teacherDictionary = teachers.ToDictionary(t => t.TeacherId, t => t.Name);

            var result = attendances.Select(attendance => new
            {
                Id = attendance.Id,
                TeacherName = attendance.TeacherId.HasValue && teacherDictionary.ContainsKey(attendance.TeacherId.Value) ? teacherDictionary[attendance.TeacherId.Value] : "N/A",
                Status = attendance.Status.HasValue && attendance.Status.Value ? "Present" : "Absent",
                Date = attendance.Date.HasValue ? attendance.Date.Value.ToString("yyyy-MM-dd") : "N/A"
            });

            return Json(new { data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var attendance = await _teacherAttendanceRepository.GetAsync(URL.TeacherAttendanceAPIPath, id);
            if (attendance == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _teacherAttendanceRepository.DeleteAsync(URL.TeacherAttendanceAPIPath, id);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveOrUpdate(int? id)
        {
            TeacherAttendance attendance = new TeacherAttendance();
            if (id != null)
            {
                attendance = await _teacherAttendanceRepository.GetAsync(URL.TeacherAttendanceAPIPath, id.Value);
                if (attendance == null)
                    return NotFound();
            }

            var teachers = await _teacherRepository.GetAllAsync(URL.TeacherAPIPath);
            ViewBag.Teachers = teachers.Select(t => new { t.TeacherId, t.Name });

            return View(attendance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(TeacherAttendance attendance)
        {
            if (ModelState.IsValid)
            {
                if (attendance.Id == 0)
                    await _teacherAttendanceRepository.CreateAsync(URL.TeacherAttendanceAPIPath, attendance);
                else
                    await _teacherAttendanceRepository.UpdateAsync(URL.TeacherAttendanceAPIPath, attendance);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var teachers = await _teacherRepository.GetAllAsync(URL.TeacherAPIPath);
                ViewBag.Teachers = teachers.Select(t => new { t.TeacherId, t.Name });
                return View(attendance);
            }
        }
    }
}
