using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.DTO;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Controllers
{
    [Route("api/TeacherSubject")]
    [ApiController]
    public class TeacherSubjectController : ControllerBase
    {
        private readonly ITeacherSubjectRepository _teacherSubjectRepository;
        private readonly IMapper _mapper;
        public TeacherSubjectController(ITeacherSubjectRepository teacherSubjectRepository,IMapper mapper)
        {
            _teacherSubjectRepository = teacherSubjectRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetTeacherSubject()
        {
            var teachersubject = _teacherSubjectRepository.GetTeacherSubjects();
            var teachersubjectDTO = _mapper.Map<List<TeacherSubjectDTO>>(teachersubject);
            return Ok(teachersubjectDTO);
        }
        [HttpGet("{Id:int}", Name = "GetTeacherSubject")]
        public IActionResult GetTeacherSubject(int Id)
        {
            var teachersubject = _teacherSubjectRepository.GetTeacherSubject(Id);
            if (teachersubject == null) return NotFound();//404
            var teachersubjectDTO = _mapper.Map<TeacherSubjectDTO>(teachersubject);
            return Ok(teachersubjectDTO);
        }
        [HttpPost]
        public IActionResult CreateTeacherSubject([FromBody] TeacherSubjectDTO teacherSubjectDTO)
        {
            if (teacherSubjectDTO == null) return BadRequest(ModelState);
            if (_teacherSubjectRepository.TeacherSubjectExists(teacherSubjectDTO.Id))
            {
                ModelState.AddModelError("", "Teacher Subject in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var teacherSubject = _mapper.Map<TeacherSubjectDTO, TeacherSubject>(teacherSubjectDTO);
            if (!_teacherSubjectRepository.CreateTeacherSubject(teacherSubject))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{teacherSubject.Id}");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTeacherSubject", new { Id = teacherSubject.Id },teacherSubject);
        }
        [HttpPut]
        public IActionResult UpdateTeacherSubject([FromBody] TeacherSubjectDTO teacherSubjectDTO)
        {
            if (teacherSubjectDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var teachersubject = _mapper.Map<TeacherSubject>(teacherSubjectDTO);
            if (!_teacherSubjectRepository.UpdateTeacherSubject(teachersubject))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(teacherSubjectDTO);
        }
        [HttpDelete("{Id:int}")]
        public IActionResult DeleteTeacherSuject(int Id)
        {
            if (!_teacherSubjectRepository.TeacherSubjectExists(Id)) return BadRequest();
            var teachersubject = _teacherSubjectRepository.GetTeacherSubject(Id);
            if (teachersubject == null) return BadRequest();
            if (!_teacherSubjectRepository.DeleteTeacherSubject(teachersubject))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
