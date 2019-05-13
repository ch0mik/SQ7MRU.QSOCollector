using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SQ7MRU.QSOCollector.Services.EQSL;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SQ7MRU.QSOCollector
{
    public class Startup
    {
        public DbContext context;
        private IHostingEnvironment _env;
        private string AppName;
        private string AppVer;
        private ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _env = env;
            _loggerFactory = loggerFactory;
            AppName = Assembly.GetEntryAssembly().GetName().Name;
            AppVer = Assembly.GetEntryAssembly().GetName().Version.ToString(3);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            if (_env.IsDevelopment())
            {
                // Register the Swagger generator, defining 1 or more Swagger documents
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Title = AppName,
                        Version = AppVer,
                        Description = "Service for Collect and Manage QSOs (ADIF Records)",
                        Contact = new Contact() { Url = "https://github.com/ch0mik/SQ7MRU.QSOCollector", Name = "SQ7MRU.QSOCollector" },
                        License = new License() { Url = "https://github.com/ch0mik/SQ7MRU.QSOCollector/blob/master/LICENSE", Name = "Apache License v2.0" }
                    });

                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "Bearer", Enumerable.Empty<string>() }, });
                });
            }

            //Sqlite
            services.AddDbContext<QSOColletorContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            //Sqlite + LazyLoading
            //https://docs.microsoft.com/en-us/ef/core/querying/related-data + http://ralms.net/efcore/lazyload/
            //services.AddDbContext<QSOColletorContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());

            //EQSL.CC Job Service
            services.AddSingleton<IHostedService, EqslUploadService>();
            services.AddSingleton<IHostedService, EqslDownloadService>();

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (_env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppName} V{AppVer}");
                    c.DocExpansion(DocExpansion.None);
                });
            }

            app.UseHsts();
            app.UseCors(option => option
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}