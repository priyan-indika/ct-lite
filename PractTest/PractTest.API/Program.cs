using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PractTest.API.Filters;
using PractTest.API.Middleware;
using PractTest.Application;
using PractTest.Application.DTOValidations;
using PractTest.Application.Interfaces.Customers;
using PractTest.Application.Services;
using PractTest.Domain.Entities;
using PractTest.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => 
{
    options.Filters.Add<ValidationFilter>(); // Add your custom validation filter
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseInMemoryDatabase("TestDb"));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerValidator>();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//    if (!context.Customers.Any())
//    {
//        context.Customers.AddRange(
//            new Customer { Id = 1, Username = "aaaa", Password = "AAAA", Name = "Aaaa", Email = "a@a.com", LoyaltyPoints = 11, CreatedBy = 0 },
//            new Customer { Id = 2, Username = "bbbb", Password = "BBBB", Name = "Bbbb", Email = "b@a.com", LoyaltyPoints = 25, CreatedBy = 0 },
//            new Customer { Id = 3, Username = "cccc", Password = "CCCC", Name = "Cccc", Email = "c@a.com", LoyaltyPoints = 45, CreatedBy = 0 },
//            new Customer { Id = 4, Username = "dddd", Password = "DDDD", Name = "Dddd", Email = "d@a.com", LoyaltyPoints = 74, CreatedBy = 0 },
//            new Customer { Id = 5, Username = "eeee", Password = "EEEE", Name = "Eeee", Email = "e@a.com", LoyaltyPoints = 0, CreatedBy = 0 }
//        );

//        context.SaveChanges();
//    }
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapControllers();

app.Run();
