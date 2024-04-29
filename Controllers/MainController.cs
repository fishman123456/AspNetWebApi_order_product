using Microsoft.AspNetCore.Mvc;

using static AspNetWebApi_order_product.Controllers.BasicApiMessages;

namespace AspNetWebApi_order_product.Controllers
{
    
    [Route("api")]
    [ApiController]
    public class MainController : ControllerBase
    {
       
        [HttpGet]
        public StringMessage Index()
        {
            int? port = HttpContext.Request.Host.Port;
            return new StringMessage(message: $"server is running on port {port}");
        }

        
        [HttpGet("ping")]
        public StringMessage Ping()
        {
            return new StringMessage(message: "pong");
        }
    }
}
