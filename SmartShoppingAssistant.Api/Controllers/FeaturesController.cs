using Microsoft.AspNetCore.Mvc;
using SmartShoppingAssistant.BusinessLogic.Models;

namespace SmartShoppingAssistant.Api.Controllers
{
    // Tells the frontend which optional features are available on this server
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController(AiSettings aiSettings) : ControllerBase
    {
        [HttpGet]
        public ActionResult<object> Get()
        {
            return Ok(new { aiAssistant = aiSettings.IsConfigured });
        }
    }
}
