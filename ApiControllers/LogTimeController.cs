using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Payload.Request;
using MyProject.Payload.Response;
using MyProject.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.ApiControllers
{
    [Route("api/[controller]")]
    public class LogTimeController : Controller
    {

        private readonly ILogTimeService _logTimeService;

        public LogTimeController(ILogTimeService logTimeService)
        {
            _logTimeService = logTimeService;
        }

        // GET: api/logTime
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (accountIdClaim == null)
                return Unauthorized(new MessageResponse("User not logged in"));

            int accountId = int.Parse(accountIdClaim.Value);

            var result = await _logTimeService.GetByAccountId(accountId);

            return Ok(result);
        }

        // GET api/logTime/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _logTimeService.GetById(id);
            return Ok(result);
        }

        // POST api/logTime
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LogTimeRequest rq)
        {
            var accountIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (accountIdClaim == null)
                return Unauthorized(new MessageResponse("User not logged in"));

            int accountId = int.Parse(accountIdClaim.Value);

            var result = await _logTimeService.Add(accountId, rq);

            return result ?
                Ok(new MessageResponse("Add log time successfully")) :
                BadRequest(new MessageResponse("Add log time failed"));
        }

        // PUT api/logTime/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LogTimeRequest rq)
        {
            var result = await _logTimeService.Edit(id, rq);

            return result ?
                Ok(new MessageResponse("Edit log time successfully")) :
                BadRequest(new MessageResponse("Edit log time failed"));
        }

        // DELETE api/logTime/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _logTimeService.Delete(id);

            return result ?
                Ok(new MessageResponse("Delete log time successfully")) :
                BadRequest(new MessageResponse("Delete log time failed"));
        }
    }
}

