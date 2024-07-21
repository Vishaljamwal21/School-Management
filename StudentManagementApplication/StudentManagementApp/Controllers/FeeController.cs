using Microsoft.AspNetCore.Mvc;
using StudentManagementApp.Models;
using StudentManagementApp.Repository.IRepository;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagementApp.Controllers
{
    public class FeeController : Controller
    {
        private readonly IFeeRepository _feeRepository;
        private readonly IClassRepository _classRepository;

        public FeeController(IFeeRepository feeRepository, IClassRepository classRepository)
        {
            _feeRepository = feeRepository;
            _classRepository = classRepository;
        }

        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fees = await _feeRepository.GetAllAsync(URL.FeeAPIPath);
            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            var classDictionary = classes.ToDictionary(c => c.ClassId, c => c.ClassName);

            var result = fees.Select(fee => new
            {
                FeesId = fee.FeesId,
                ClassId = fee.ClassId,
                ClassName = fee.ClassId.HasValue && classDictionary.ContainsKey(fee.ClassId.Value) ? classDictionary[fee.ClassId.Value] : "N/A",
                FeesAmount = fee.FeesAmount
            });

            return Json(new { data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int feesId)
        {
            var fee = await _feeRepository.GetAsync(URL.FeeAPIPath, feesId);
            if (fee == null)
                return Json(new { success = false, message = "Unable to delete the data" });

            await _feeRepository.DeleteAsync(URL.FeeAPIPath, feesId);
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SaveOrUpdate(int? feesId)
        {
            Fee fee = new Fee();
            if (feesId != null)
            {
                fee = await _feeRepository.GetAsync(URL.FeeAPIPath, feesId.Value);
                if (fee == null)
                    return NotFound();
            }

            var classes = await _classRepository.GetAllAsync(URL.ClassAPIPath);
            ViewBag.Classes = classes.Select(c => new { c.ClassId, c.ClassName });

            return View(fee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveOrUpdate(Fee fee)
        {
            if (ModelState.IsValid)
            {
                if (fee.FeesId == 0)
                    await _feeRepository.CreateAsync(URL.FeeAPIPath, fee);
                else
                    await _feeRepository.UpdateAsync(URL.FeeAPIPath, fee);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(fee);
            }
        }
    }
}
