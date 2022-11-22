using Microsoft.EntityFrameworkCore;
using Persistence;
using Service.Inteface;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("MasterDatabase");
builder.Services.AddDbContext<DbCabServicesContext>(option =>
option.UseSqlServer(connectionString)
);

builder.Services.AddScoped<IUserDetails, UserService>();
builder.Services.AddScoped<IEncrypt, Encrypt>();
builder.Services.AddScoped<ICabDetail, CabDetailService>();

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
