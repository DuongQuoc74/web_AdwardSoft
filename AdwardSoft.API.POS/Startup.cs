using AdwardSoft.API.POS.Configurations.AutoMapper;
using AdwardSoft.API.POS.SearchEngineer.Elastic;
using AdwardSoft.Core.Pattern;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.ORM.SearchEngineer.Elastic;
using AdwardSoft.Repositories.Pattern;
using AdwardSoft.Utilities.Formatters;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;


namespace AdwardSoft.API.POS
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

            //ADS Elastic
            services.AddElasticSearch(Configuration);
            // Add our hosted service which will create our indices and mapping
            // asynchronously on startup
            services.AddHostedService<ElasticsearchHostedService>();

            services.AddControllers().AddProtobufFormatters();

            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapping));

            services.AddAuthorization();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["ADSSetting:Authentication:AuthServer"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
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
