using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace SceneSherpa
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie(options =>
            //{
            //    options.LoginPath = "/users/google-login";
            //})
            //.AddGoogle(options =>
            //{
            //    options.ClientId = "1012338779715-5mg7jdcgfe7h9pp6ufhkokss1r9hgafu.apps.googleusercontent.com";
            //    options.ClientSecret = "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";
            //});
            services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = "1012338779715-5mg7jdcgfe7h9pp6ufhkokss1r9hgafu.apps.googleusercontent.com";
                    googleOptions.ClientSecret = "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";
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure middleware (e.g., routing, error handling, etc.)

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}