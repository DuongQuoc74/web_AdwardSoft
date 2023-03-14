using AdwardSoft.Core.Pattern;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.ORM.SearchEngineer.Elastic;
using AdwardSoft.Repositories.Pattern;
using AdwardSoft.Utilities.Formatters;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace AdwardSoft.API.Core
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
            //ADS Redis            
            services.AddRedisCache(Configuration);
            services.AddScoped<IRedisRepositoty, RedisRepository>();

            //ADS Generic
            services.AddDapper();
            services.AddScoped<IGenericRepository, GenericRepository>();

            //ADS MongoDb
            services.AddMongoDb();
            services.AddScoped<IMongoDbRepository, MongoDbRepository>();

            services.AddControllers().AddProtobufFormatters();

            services.AddAuthorization();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = Configuration["ADSSetting:Authentication:AuthServer"];
            //        options.RequireHttpsMetadata = false;
            //        options.ApiName = Configuration["ADSSetting:Authentication:Scope"];
            //    });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["ADSSetting:Authentication:AuthServer"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                    options.RequireHttpsMetadata = false;
                });


            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
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
                app.UseHsts();
            }

            
            app.UseStaticFiles();
            //serilog
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
