using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Configuration;
using UdemyRabbitMQ.WebApp.Models;
using UdemyRabbitMQ.WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "productDb");
});

IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();

builder.Services.AddSingleton(sp => new ConnectionFactory()
{
    Uri = new Uri(configuration.GetConnectionString("ConnectionStrings"))
});

builder.Services.AddSingleton<RabitMQClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
