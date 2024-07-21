using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.DTO;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Controllers
{
    [Route("api/Subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;
        public SubjectController(ISubjectRepository subjectRepository,IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetSubject()
        {
            var subjectDTO = _subjectRepository.GetSubjects().ToList().Select(_mapper.Map<Subject, SubjectDTO>);
            return Ok(subjectDTO);
        }
        [HttpGet("{subjectId:int}", Name = "GetSubject")]
        public IActionResult GetSubject(int subjectId)
        {
            var subject = _subjectRepository.GetSubject(subjectId);
            if (subject == null) return NotFound();//404
            var SubjectDTO = _mapper.Map<SubjectDTO>(subject);
            return Ok(SubjectDTO);
        }
        [HttpPost]
        public IActionResult CreateSubject([FromBody] SubjectDTO subjectDTO)
        {
            if (subjectDTO == null) return BadRequest(ModelState);
            if (_subjectRepository.SubjectExists(subjectDTO.SubjectName))
            {
                ModelState.AddModelError("", "Subject in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var subject = _mapper.Map<SubjectDTO, Subject>(subjectDTO);
            if (!_subjectRepository.CreateSubject(subject))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{subject.SubjectName}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetSubject", new { subjetId = subject.SubjectId }, subject);
        }
        [HttpPut]
        public IActionResult UpdateSUbject([FromBody] SubjectDTO subjectDTO)
        {
            if (subjectDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var subject = _mapper.Map<Subject>(subjectDTO);
            if (!_subjectRepository.UpdateSubject(subject))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(subjectDTO);
        }
        [HttpDelete("{subjectId:int}")]
        public IActionResult DeleteSubject(int subjectId)
        {
            if (!_subjectRepository.SubjectExists(subjectId)) return BadRequest();
            var subject  = _subjectRepository.GetSubject(subjectId);
            if (subject == null) return BadRequest();
            if (!_subjectRepository.DeleteSubject(subject))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
