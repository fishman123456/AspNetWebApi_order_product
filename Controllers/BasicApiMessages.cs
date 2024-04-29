using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApi_order_product.Controllers
{
    public class BasicApiMessages : Controller
    {
        public record StringMessage(string message);
    }
}
