using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MVC and API controllers
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();   // for API controllers
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<BlobService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Map both MVC and API routes
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();   // required for API endpoints

app.Run();
