using AspNetWebApi_order_product.Entity;
using AspNetWebApi_order_product.Service;

using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApi_order_product.Controllers
{

        
        [Route("api/order")]
        [ApiController]
        public class OrderController : ControllerBase
        {
           
            private readonly IOrderService _orderService;

            public OrderController(IOrderService orderService)
            {
                _orderService = orderService;
            }

            [HttpGet]
            public async Task<List<Order>> ListAll()
            {
                return await _orderService.ListAll();
            }

            [HttpGet("{id:int}")]
            public async Task<Order?> GetById(int id)
            {
                Order? order = await _orderService.GetById(id);
                if (order == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                return order;
            }

            [HttpPost]
            public async Task<Order?> Add(Order order)
            {
                Order? result = await _orderService.Add(order);
                if (result == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
                return result;
            }

           

            [HttpDelete("{id:int}")]
            public async Task<Order?> RemoveById(int id)
            {
                Order? order = await _orderService.RemoveById(id);
                if (order == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                return order;
            }

            [HttpPatch("{id:int}")]
            public async Task<Order?> UpdateById(int id, Order order)
            {
                Order? updated = await _orderService.UpdateById(id, order);
                if (updated == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
                return updated;
            }
        }
    }
