using Bogus;
using Bogus.DataSets;

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

var persons = Enumerable.Range(1, 100).Select(num => new { Id = num, Name = new Faker().Person.FullName }).ToArray();

app.MapGet("/users", (string? filter) => persons.Where(p => string.IsNullOrWhiteSpace(filter) || p.Name.Contains(filter)));

app.MapGet("/users/{id}", (int id) =>
{
    var person = persons.FirstOrDefault(p => p.Id == id);

    return person is not null ? Results.Ok(person) : Results.NotFound();
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
