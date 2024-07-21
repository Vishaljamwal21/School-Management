using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models.DTO;
using StudentManagement.Models;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentController(IStudentRepository studentRepository,IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetStudent()
        {
            var students = _studentRepository.GetStudents();
            var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDTOs);
        }
        [HttpGet("{studentId:int}", Name = "GetStudent")]
        public IActionResult GetStudent(int studentId)
        {
            var student = _studentRepository.GetStudent(studentId);
            if (student == null) return NotFound();//404
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(studentDTO);
        }
        [HttpGet("Class/{classId:int}", Name = "GetStudentByClassId")]
        public IActionResult GetStudentByClassId(int classId)
        {
            var students = _studentRepository.GetStudentByClassId(classId);
            if (students == null) return NotFound();//404
            var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDTOs);
        }
        [HttpPost]
        public IActionResult CreateStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null) return BadRequest(ModelState);
            if (_studentRepository.StudentExists(studentDTO.Name))
            {
                ModelState.AddModelError("", "Student in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var student = _mapper.Map<StudentDTO,Student>(studentDTO);
            if (!_studentRepository.Createstudent(student))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{student.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetStudent", new { studentId = student.StudentId }, student);
        }
        [HttpPut]
        public IActionResult UpdateStudent([FromBody] StudentDTO studentDTO)
        {
            if (studentDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var student = _mapper.Map<Student>(studentDTO);
            if (!_studentRepository.Updatestudent(student))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(studentDTO);
        }
        [HttpDelete("{studentId:int}")]
        public IActionResult DeleteStudent(int studentId)
        {
            if (!_studentRepository.StudentExists(studentId)) return BadRequest();
            var student = _studentRepository.GetStudent(studentId);
            if (student == null) return BadRequest();
            if (!_studentRepository.Deletestudent(student))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
