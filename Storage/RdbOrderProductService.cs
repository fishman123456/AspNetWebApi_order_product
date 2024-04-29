using AspNetWebApi_order_product.Entity;
using AspNetWebApi_order_product.Service;

using Microsoft.EntityFrameworkCore;

namespace AspNetWebApi_order_product.Storage
{
    public class RdbOrderProductService : IOrderProductService
    {
        private readonly ApplicationDbContext _db;
        public RdbOrderProductService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<OrderProduct?> Add(OrderProduct orderProduct)
        {
            _db.OrderProducts.Add(orderProduct);
            await _db.SaveChangesAsync();
            return orderProduct;
        }

        public async Task<OrderProduct?> GetById(int id)
        {
            return await _db.OrderProducts.FirstOrDefaultAsync(OrderProduct => OrderProduct.Id == id);
        }

        public async Task<List<OrderProduct>> ListAll()
        {
            return await _db.OrderProducts.ToListAsync();
        }

        public async Task<OrderProduct?> RemoveById(int id)
        {
            OrderProduct? removed = await _db.OrderProducts.FirstOrDefaultAsync(OrderProduct => OrderProduct.Id == id);
            if (removed != null)
            {
                _db.OrderProducts.Remove(removed);
                await _db.SaveChangesAsync();
            }
            return removed;
        }

        public async Task<OrderProduct?> UpdateById(int id, OrderProduct orderProduct)
        {

            OrderProduct? updated = await _db.OrderProducts.FirstOrDefaultAsync(OrderProduct => orderProduct.Id == id);
            if (updated != null)
            {
                updated.Quantity = orderProduct.Quantity;
                await _db.SaveChangesAsync();
            }
            return updated;
        }
    }
}
