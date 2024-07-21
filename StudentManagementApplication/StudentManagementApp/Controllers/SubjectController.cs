using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;

namespace StudentManagementApp.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassRepository _classRepository;
        public SubjectController(ISubjectRepository subjectRepository,IClassRepository classRepository)
        {
            _subjectRepository = subjectRepository;
            _classRepository = classRepository;
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
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

            var result = subjects.Select(subject => new
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                ClassName = subject.ClassId.HasValue ? classDictionary[subject.ClassId.Value] : "N/A"
            });

            return Json(new { data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int subjectId)
        {
            var subject = await _subjectRepository.GetAsync(URL.SubjectAPIPath, subjectId);
            if (subject == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _subjectRepository.DeleteAsync(URL.SubjectAPIPath, subjectId);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SaveOrUpdate(int? subjectId)
        {
            Subject subject = new Subject();
            if (subjectId != null)
            {
                subject = await _subjectRepository.GetAsync(URL.SubjectAPIPath, subjectId.Value);
                if (subject == null)
                    return NotFound();
            }

            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            ViewBag.Classes = classes.Select(c => new { c.ClassId, c.ClassName });

            return View(subject);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(Subject subject)
        {
            if (ModelState.IsValid)
            {
                if (subject.SubjectId == 0)
                    await _subjectRepository.CreateAsync(URL.SubjectAPIPath, subject);
                else
                    await _subjectRepository.UpdateAsync(URL.SubjectAPIPath, subject);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(subject);
            }
        }
    }
}
