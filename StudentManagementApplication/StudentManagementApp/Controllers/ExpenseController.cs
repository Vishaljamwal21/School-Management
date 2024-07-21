using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace StudentManagementApp.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IClassRepository _classRepository;
        private readonly ISubjectRepository _subjectRepository;

        public ExpenseController(IExpenseRepository expenseRepository, IClassRepository classRepository, ISubjectRepository subjectRepository)
        {
            _expenseRepository = expenseRepository;
            _classRepository = classRepository;
            _subjectRepository = subjectRepository;
        }

        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _expenseRepository.GetAllAsync(URL.ExpenseAPIPath);
            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            var subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);

            var classDictionary = classes.ToDictionary(c => c.ClassId, c => c.ClassName);
            var subjectDictionary = subjects.ToDictionary(s => s.SubjectId, s => s.SubjectName);

            var result = expenses.Select(e => new
            {
                e.ExpenseId,
                ClassName = e.ClassId.HasValue ? classDictionary.GetValueOrDefault(e.ClassId.Value, "N/A") : "N/A",
                SubjectName = e.SubjectId.HasValue ? subjectDictionary.GetValueOrDefault(e.SubjectId.Value, "N/A") : "N/A",
                e.ChargeAmount
            });

            return Json(new { data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int expenseId)
        {
            var expense = await _expenseRepository.GetAsync(URL.ExpenseAPIPath, expenseId);
            if (expense == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _expenseRepository.DeleteAsync(URL.ExpenseAPIPath, expenseId);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveOrUpdate(int? expenseId)
        {
            Expense expense = new Expense();
            if (expenseId != null)
            {
                expense = await _expenseRepository.GetAsync(URL.ExpenseAPIPath, expenseId.Value);
                if (expense == null)
                    return NotFound();
            }

            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            var subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);

            ViewBag.Classes = classes ?? new List<Class>();
            ViewBag.Subjects = subjects ?? new List<Subject>();

            return View(expense);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(Expense expense)
        {
            if (ModelState.IsValid)
            {
                if (expense.ExpenseId == 0)
                    await _expenseRepository.CreateAsync(URL.ExpenseAPIPath, expense);
                else
                    await _expenseRepository.UpdateAsync(URL.ExpenseAPIPath, expense);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
                ViewBag.Subjects = await _subjectRepository.GetAllAsync(URL.SubjectAPIPath);
                return View(expense);
            }
        }
    }
}
