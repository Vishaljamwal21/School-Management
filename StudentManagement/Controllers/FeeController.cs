using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.Models.DTO;
using StudentManagement.Repository;
using StudentManagement.Repository.IRepository;

namespace StudentManagement.Controllers
{
    [Route("api/Fee")]
    [ApiController]
    public class FeeController : ControllerBase
    {
        private readonly IFeeRepository _feeRepository;
        private readonly IMapper _mapper;
        public FeeController(IFeeRepository feeRepository,IMapper mapper)
        {
            _feeRepository = feeRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetFees()
        {
            var fee = _feeRepository.GetFees();
            var feeDTO = _mapper.Map<List<FeeDTO>>(fee);
            return Ok(feeDTO);
        }
        [HttpGet("{feesId:int}", Name = "GetFee")]
        public IActionResult GetFee(int feesId)
        {
            var fee = _feeRepository.GetFee(feesId);
            if (fee == null) return NotFound();//404
            var feeDTO = _mapper.Map<FeeDTO>(fee);
            return Ok(feeDTO);
        }
        [HttpGet("Class/{classId:int}", Name = "GetFeeByClassId")]
        public IActionResult GetFeeByClassId(int classId)
        {
            var fee = _feeRepository.GetFeeByClassID(classId);
            if (fee == null) return NotFound();//404
            var feeDTO = _mapper.Map<List<FeeDTO>>(fee);
            return Ok(feeDTO);
        }
        [HttpPost]
        public IActionResult CreateFee([FromBody] FeeDTO feeDTO)
        {
            if (feeDTO == null) return BadRequest(ModelState);
            if (_feeRepository.FeeExists(feeDTO.FeesId))
            {
                ModelState.AddModelError("", "Fee in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var fee = _mapper.Map<FeeDTO, Fee>(feeDTO);
            if (!_feeRepository.CreateFee(fee))
            {
                ModelState.AddModelError("", $"Something went wrong while saving data:{fee.FeesId}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetFee", new { FeesId = fee.FeesId }, fee);
        }
        [HttpPut]
        public IActionResult UpdateFee([FromBody] FeeDTO feeDTO)
        {
            if (feeDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var fee = _mapper.Map<Fee>(feeDTO);
            if (!_feeRepository.UpdateFee(fee))
            {
                ModelState.AddModelError("", $"Something Went Wrong while Updating the data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(feeDTO);
        }
        [HttpDelete("{feesId:int}")]
        public IActionResult DeleteFee(int feesId)
        {
            if (!_feeRepository.FeeExists(feesId)) return BadRequest();
            var fee = _feeRepository.GetFee(feesId);
            if (fee == null) return BadRequest();
            if (!_feeRepository.DeleteFee(fee))
            {
                ModelState.AddModelError("", "Something Went Wrong while deleting data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
