using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Providers;
using tbb.payments.api.Repositories;
using tbb.payments.api.Services;
using tbb.payments.api.Models;
using FluentEmail.Core;
using FluentEmail.Smtp;
using FluentEmail.Razor;
using Square;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using Square.Authentication;
using tbb.payments.api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure database connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register dependencies
builder.Services.AddScoped<IPaymentProvider, PaymentProvider>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>(provider => new PaymentRepository(connectionString));
builder.Services.AddScoped<IEmailService, EmailService>();

// Configure FluentEmail
builder.Services
    .AddFluentEmail("your-email@example.com")
    .AddRazorRenderer()
    .AddSmtpSender(new SmtpClient("smtp.example.com")
    {
        Port = 587,
        Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-email-password"),
        EnableSsl = true,
    });

// Register Square settings
builder.Services.Configure<SquareSettings>(builder.Configuration.GetSection("SquareSettings"));

// Register the Square client
builder.Services.AddSingleton<ISquareClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<SquareSettings>>().Value;
    var environment = settings.Environment.ToLower() == "production" ? Square.Environment.Production : Square.Environment.Sandbox;
    var bearerAuthCredentials = new BearerAuthModel. Builder(settings.AccessToken). Build();
    return new SquareClient.Builder()
        .Environment(environment)
        .BearerAuthCredentials(bearerAuthCredentials)
        .Build();
});

// Register refund provider
builder.Services.AddScoped<IRefundProvider, RefundProvider>();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Enable detailed error messages
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>(); // Use the error handling middleware

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAllOrigins"); // Enable CORS

app.MapControllers();

app.Run();
