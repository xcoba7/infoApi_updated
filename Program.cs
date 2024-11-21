using Microsoft.EntityFrameworkCore;
using InfoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    // options.UseInMemoryDatabase("InfoApi");
    options.UseSqlServer("Server=ESCOBARS-PC\\MSSQLSERVER01;Database=infoDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;");

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


