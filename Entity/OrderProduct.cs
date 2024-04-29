using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AspNetWebApi_order_product.Entity
{
    public class OrderProduct
    {
        [Column("IdOrderProduct_f")]
        public int Id { get; set; }
        [Column("QuantityOrderProduct_f")]
        public int Quantity { get; set; }

       
        [JsonIgnore]
        public Order? Order { get; set; }
        public Product? Product { get; set; }
        public override string ToString()
        {
            return $"{Id} - {Quantity}";
        }
    }
}
