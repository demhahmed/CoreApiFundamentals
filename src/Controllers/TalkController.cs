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
    [Route("api/camps/{moniker}/talks")]
    [ApiController]
    public class TalkController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _link;

        public TalkController(ICampRepository repository, IMapper mapper, LinkGenerator link)
        {
            _repository = repository;
            _mapper = mapper;
            _link = link;
        }

        [HttpGet]
        public async Task<IActionResult> GetTalks(string moniker)
        {
            try
            {
                var talks = await _repository.GetTalksByMonikerAsync(moniker, true);

                return Ok(_mapper.Map<TalkModel[]>(talks));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTalk(string moniker, int id)
        {
            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);

                if (talk == null) return NotFound("Couldn't find the talk.");

                return Ok(_mapper.Map<TalkModel>(talk));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(string moniker, TalkModel newTalk)
        {
            try
            {
                var talk = _mapper.Map<Talk>(newTalk);
                
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null) return BadRequest("Couldn't find the camp.");
                talk.Camp = camp;
                
                var speaker = await _repository.GetSpeakerAsync(newTalk.Speaker.SpeakerId);
                if (speaker == null) return BadRequest("Couldn't find the speaker.");
                talk.Speaker = speaker;
                
                _repository.Add(talk);

                if (!await _repository.SaveChangesAsync()) return BadRequest("Couldn't save the new talk.");
                var url = _link.GetPathByAction(HttpContext, "GetTalks", values: new {moniker, id = talk.TalkId});
                if (string.IsNullOrEmpty(url)) return BadRequest();
                return Created(url, _mapper.Map<TalkModel>(talk));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(string moniker, int id, TalkModel updatedTalk)
        {
            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
                if (talk == null) return NotFound("Couldn't find the talk.");

                _mapper.Map(updatedTalk, talk);
                if (updatedTalk.Speaker != null)
                {
                    var speaker = await _repository.GetSpeakerAsync(updatedTalk.Speaker.SpeakerId);
                    if (speaker == null) return NotFound("Couldn't find the speaker.");
                    talk.Speaker = speaker;
                }

                if (await _repository.SaveChangesAsync()) return Ok(_mapper.Map<TalkModel>(talk));
                return BadRequest("Couldn't update the talk.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(string moniker, int id)
        {
            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id);
                if (talk == null) return NotFound("Couldn't find the talk.");
                
                _repository.Delete(talk);

                if (await _repository.SaveChangesAsync()) return Ok();
                return BadRequest("Couldn't delete the talk.");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}