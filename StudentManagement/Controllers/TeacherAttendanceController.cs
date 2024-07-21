using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.DTO;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Controllers
{
    [Route("api/TeacherAttendance")]
    [ApiController]
    public class TeacherAttendanceController : ControllerBase
    {
        private readonly ITeacherAttendanceRepository _teacherAttendanceRepository;
        private readonly IMapper _mapper;
        public TeacherAttendanceController(ITeacherAttendanceRepository teacherAttendanceRepository,IMapper mapper)
        {
            _teacherAttendanceRepository = teacherAttendanceRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetTeacherAttendance()
        {
            var attendance = _teacherAttendanceRepository.GetTeacherAttendances();
            var teacherattendanceDTO = _mapper.Map<List<TeacherAttendanceDTO>>(attendance);
            return Ok(teacherattendanceDTO);
        }
        [HttpGet("{Id:int}", Name = "GetTeacherAttendance")]
        public IActionResult GetTeacherAttendance(int Id)
        {
            var attendance = _teacherAttendanceRepository.GetTeacherAttendance(Id);
            if (attendance == null) return NotFound();//404
            var teacherattendanceDTO = _mapper.Map<TeacherAttendanceDTO>(attendance);
            return Ok(teacherattendanceDTO);
        }
        [HttpGet("Teacher/{teacherId:int}", Name = "GetAttendanceByTeacherId")]
        public IActionResult GetAttendanceByTeacherId(int teacherId)
        {
            var attendance = _teacherAttendanceRepository.GetAttendanceByTeacherId(teacherId);
            if (attendance == null) return NotFound();//404
            var teacherattendanceDTO = _mapper.Map<List<TeacherAttendanceDTO>>(attendance);
            return Ok(teacherattendanceDTO);
        }
        [HttpPost]
        public IActionResult CreateTeacherAttendance([FromBody] TeacherAttendanceDTO teacherAttendanceDTO)
        {
            if (teacherAttendanceDTO == null) return BadRequest(ModelState);
            if (_teacherAttendanceRepository.TeacherAttendanceExists(teacherAttendanceDTO.Id))
            {
                ModelState.AddModelError("", "Teacher Attendance in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var attendance = _mapper.Map<TeacherAttendanceDTO, TeacherAttendance>(teacherAttendanceDTO);
            if (!_teacherAttendanceRepository.CreateTeacherAttendance(attendance))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{attendance.Id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTeacherAttendance", new { Id = attendance.Id }, attendance);
        }
        [HttpPut]
        public IActionResult UpdateTeacherAttendance([FromBody] TeacherAttendanceDTO teacherAttendanceDTO)
        {
            if (teacherAttendanceDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var attendance = _mapper.Map<TeacherAttendance>(teacherAttendanceDTO);
            if (!_teacherAttendanceRepository.UpdateTeacherAttendance(attendance))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(teacherAttendanceDTO);
        }
        [HttpDelete("{Id:int}")]
        public IActionResult DeleteTeacherAttendance(int Id)
        {
            if (!_teacherAttendanceRepository.TeacherAttendanceExists(Id)) return BadRequest();
            var attendance = _teacherAttendanceRepository.GetTeacherAttendance(Id);
            if (attendance == null) return BadRequest();
            if (!_teacherAttendanceRepository.DeleteTeacherAttendance(attendance))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
