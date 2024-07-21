using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.DTO;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;
        public TeacherController(ITeacherRepository teacherRepository,IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetTeacher()
        {
            var teacher = _teacherRepository.GetTeachers();
            var teacherDTO = _mapper.Map<List<TeacherDTO>>(teacher);
            return Ok(teacherDTO);
        }
        [HttpGet("{teacherId:int}", Name = "GetTeacher")]
        public IActionResult GetTeacher(int teacherId)
        {
            var teacher = _teacherRepository.GetTeacher(teacherId);
            if (teacher == null) return NotFound();//404
            var teacherDTO = _mapper.Map<TeacherDTO>(teacher);
            return Ok(teacherDTO);
        }
        [HttpPost]
        public IActionResult CreateTeacher([FromBody] TeacherDTO teacherDTO)
        {
            if (teacherDTO == null) return BadRequest(ModelState);
            if (_teacherRepository.TeacherExists(teacherDTO.Name))
            {
                ModelState.AddModelError("", "Teacher in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var teacher = _mapper.Map<TeacherDTO, Teacher>(teacherDTO);
            if (!_teacherRepository.Createteacher(teacher))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{teacher.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTeacher", new { teacherId = teacher.TeacherId }, teacher);
        }
        [HttpPut]
        public IActionResult UpdateTeacher([FromBody] TeacherDTO teacherDTO)
        {
            if (teacherDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var teacher = _mapper.Map<Teacher>(teacherDTO);
            // Set the Role property to a non-null value
            teacher.Role = "Teacher";
            if (!_teacherRepository.Updateteacher(teacher))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(teacherDTO);
        }
        [HttpDelete("{teacherId:int}")]
        public IActionResult DeleteTeacher(int teacherId)
        {
            if (!_teacherRepository.TeacherExists(teacherId)) return BadRequest();
            var teacher = _teacherRepository.GetTeacher(teacherId);
            if (teacher == null) return BadRequest();
            if (!_teacherRepository.Deleteteacher(teacher))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
