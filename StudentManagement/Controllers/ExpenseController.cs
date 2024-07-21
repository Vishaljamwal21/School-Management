using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.DTO;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Controllers
{
    [Route("api/Expense")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;
        public ExpenseController(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetExpense()
        {
            var expense = _expenseRepository.GetExpenses();
            var expenseDTO = _mapper.Map<List<ExpenseDTO>>(expense);
            return Ok(expenseDTO);
        }
        [HttpGet("{ExpenseId:int}", Name = "GetExpense")]
        public IActionResult GetExpense(int ExpenseId)
        {
            var expense = _expenseRepository.GetExpense(ExpenseId);
            if (expense == null) return NotFound();
            var expenseDTO = _mapper.Map<ExpenseDTO>(expense);
            return Ok(expenseDTO);
        }
        [HttpGet("Class/{classId:int}", Name = "GetExprnseByClassId")]
        public IActionResult GetExprnseByClassId(int classId)
        {
            var expense = _expenseRepository.GetExpenseByClassID(classId);
            if (expense == null) return NotFound();//404
            var expenseDTO = _mapper.Map<List<ExpenseDTO>>(expense);
            return Ok(expenseDTO);
        }
        [HttpGet("Subject/{subjectId:int}", Name = "GetExpenseBySubjectId")]
        public IActionResult GetExpenseBySubjectId(int subjectId)
        {
            var expense = _expenseRepository.GetExpenseBysubjectId(subjectId);
            if (expense == null) return NotFound();//404
            var expenseDTO = _mapper.Map<List<ExpenseDTO>>(expense);
            return Ok(expenseDTO);
        }
        [HttpPost]
        public IActionResult CreateExpense([FromBody]ExpenseDTO expenseDTO)
        {
            if (expenseDTO == null) return BadRequest(ModelState);
            if(_expenseRepository.ExpenseExists(expenseDTO.ExpenseId))
            {
                ModelState.AddModelError("", "Expense in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var expense = _mapper.Map<ExpenseDTO, Expense>(expenseDTO);
            if (!_expenseRepository.CreateExpense(expense))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{expense.ExpenseId}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetExpense", new { ExpenseId = expense.ExpenseId }, expense);
        }
        [HttpPut]
        public IActionResult UpdateExpense([FromBody] ExpenseDTO expenseDTO)
        {
            if (expenseDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var expense = _mapper.Map<Expense>(expenseDTO);
            if (!_expenseRepository.UpdateExpense(expense))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(expenseDTO);
        }
        [HttpDelete("{ExpenseId:int}")]
        public IActionResult DeleteExpense(int ExpenseId)
        {
            if (!_expenseRepository.ExpenseExists(ExpenseId)) return BadRequest();
            var expense = _expenseRepository.GetExpense(ExpenseId);
            if (expense == null) return BadRequest();
            if (!_expenseRepository.DeleteExpense(expense))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
