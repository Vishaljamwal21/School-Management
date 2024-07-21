using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace StudentManagementApp.Controllers
{
    public class ExamController : Controller
    {
        private readonly IExamRepository _examRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IStudentRepository _studentRepository;

        public ExamController(IExamRepository examRepository, IClassRepository classRepository, ISubjectRepository subjectRepository, IStudentRepository studentRepository)
        {
            _examRepository = examRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
            _studentRepository = studentRepository;
        }

        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exams = await _examRepository.GetAllAsync(URL.ExamAPIPath);
            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            var subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);
            var students = await _studentRepository.GetAllAsync(URL.StudentAPIPath);

            var classDictionary = classes.ToDictionary(c => c.ClassId, c => c.ClassName);
            var subjectDictionary = subjects.ToDictionary(s => s.SubjectId, s => s.SubjectName);
            var studentDictionary = students.ToDictionary(s => s.RollNo, s => s.Name);

            var result = exams.Select(e => new
            {
                e.ExamId,
                ClassName = e.ClassId.HasValue ? classDictionary.GetValueOrDefault(e.ClassId.Value, "N/A") : "N/A",
                SubjectName = e.SubjectId.HasValue ? subjectDictionary.GetValueOrDefault(e.SubjectId.Value, "N/A") : "N/A",
                StudentName = e.RollNo != null && studentDictionary.ContainsKey(e.RollNo) ? studentDictionary[e.RollNo] : "N/A",
                e.TotalMarks,
                e.OutOfMarks
            });

            return Json(new { data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int examId)
        {
            var exam = await _examRepository.GetAsync(URL.ExamAPIPath, examId);
            if (exam == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _examRepository.DeleteAsync(URL.ExamAPIPath, examId);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveOrUpdate(int? examId)
        {
            Exam exam = new Exam();
            if (examId != null)
            {
                exam = await _examRepository.GetAsync(URL.ExamAPIPath, examId.Value);
                if (exam == null)
                    return NotFound();
            }

            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            var subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);
            var students = await _studentRepository.GetAllAsync(URL.StudentAPIPath);

            ViewBag.Classes = classes ?? new List<Class>();
            ViewBag.Subjects = subjects ?? new List<Subject>();
            ViewBag.Students = students ?? new List<Student>();

            return View(exam);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(Exam exam)
        {
            if (ModelState.IsValid)
            {
                if (exam.ExamId == 0)
                    await _examRepository.CreateAsync(URL.ExamAPIPath, exam);
                else
                    await _examRepository.UpdateAsync(URL.ExamAPIPath, exam);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
                ViewBag.Subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);
                ViewBag.Students = await _studentRepository.GetAllAsync(URL.StudentAPIPath);
                return View(exam);
            }
        }
    }
}
