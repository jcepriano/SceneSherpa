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
    //Override Exception Page
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Handle 404 Routing
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/Media/48";
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name:"default", pattern:"{controller=Media}/{action=Index}/{id?}");
app.Run();
