using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using prjToDoList.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// =========== Add DBcontext based on connection string ===========

builder.Services.AddDbContext<demoDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("demoDBConnection"));
});

// =========== Add Session / Cookie Service ===========

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session DueTime
});

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

// activate session service before use routing
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ToDo}/{action=List}/{id?}");

app.Run();
