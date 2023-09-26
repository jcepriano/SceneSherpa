using Markdig;
using Microsoft.EntityFrameworkCore;
using SceneSherpa.DataAccess;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SceneSherpaContext>(
    options =>
        options
            .UseNpgsql(
                builder.Configuration["SCENESHERPA_DBCONNECTIONSTRING"]
                    ?? throw new InvalidOperationException(
                        "Connection string 'SceneSherpaDbNotFound' not found."
                    )
            )
            .UseSnakeCaseNamingConvention()
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name:"default", pattern:"{controller=Media}/{action=Index}/{id?}");
app.Run();
