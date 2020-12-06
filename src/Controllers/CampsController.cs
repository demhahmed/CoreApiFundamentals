using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _link;

        public CampsController(ICampRepository repository, IMapper mapper, LinkGenerator link)
        {
            _repository = repository;
            _mapper = mapper;
            _link = link;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCamps(bool includeTalks = false)
        {
            try
            {
                var result = await _repository.GetAllCampsAsync(includeTalks);

                return Ok(_mapper.Map<CampModel[]>(result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
        
        [HttpGet("{moniker}")] //    "{moniker:int}"
        public async Task<IActionResult> GetCamp(string moniker)
        {
            try
            {
                var result = await _repository.GetCampAsync(moniker);
                if (result == null) return NotFound();
                return Ok(_mapper.Map<CampModel>(result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
            
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> Search(DateTime Date, bool includeTalks = false)
        {
            try
            {
                var result = await _repository.GetAllCampsByEventDate(Date, includeTalks);
                if (result == null) return NotFound();
                return Ok(_mapper.Map<CampModel[]>(result));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in searching.");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateCamp(CampModel newCamp)
        {
            try
            {
                var found = await _repository.GetCampAsync(newCamp.Moniker);
                if (found != null) return BadRequest("Moniker exists");

                var link = _link.GetPathByAction("GetCamp", "Camps", new {moniker = newCamp.Moniker});
                if (string.IsNullOrEmpty(link)) return BadRequest("Couldn't create a link using the given moniker.");

                var camp = _mapper.Map<Camp>(newCamp);
                _repository.Add(camp);
                if (await _repository.SaveChangesAsync()) return Created(link, _mapper.Map<CampModel>(camp));
                return BadRequest("Something went wrong.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to create new camp.");
            }
        }

        [HttpPut("{moniker}")]
        public async Task<IActionResult> UpdateCamp(string moniker, CampModel camp)
        {
            try
            {
                var oldCamp = await _repository.GetCampAsync(moniker);
                if (oldCamp == null) return NotFound("Couldn't find the camp with the corresponding moniker.");
                _mapper.Map(camp, oldCamp);
                if (await _repository.SaveChangesAsync()) return Ok(_mapper.Map<CampModel>(oldCamp));
                return BadRequest("Something went wrong updating the camp.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to update camp.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) return NotFound("Couldn't find the camp with the corresponding moniker.");
                _repository.Delete(camp);
                if (await _repository.SaveChangesAsync()) return Ok();
                return BadRequest("Something went wrong deleting the camp.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to delete camp.");
            }
        }
    }
}