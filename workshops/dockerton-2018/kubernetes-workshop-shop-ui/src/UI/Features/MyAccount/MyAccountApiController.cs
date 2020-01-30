using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DFDS.UI.Features.MyAccount
{
    [Route("api/myaccount")]
    [ApiController]
    public class MyAccountApiController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public MyAccountApiController(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        [HttpGet("{id}/profile")]
        public async Task<IActionResult> GetProfile(string id)
        {
            var url = _configuration["CRM_SERVICE_URL"];
            var response = await _client.GetAsync($"{url}/api/customers/{id}");
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, response.Content.Headers.ContentType.MediaType);
        }

        [HttpGet("{id}/orders")]
        public async Task<IActionResult> GetOrders(string id)
        {
            var url = _configuration["ORDER_SERVICE_URL"];
            var response = await _client.GetAsync($"{url}/api/orders");
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, response.Content.Headers.ContentType.MediaType);
        }

        [HttpGet("{id}/recommendations")]
        public async Task<IActionResult> GetRecommendations(string id)
        {
            var url = _configuration["RECOMMENDATION_SERVICE_URL"];
            var response = await _client.GetAsync($"{url}/api/customers/{id}/recommendations");
            var content = await response.Content.ReadAsStringAsync();

            return Content(content, response.Content.Headers.ContentType.MediaType);
        }
    }
}