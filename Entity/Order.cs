using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using System.Text.Json.Serialization;

namespace AspNetWebApi_order_product.Entity
{
    [Index(nameof(СodeOrder), IsUnique = true)]
    public class Order
    {   
        [Column("IdOrder_f")]
        public int Id { get; set; }
        [Column("СodeOrder_f")]
        public string СodeOrder { get; set; }
        [Column("CreatationDate")]
        public DateTime CreatationDate { get; set; } = DateTime.Now;
        [Column("ClientOrder_f")]
        public string Client  { get; set; }
        
        [JsonIgnore]
        public ICollection<OrderProduct>? OrderProducts { get; set; }
        public Order()
        {
            Client = string.Empty;
        }
        public override string ToString()
        {
            return $"{Id} - {СodeOrder} - {CreatationDate} - {Client}";
        }
    }
}
