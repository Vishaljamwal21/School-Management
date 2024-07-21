using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.Threading.Tasks;

namespace StudentManagementApp.Controllers
{
    public class TeacherSubjectController : Controller
    {
        private readonly ITeacherSubjectRepository _teacherSubjectRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassRepository _classRepository;
        private readonly ITeacherRepository _teacherRepository;

        public TeacherSubjectController(ITeacherSubjectRepository teacherSubjectRepository,ISubjectRepository subjectRepository
            ,IClassRepository classRepository,ITeacherRepository teacherRepository)
        {
            _teacherSubjectRepository = teacherSubjectRepository;
            _subjectRepository = subjectRepository;
            _classRepository = classRepository;
            _teacherRepository = teacherRepository;
        }

        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teacherSubjects = await _teacherSubjectRepository.GetAllAsync(URL.TeacherSubjectAPIPath);

            var subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);
            var classDictionary = new Dictionary<int, string>();

            foreach (var subject in subjects)
            {
                if (subject.ClassId.HasValue && !classDictionary.ContainsKey(subject.ClassId.Value))
                {
                    var classObj = await _classRepository.GetAsync(URL.ClassAPIPath, subject.ClassId.Value);
                    if (classObj != null)
                    {
                        classDictionary[subject.ClassId.Value] = classObj.ClassName;
                    }
                }
            }

            var teachers = await _teacherRepository.GetAllAsync(URL.TeacherAPIPath);
            var teacherDictionary = teachers.ToDictionary(t => t.TeacherId, t => t.Name);

            var result = teacherSubjects.Select(ts => new
            {
                ts.Id,
                TeacherName = ts.TeacherId.HasValue && teacherDictionary.ContainsKey(ts.TeacherId.Value) ? teacherDictionary[ts.TeacherId.Value] : "N/A",
                SubjectName = ts.SubjectId.HasValue ? subjects.FirstOrDefault(s => s.SubjectId == ts.SubjectId)?.SubjectName ?? "N/A" : "N/A",
                ClassName = ts.ClassId.HasValue ? classDictionary.GetValueOrDefault(ts.ClassId.Value, "N/A") : "N/A"
            });

            return Json(new { data = result });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var teacherSubject = await _teacherSubjectRepository.GetAsync(URL.TeacherSubjectAPIPath, Id);
            if (teacherSubject == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _teacherSubjectRepository.DeleteAsync(URL.TeacherSubjectAPIPath, Id);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SaveOrUpdate(int? Id)
        {
            TeacherSubject teacherSubject = new TeacherSubject();
            if (Id != null)
            {
                teacherSubject = await _teacherSubjectRepository.GetAsync(URL.TeacherSubjectAPIPath, Id.Value);
                if (teacherSubject == null)
                    return NotFound();
            }
            ViewBag.Teachers = await _teacherRepository.GetAllAsync(URL.TeacherAPIPath);
            ViewBag.Classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            ViewBag.Subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);

            return View(teacherSubject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(TeacherSubject teacherSubject)
        {
            if (ModelState.IsValid)
            {
                if (teacherSubject.Id == 0)
                    await _teacherSubjectRepository.CreateAsync(URL.TeacherSubjectAPIPath, teacherSubject);
                else
                    await _teacherSubjectRepository.UpdateAsync(URL.TeacherSubjectAPIPath, teacherSubject);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(teacherSubject);
            }
        }
    }
}
