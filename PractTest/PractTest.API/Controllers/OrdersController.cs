using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PractTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public OrdersController() { }

        [HttpGet]
        public ActionResult Get()
        {
            var i = 10;
            var j = 0;
            var result = i / j;
            return Ok(result);
        }
    }
}
