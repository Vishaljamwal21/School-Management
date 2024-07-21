using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.DTO;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Controllers
{
    [Route("api/StudentAttendance")]
    [ApiController]
    public class StudentAttendanceController : ControllerBase
    {
        private readonly IStudentAttendanceRepository _studentAttendanceRepository;
        private readonly IMapper _mapper;
        public StudentAttendanceController(IStudentAttendanceRepository studentAttendanceRepository,IMapper mapper)
        {
            _studentAttendanceRepository = studentAttendanceRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetStudentAttendance()
        {
            var attendance = _studentAttendanceRepository.GetStudentAttendances();
            var studentattendanceDTO = _mapper.Map<List<StudentAttendanceDTO>>(attendance);
            return Ok(studentattendanceDTO);
        }
        [HttpGet("{Id:int}", Name = "GetStudentAttendance")]
        public IActionResult GetStudentAttendance(int Id)
        {
            var attendance = _studentAttendanceRepository.GetStudentAttendance(Id);
            if (attendance == null) return NotFound();//404
            var studentattendanceDTO = _mapper.Map<StudentAttendanceDTO>(attendance);
            return Ok(studentattendanceDTO);
        }
        [HttpGet("Class/{classId:int}", Name = "GetAttendanceByClassId")]
        public IActionResult GetAttendanceByClassId(int classId)
        {
            var attendance = _studentAttendanceRepository.GetAttendanceByClassID(classId);
            if (attendance == null) return NotFound();//404
            var studentattendanceDTO = _mapper.Map<List<StudentAttendanceDTO>>(attendance);
            return Ok(studentattendanceDTO);
        }
        [HttpGet("Subject/{subjectId:int}", Name = "GetAttendanceBySubjectId")]
        public IActionResult GetAttendanceBySubjectId(int subjectId)
        {
            var attendance = _studentAttendanceRepository.GetAttendanceByClassID(subjectId);
            if (attendance == null) return NotFound();//404
            var studentattendanceDTO = _mapper.Map<List<StudentAttendanceDTO>>(attendance);
            return Ok(studentattendanceDTO);
        }
        [HttpPost]
        public IActionResult CreateStudentAttendance([FromBody] StudentAttendanceDTO studentAttendanceDTO)
        {
            if (studentAttendanceDTO == null) return BadRequest(ModelState);
            if (_studentAttendanceRepository.StudentAttendanceExists(studentAttendanceDTO.Id))
            {
                ModelState.AddModelError("", "Student Attendance in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var attendance = _mapper.Map<StudentAttendanceDTO, StudentAttendance>(studentAttendanceDTO);
            if (!_studentAttendanceRepository.CreateStudentAttendance(attendance))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{attendance.Id}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetStudentAttendance", new { Id = attendance.Id }, attendance);
        }
        [HttpPut]
        public IActionResult UpdateStudentAttendance([FromBody] StudentAttendanceDTO studentAttendanceDTO)
        {
            if (studentAttendanceDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var attendance = _mapper.Map<StudentAttendance>(studentAttendanceDTO);
            if (!_studentAttendanceRepository.UpdateStudentAttendance(attendance))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(studentAttendanceDTO);
        }
        [HttpDelete("{Id:int}")]
        public IActionResult DeleteStudentAttendance(int Id)
        {
            if (!_studentAttendanceRepository.StudentAttendanceExists(Id)) return BadRequest();
            var attendance = _studentAttendanceRepository.GetStudentAttendance(Id);
            if (attendance == null) return BadRequest();
            if (!_studentAttendanceRepository.DeleteStudentAttendance(attendance))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
