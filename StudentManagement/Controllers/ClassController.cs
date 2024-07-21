using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.DTO;
using StudentManagement.Repository.IRepository;
using System.Runtime.InteropServices;

namespace StudentManagement.Controllers
{
    [Route("api/Class")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;
        public ClassController(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetClass()
        {
            var classDTO = _classRepository.GetClasses().ToList().Select(_mapper.Map<Class, ClassDTO>);
            return Ok(classDTO);
        }
        [HttpGet("{classId:int}", Name = "GetClass")]
        public IActionResult GetClass(int classId)
        {
            var classs = _classRepository.GetClass(classId);
            if (classs == null) return NotFound();//404
            var ClassDTO = _mapper.Map<ClassDTO>(classs);
            return Ok(ClassDTO);
        }
        [HttpGet("ByName/{className}", Name = "GetClassByName")]
        public IActionResult GetClassByName(string className)
        {
            var classExists = _classRepository.ClassExists(className);
            if (!classExists)
            {
                return NotFound(); // Return 404 if class does not exist
            }
            var classEntity = _classRepository.GetClassByName(className);
            var classDTO = _mapper.Map<ClassDTO>(classEntity);
            return Ok(classDTO); 
        }

        [HttpPost]
        public IActionResult CreateClass([FromBody]ClassDTO classDTO)
        {
            if (classDTO == null) return BadRequest(ModelState);
            if(_classRepository.ClassExists(classDTO.ClassName))
            {
                ModelState.AddModelError("", "Class in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var classs = _mapper.Map<ClassDTO, Class>(classDTO);
            if(!_classRepository.CreateClass(classs))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{classs.ClassName}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetClass", new { classId = classs.ClassId }, classs);
        }
        [HttpPut]
        public IActionResult UpdateClass([FromBody] ClassDTO classDTO)
        {
            if (classDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var classs = _mapper.Map<Class>(classDTO);
            if (!_classRepository.UpdateClass(classs))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(classDTO);
        }
        [HttpDelete("{classId:int}")]
        public IActionResult DeleteClass(int classId)
        {
            if (!_classRepository.ClassExists(classId)) return BadRequest();
            var classs = _classRepository.GetClass(classId);
            if (classs == null) return BadRequest();
            if (!_classRepository.DeleteClass(classs))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
