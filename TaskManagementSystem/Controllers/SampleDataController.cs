using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleDataController : ControllerBase
    {
        [HttpGet("getHello")]
        public string HelloPrint()
        {
            return "Hello From Test";
        }
    }
}
