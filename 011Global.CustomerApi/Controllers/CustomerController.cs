using _011Global.CustomerApplication.DTO;
using _011Global.CustomerApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace _011Global.CustomerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService; 
        private readonly IUnsubscritionService _unsubscritionService;

        public CustomersController(ISubscriptionService customerService, IUnsubscritionService unsubscritionServer)
        {
            _subscriptionService = customerService;
            _unsubscritionService = unsubscritionServer;
        }

        [HttpPost("subscribeCustomer")]
        public async Task<IActionResult> SubscribeCustomer([FromBody] SubscribeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _subscriptionService.SubscribeCustomer(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("unsubscribe/{customerId}")]
        public async Task<IActionResult> UnsubscribeCustomer(string email)
        {
            var result = await _unsubscritionService.UnsubscribeCustomer(email);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
