using Bogus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var faker = new Faker();
var products = Enumerable.Range(1, 100).Select(num => new { Id = num, Name = faker.Commerce.ProductName() }).ToArray();

app.MapGet("/products", (string? filter) => products.Where(p => string.IsNullOrWhiteSpace(filter) || p.Name.Contains(filter)));

app.MapGet("/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);

    return product is not null ? Results.Ok(product) : Results.NotFound();
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
