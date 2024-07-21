using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace StudentManagementApp.Controllers
{
    public class StudentAttendanceController : Controller
    {
        private readonly IStudentAttendanceRepository _studentAttendanceRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;

        public StudentAttendanceController(IStudentAttendanceRepository studentAttendanceRepository,
                                           IClassRepository classRepository,
                                           ISubjectRepository subjectRepository)
        {
            _studentAttendanceRepository = studentAttendanceRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
        }

        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _studentAttendanceRepository.GetAllAsync(URL.StudentAttendanceAPIPath);
            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            var subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);

            var classDictionary = classes.ToDictionary(c => c.ClassId, c => c.ClassName);
            var subjectDictionary = subjects.ToDictionary(s => s.SubjectId, s => s.SubjectName);

            var result = attendances.Select(attendance => new
            {
                Id = attendance.Id,
                ClassName = attendance.ClassId.HasValue ? classDictionary.GetValueOrDefault(attendance.ClassId.Value, "N/A") : "N/A",
                SubjectName = attendance.SubjectId.HasValue ? subjectDictionary.GetValueOrDefault(attendance.SubjectId.Value, "N/A") : "N/A",
                RollNo = attendance.RollNo,
                Status = attendance.Status.HasValue && attendance.Status.Value ? "Present" : "Absent",
                Date = attendance.Date.HasValue ? attendance.Date.Value.ToString("yyyy-MM-dd") : "N/A"
            });

            return Json(new { data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var attendance = await _studentAttendanceRepository.GetAsync(URL.StudentAttendanceAPIPath, id);
            if (attendance == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _studentAttendanceRepository.DeleteAsync(URL.StudentAttendanceAPIPath, id);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveOrUpdate(int? id)
        {
            StudentAttendance attendance = new StudentAttendance();
            if (id != null)
            {
                attendance = await _studentAttendanceRepository.GetAsync(URL.StudentAttendanceAPIPath, id.Value);
                if (attendance == null)
                    return NotFound();
            }

            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            ViewBag.Classes = classes.Select(c => new { c.ClassId, c.ClassName });

            var subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);
            ViewBag.Subjects = subjects.Select(s => new { s.SubjectId, s.SubjectName });

            return View(attendance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(StudentAttendance attendance)
        {
            if (ModelState.IsValid)
            {
                if (attendance.Id == 0)
                    await _studentAttendanceRepository.CreateAsync(URL.StudentAttendanceAPIPath, attendance);
                else
                    await _studentAttendanceRepository.UpdateAsync(URL.StudentAttendanceAPIPath, attendance);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
                ViewBag.Classes = classes.Select(c => new { c.ClassId, c.ClassName });

                var subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);
                ViewBag.Subjects = subjects.Select(s => new { s.SubjectId, s.SubjectName });

                return View(attendance);
            }
        }
    }
}
