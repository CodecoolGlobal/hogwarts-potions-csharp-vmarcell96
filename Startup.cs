using HogwartsPotions.Models;
using HogwartsPotions.Models.Interfaces;
using HogwartsPotions.Models.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

namespace HogwartsPotions
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:3000",
                            "https://icy-mushroom-0411fdf0f.1.azurestaticapps.net")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddDbContext<HogwartsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews()
                .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IPotionRepository, PotionRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, HogwartsContext context)
        {
            if (env.IsDevelopment())
            {
                if (!context.Database.EnsureCreated())
                {
                    context.Database.Migrate();
                }
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

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Rooms}/{action=Index}/{id?}");
            });
        }
    }
}
