using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api1")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello World!");
        }

        [HttpPost]
        public IActionResult Post(JObject payload)
        {
            payload.Add("message", "Hello world!");

            return Ok(payload);
        }
    }
}