using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using tbb.payments.api.Interfaces;
using tbb.payments.api.Providers;
using tbb.payments.api.Repositories;
using FluentEmail.Core;
using FluentEmail.Smtp;
using FluentEmail.Razor;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

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

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
