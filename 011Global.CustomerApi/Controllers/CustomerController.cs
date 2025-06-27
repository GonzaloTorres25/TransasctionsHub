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
       //private readonly IUSAEpayService _usaepayService;
       // private readonly IUnsubscritionService _unsubscritionService;

        public CustomersController(ISubscriptionService customerService/*, IUnsubscritionService unsubscritionServer*/)
        {
            _subscriptionService = customerService;
            //_unsubscritionService = unsubscritionServer;
        }

        /*
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _subscriptionService.GetCustomers();
            return Ok(customers);
        }
        */

        [HttpPost("subscribeCustomer")]
        public async Task<IActionResult> SubscribeCustomer([FromBody] SubscribeRequest request)
        {
            var result = await _subscriptionService.SubscribeCustomer(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        /*
        [HttpPost("unsubscribe/{customerId}")]
        public async Task<IActionResult> UnsubscribeCustomer(int customerId)
        {
            var result = await _unsubscritionService.UnsubscribeCustomer(customerId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        */
    }
}
