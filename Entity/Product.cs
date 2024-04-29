using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AspNetWebApi_order_product.Entity
{
    public class Product
    {
        [Column("IdProduct_f")]
        public int Id { get; set; }
        [Column("TitleProduct_f")]
        public string Title { get; set; }
        [Column("PriceProduct_f")]
        public decimal Price { get; set; }
        [Column("QuantityProduct_f")]
        public int Quantity { get; set; }
        
       
        
        [JsonIgnore]
        public ICollection<OrderProduct>? OrderProducts { get; set; }
        public Product()
        {
            Title = string.Empty;
            Price = 0;
            Quantity = 0;
        }

        public override string ToString()
        {
            return $"{Id} - {Title} - {Price} - {Quantity}";
        }
    }
}
