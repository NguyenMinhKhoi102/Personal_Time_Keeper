using Microsoft.AspNetCore.Mvc;
using MyProject.Payload.Request;
using MyProject.Payload.Response;
using MyProject.Service;

namespace MyProject.ApiControllers
{
    [Route("api/[controller]")]
    public class ActivityTypeController : ControllerBase
    {
        private readonly IActivityTypeService _activityTypeService;

        public ActivityTypeController(IActivityTypeService activityTypeService)
        {
            _activityTypeService = activityTypeService;
        }

        // GET api/activityType
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _activityTypeService.GetAll();
            return Ok(result);
        }

        // GET api/activityType/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _activityTypeService.GetById(id);
            return Ok(result);
        }

        // POST api/activityType
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ActivityTypeRequest rq)
        {
            if (await _activityTypeService.NameExist(rq.Name))
                return BadRequest(new MessageResponse("Name was existed"));

            var result = await _activityTypeService.Add(rq);
            return result ?
                Ok(new MessageResponse("Add activity type successfully")) :
                BadRequest(new MessageResponse("Add activity type failed"));
        }

        // PUT api/activityType/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ActivityTypeRequest rq)
        {
            var result = await _activityTypeService.Edit(id, rq);
            return result ?
                Ok(new MessageResponse("Edit activity type successfully")) :
                BadRequest(new MessageResponse("Edit activity type failed"));
        }

        // DELETE api/activityType/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _activityTypeService.Delete(id);
            return result ?
                Ok(new MessageResponse("Delete activity type successfully")) :
                BadRequest(new MessageResponse("Delete activity type failed"));
        }
    }
}

