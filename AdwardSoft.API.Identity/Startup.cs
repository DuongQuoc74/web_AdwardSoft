using System;
using AdwardSoft.Core.Identity;
using AdwardSoft.DTO.Identity;
using AdwardSoft.ORM.Dapper;
using AdwardSoft.Repositories.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AdwardSoft.API.Authentication.Configurations.AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AdwardSoft.Core.Pattern;
using AdwardSoft.Repositories.Pattern;
using AdwardSoft.Utilities.Formatters;
using Microsoft.IdentityModel.Tokens;
using AdwardSoft.ORM.SearchEngineer.Elastic;
using Serilog;

namespace AdwardSoft.API.Authentication
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

            // Add identity types
            services.AddIdentity<User, Role>();
            //Identity Services
            services.AddTransient<IUserStore<User>, UserStore>();
            services.AddTransient<IRoleStore<Role>, RoleStore>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            //ADS Generic
            services.AddDapper();
            services.AddScoped<IGenericRepository, GenericRepository>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Default User settings.
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            services.AddControllers().AddProtobufFormatters();

            //AutoMapper
            services.AddAutoMapper(typeof(AutoMapping));

            services.AddAuthorization();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = Configuration["ADSSetting:Authentication:AuthServer"];
            //        options.RequireHttpsMetadata = false;
            //        options.ApiName = Configuration["ADSSetting:Authentication:Scope"];
            //    });

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["ADSSetting:Authentication:AuthServer"];
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                    options.RequireHttpsMetadata = false;
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

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
