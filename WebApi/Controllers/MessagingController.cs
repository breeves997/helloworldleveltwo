using MessagingService.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class MessagingController : ControllerBase
    {
        private readonly IMessagingService _msgSvc;

        public MessagingController(IMessagingService msgSvc)
        {
            this._msgSvc = msgSvc;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Message message)
        {
            var success = await _msgSvc.SendMessage("Ben Reeves", message);
            if (success)
                return new OkResult();
            else
                return new NoContentResult();
        }
    }
}
