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
