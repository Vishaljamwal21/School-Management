using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.DTO;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Controllers
{
    [Route("api/Exam")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;
        public ExamController(IExamRepository examRepository,IMapper mapper)
        {
            _examRepository = examRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetExam()
        {
            var exam = _examRepository.GetExams();
            var examDTO = _mapper.Map<List<ExamDTO>>(exam);
            return Ok(examDTO);
        }
        [HttpGet("{ExamId:int}", Name = "GetExam")]
        public IActionResult GetExam(int ExamId)
        {
            var exam = _examRepository.GetExam(ExamId);
            if (exam == null) return NotFound();
            var examDTO = _mapper.Map<ExamDTO>(exam);
            return Ok(examDTO);
        }
        [HttpGet("Class/{classId:int}", Name = "GetExamByClassId")]
        public IActionResult GetExamByClassId(int classId)
        {
            var exam = _examRepository.GetExamByClassID(classId);
            if (exam == null) return NotFound();//404
            var examDTO = _mapper.Map<List<ExamDTO>>(exam);
            return Ok(examDTO);
        }
        [HttpGet("Subject/{subjectId:int}", Name = "GetExamBySubjectId")]
        public IActionResult GetExamBySubjectId(int subjectId)
        {
            var exam = _examRepository.GetExamBysubjectId(subjectId);
            if (exam == null) return NotFound();//404
            var examDTO = _mapper.Map<List<ExamDTO>>(exam);
            return Ok(examDTO);
        }
        [HttpPost]
        public IActionResult CreateExam([FromBody] ExamDTO examDTO)
        {
            if (examDTO == null) return BadRequest(ModelState);
            if (_examRepository.ExamExists(examDTO.ExamId))
            {
                ModelState.AddModelError("", "Exam in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var exam = _mapper.Map<ExamDTO, Exam>(examDTO);
            if (!_examRepository.CreateExam(exam))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{exam.ExamId}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetExam", new { ExamId = exam.ExamId }, exam);
        }
        [HttpPut]
        public IActionResult UpdateExam([FromBody] ExamDTO examDTO)
        {
            if (examDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var exam = _mapper.Map<Exam>(examDTO);
            if (!_examRepository.UpdateExam(exam))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(examDTO);
        }
        [HttpDelete("{ExamId:int}")]
        public IActionResult DeleteExam(int ExamId)
        {
            if (!_examRepository.ExamExists(ExamId)) return BadRequest();
            var exam = _examRepository.GetExam(ExamId);
            if (exam == null) return BadRequest();
            if (!_examRepository.DeleteExam(exam))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
