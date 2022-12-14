using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Repository;
using Service.Inteface;
using Service.Services;
using System.Text;

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
builder.Services.AddScoped<ICabBooking, CabBookingService>();
builder.Services.AddScoped<IGenerateToken , GenerateTokenServices>();
builder.Services.AddScoped<ISendNotification, SendNotificationService>();
builder.Services.AddScoped<IPagination, UserService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Cab_Admin",
         policy => policy.RequireRole("Cab_Admin"));
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Customer",
         policy => policy.RequireRole("Customer"));
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

app.Run();
