using BPMWebConsole.Factory.CustomClaims;
using BPMWebConsole.Factory.CustomSession;
using BPMWebConsole.Models.ConfigScript;
using BPMWebConsole.Models.Entities;
using BundlerMinifier.TagHelpers;
using EmailService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BPMWebConsole
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(opts =>
                opts.UseSqlServer(WebConfig.WebPropertySetting.Instance().DBServer.ServerDB,
                    options => options.MigrationsAssembly("ASI.ICT.BPMWebConsole")));

            EmailConfiguration emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();

            // Limitation for request data of a form
            services.Configure<FormOptions>(opt =>
            {
                opt.ValueLengthLimit = int.MaxValue;
                opt.MultipartBodyLengthLimit = int.MaxValue;
                opt.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                // Password settings
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;

                // Lockout settings (lockoutOnFailure should be true)
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.AllowedForNewUsers = true;

                // User settings
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opt.User.RequireUniqueEmail = true;
            }).AddErrorDescriber<LocalizedIdentityErrorDescriber>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomClaimsFactory>();

            services.ConfigureApplicationCookie(opt =>
            {
                // Cookie settings
                opt.LoginPath = "/Account/Login";
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(WebConfig.WebPropertySetting.Instance().Basic.ExpiredTimeSpan);
                opt.SlidingExpiration = true; // Expired Time Span can be extended if page is refreshed or navigated
            });

            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromMinutes(WebConfig.WebPropertySetting.Instance().Basic.TokenLifespan));

            // Save Session into the memory of ASP.NET Core
            services.AddDistributedMemoryCache();
            services.AddSession(opt =>
            {
                opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opt.Cookie.Name = "Session_BPMWebConsole";
                opt.IdleTimeout = TimeSpan.FromMinutes(WebConfig.WebPropertySetting.Instance().Basic.ExpiredTimeSpan);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ISessionWapper<string>, CaptchaSessionWapper>();

            services.AddControllers().AddNewtonsoftJson(o => o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();

            services.AddBundles(options =>
            {
                options.AppendVersion = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Add SessionMiddleware into Pipeline
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
