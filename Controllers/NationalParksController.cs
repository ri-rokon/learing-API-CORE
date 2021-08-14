using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WildAPI.Models;
using WildAPI.Models.Dtos;
using WildAPI.Repository.IRepository;

namespace API_CORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;
        // public NationalParksController( INationalParkRepository npRepo)
        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }


        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            //Auto Mapper Without Di
            //var config = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<NationalPark, NationalParkDto>();
            //});
            //IMapper iMapper = new AutoMapperConfig().Configure().CreateMapper();

            var objList = _npRepo.GetNationalParks();
            var objDto = new List<NationalParkDto>();
            foreach (var obj in objList)
            {
                //Auto Mapper Without DI
                //objDto.Add(iMapper.Map<NationalPark, NationalParkDto>(obj));
                objDto.Add(_mapper.Map<NationalPark, NationalParkDto>(obj));
            }
            //objDto[0].Name = typeof(MyMapper).ToString();
            return Ok(objDto);
        }
        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId"> The Id of the national Park </param>
        /// <returns></returns>

        //[HttpGet("nationalParkId:int")]
        //gives a Route name to call in route
        [HttpGet("nationalParkId:int", Name = "GetNationalPark")]

        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _npRepo.GetNationalPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<NationalPark, NationalParkDto>(obj);
            return Ok(objDto);
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_npRepo.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }

            //not requier because we use [Required] DataAnnotations add in model
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            //The last parameter is the newly created item - or any object you want to return in that field.

            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id });
        }
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_npRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]

        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_npRepo.NationalParkExists(nationalParkId))
            {
                return NotFound();
            }

            var nationalParkObj = _npRepo.GetNationalPark(nationalParkId);
            if (!_npRepo.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }


    }
}
