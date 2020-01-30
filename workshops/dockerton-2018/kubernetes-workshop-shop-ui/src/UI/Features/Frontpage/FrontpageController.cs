using Microsoft.AspNetCore.Mvc;

namespace DFDS.UI.Features.Frontpage
{
    [Route("")]
    public class FrontpageController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}