# AspNetWebApi_order_product
 Экзамен ASP_NET_WEB_API
Оглавление
Техническое задание «Разработка веб-API для сервиса приема заказов покупателей».	2
Контроллеры	6
Создание сущностей.	10
Создание миграции	11
Создание интерфейсов	13
Методы для работы с базой данных RDB	14
DBContext	18
Файл настройки соединения с базой данных	18
Program (запуск приложения)	18
Ссылка на GitHub	19

 
Техническое задание «Разработка веб-API для сервиса приема заказов покупателей».
1. Формулировка задания.
Товар представляет собой объект товара, который можно заказать на нашем сайте. Товар имеет поля «Название», «Стоимость», «Присутствует ли в наличии на складе».
Товар В заказе представляет собой объект, ссылающийся на товар и на заказ, а также содержащий поле «кол-во товаров в заказе»
Заказ представляет собой заявку на покупку товара. Включает в себя поля «уникальный шифр заказа», «имя клиента», «дата формирования заказа», «список товаров заказа».
api должен позволять:
	добавлять/удалять/редактировать/получать список товаров
	получать товар по id
	получать товар по названию
	формировать заказы на товары
	получать список всех заказов
	получать заказ по id
	удалять заказ по id
2. Формат общения
api осуществляет общение с клиентом посредством JSON, следовательно является JSON-api. Используется маршрутизация на основе url и методов запросов.
3. Стек технологий
C# + ASP
EF
MS SQL Server / MySQL / PostgreSQL
Порядок сдачи.
1. Работа выполняется в течение двух часов (120 минут)
2. Студент предоставляет выполненную работу, демонстрируя программу, исходный код, БД
3. Студент отвечает на вопросы защиты
 
 

 

 












Контроллеры
1.	Проверка работы сервера
using Microsoft.AspNetCore.Mvc;
using static AspNetWebApi_order_product.Controllers.BasicApiMessages;
namespace AspNetWebApi_order_product.Controllers

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

2.	Контроллер заказов
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

3.	Контроллер продуктов


using AspNetWebApi_order_product.Entity;
using AspNetWebApi_order_product.Service;
using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApi_order_product.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            
            _productService = productService;
        }
        [HttpGet]
        public async Task<List<Product>> ListAll()
        {
            return await _productService.ListAll();
        }

        [HttpGet("{id:int}")]
        public async Task<Product?> GetById(int id)
        {
            Product? product = await _productService.GetById(id);
            if (product == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            return product;
        }
        [HttpPost]
        public async Task<Product?> Add(Product product)
        {
            Product? result = await _productService.Add(product);
            if (result == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            return result;
        }

        [HttpDelete("{id:int}")]
        public async Task<Product?> RemoveById(int id)
        {
            Product? product = await _productService.RemoveById(id);
            if (product == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            return product;
        }

        [HttpPatch("{id:int}")]
        public async Task<Product?> UpdateById(int id, Product product)
        {
            Product? updated = await _productService.UpdateById(id, product);
            if (updated == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            return updated;
        }
    }
}

4.	Таблица связи заказы – продукты

using AspNetWebApi_order_product.Entity;
using AspNetWebApi_order_product.Service;

using Microsoft.AspNetCore.Mvc;

namespace AspNetWebApi_order_product.Controllers
{

        [Route("api/orderproduct")]
        [ApiController]
        public class OrderProductController : ControllerBase
        {
            private readonly IOrderProductService _orderProductService;
            public OrderProductController(IOrderProductService orderProductService)
            {
                // IoC-контейнер при создании контроллера
                // автоматически добавит нужную зависимость (если она есть)
                _orderProductService = orderProductService;
            }
            [HttpGet]
            public async Task<List<OrderProduct>> ListAll()
            {
                return await _orderProductService.ListAll();
            }

            [HttpGet("{id:int}")]
            public async Task<OrderProduct?> GetById(int id)
            {
                OrderProduct? orderProduct = await _orderProductService.GetById(id);
                if (orderProduct == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                return orderProduct;
            }
            [HttpPost]
            public async Task<OrderProduct?> Add(OrderProduct orderProduct)
            {
                OrderProduct? result = await _orderProductService.Add(orderProduct);
                if (result == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
                return result;
            }

            [HttpDelete("{id:int}")]
            public async Task<OrderProduct?> RemoveById(int id)
            {
                OrderProduct? orderProduct = await _orderProductService.RemoveById(id);
                if (orderProduct == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                return orderProduct;
            }

            [HttpPatch("{id:int}")]
            public async Task<OrderProduct?> UpdateById(int id, OrderProduct orderProduct)
            {
                OrderProduct? updated = await _orderProductService.UpdateById(id, orderProduct);
                if (updated == null)
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
                return updated;
            }
        }
    }



Создание сущностей.

1.	Заказы

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

2.	Продукты

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

3.	Заказы – Продукты


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

Создание миграции

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetWebApi_order_product.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    IdOrder_f = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    СodeOrder_f = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientOrder_f = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.IdOrder_f);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    IdProduct_f = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleProduct_f = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceProduct_f = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityProduct_f = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.IdProduct_f);
                });

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    IdOrderProduct_f = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuantityOrderProduct_f = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => x.IdOrderProduct_f);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "IdOrder_f");
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "IdProduct_f");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId",
                table: "OrderProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_СodeOrder_f",
                table: "Orders",
                column: "СodeOrder_f",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}

