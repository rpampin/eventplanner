using ElectronNET.API;
using EventPlanner;
using EventPlanner.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MudBlazor.Services;
using Syncfusion.Blazor;
using System.Globalization;
using System.Threading.Tasks;

namespace WebApplication1
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
            services.AddControllers();

            services.AddMudServices();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSyncfusionBlazor();
            services.AddDbContext<AppDBContext>(ops => ops.UseSqlite("Data Source=planner.db"));

            services.AddSingleton<AppState>();
            services.AddTransient<AttachmentService>();
            services.AddTransient<EventService>();
            services.AddTransient<PackageService>();
            services.AddTransient<EventTypeService>();
            services.AddTransient<SupplierTypeService>();
            services.AddTransient<GuestService>();
            services.AddTransient<SupplierService>();
            services.AddTransient<ReportService>();
            services.AddTransient<ConfigurationService>();
            services.AddTransient<EmailService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
            }

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthorization();

            var culture = new CultureInfo("fil-PH");
            culture.NumberFormat.CurrencySymbol = "₱";
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            // Open the Electron-Window here
            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
        }
    }
}