Создание интерфейсов
1.	Заказы

using AspNetWebApi_order_product.Entity;

namespace AspNetWebApi_order_product.Service
{
    public interface IOrderService
    {
       
        Task<Order?> Add(Order order);

        
        Task<List<Order>> AddRange(List<Order> orders);

       
        Task<Order?> GetById(int id);

       
        Task<List<Order>> ListAll();

        
        Task<Order?> RemoveById(int id);

       
        Task<Order?> UpdateById(int id, Order order);
    }
}

2.	Продукты

using AspNetWebApi_order_product.Entity;

namespace AspNetWebApi_order_product.Service
{
    public interface IProductService
    {
        
        Task<Product?> Add(Product product);

      
        Task<Product?> GetById(int id);

      
        Task<List<Product>> ListAll();

       
        Task<Product?> RemoveById(int id);

       
        Task<Product?> UpdateById(int id, Product product);
    }
}

3.	Заказы – Продукты

using AspNetWebApi_order_product.Entity;

namespace AspNetWebApi_order_product.Service
{
    public interface IOrderProductService
    {
        
        Task<OrderProduct?> Add(OrderProduct orderProduct);

        
        Task<OrderProduct?> GetById(int id);

        
        Task<List<OrderProduct>> ListAll();

       
        Task<OrderProduct?> RemoveById(int id);

        
        Task<OrderProduct?> UpdateById(int id, OrderProduct orderProduct);
    }
}

Методы для работы с базой данных RDB

1.	Заказы

using AspNetWebApi_order_product.Entity;
using AspNetWebApi_order_product.Service;

using Microsoft.EntityFrameworkCore;

namespace AspNetWebApi_order_product.Storage
{
    public class RdbOrderService : IOrderService
    {
        private readonly ApplicationDbContext _db;


        public RdbOrderService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Order?> Add(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> AddRange(List<Order> orders)
        {
            await _db.Orders.AddRangeAsync(orders);
            await _db.SaveChangesAsync();
            return orders;
        }

        public async Task<Order?> GetById(int id)
        {
            return await _db.Orders.FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<List<Order>> ListAll()
        {
            return await _db.Orders.ToListAsync();
        }

        public async Task<Order?> RemoveById(int id)
        {
            Order? removed = await _db.Orders.FirstOrDefaultAsync(order => order.Id == id);
            if (removed != null)
            {
                _db.Orders.Remove(removed);
                await _db.SaveChangesAsync();
            }
            return removed;
        }

        public async Task<Order?> UpdateById(int id, Order order)
        {
            Order? updated = await _db.Orders.FirstOrDefaultAsync(order => order.Id == id);
            if (updated != null)
            {
                updated.СodeOrder = order.СodeOrder;
                await _db.SaveChangesAsync();
            }
            return updated;
        }
    }
}



2.	Продукты

using AspNetWebApi_order_product.Entity;
using AspNetWebApi_order_product.Service;

using Microsoft.EntityFrameworkCore;

namespace AspNetWebApi_order_product.Storage
{
    public class RdbProductService : IProductService
    {
        private readonly ApplicationDbContext _db;

        public RdbProductService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Product?> Add(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public Task<Product?> GetById(int id)
        {
            return _db.Products.FirstOrDefaultAsync(product => product.Id == id);
        }

        public async Task<List<Product>> ListAll()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product?> RemoveById(int id)
        {
            Product? removed = await _db.Products.FirstOrDefaultAsync(product => product.Id == id);
            if (removed != null)
            {
                _db.Products.Remove(removed);
                await _db.SaveChangesAsync();
            }
            return removed;
        }

        public async Task<Product?> UpdateById(int id, Product product)
        {
            Product? update = await _db.Products.FirstOrDefaultAsync(product => product.Id == id);
            if (update != null)
            {
                update.Price = product.Price;
                update.Quantity = product.Quantity;
                update.Title = product.Title;
                update.Quantity = product.Quantity;
                await _db.SaveChangesAsync();
            }
            return update;
        }
    }
}

3.	Заказы – Продукты


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

DBContext

using AspNetWebApi_order_product.Entity;
using Microsoft.EntityFrameworkCore;

namespace AspNetWebApi_order_product.Storage
{
    public class ApplicationDbContext:DbContext
    { 
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            string useConnection = configuration.GetSection("UseConnection").Value ?? "DefaultConnection";
            string? connectionString = configuration.GetConnectionString(useConnection);
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}

Файл настройки соединения с базой данных 
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "UseConnection": "LocalDbConnection",
  "ConnectionStrings": {
    "LocalDbConnection": "Data Source=FISHMAN\\SQLEXPRESS; Initial Catalog=client_order_db; Integrated Security=SSPI; Timeout=5; TrustServerCertificate=true"
  }
}

Program (запуск приложения)
using AspNetWebApi_order_product.Service;
using AspNetWebApi_order_product.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();                                  // добавление классов контроллеров в IoC-контейнер
builder.Services.AddDbContext<ApplicationDbContext>();              // добавим DbContext
builder.Services.AddTransient<IOrderService, RdbOrderService>();
builder.Services.AddTransient<IProductService, RdbProductService>();
builder.Services.AddTransient<IOrderProductService, RdbOrderProductService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();



Ссылка на GitHub
https://github.com/fishman123456/AspNetWebApi_order_product

