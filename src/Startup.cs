using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace SQ7MRU.QSOCollector
{
    public class Startup
    {
        public DbContext context;
        private IHostingEnvironment _env;
        private string AppName;
        private string AppVer;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
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
                        Title = AppName, Version = AppVer, Description = "Service for Collect and Manage QSOs (ADIF Records)",
                        Contact = new Contact() { Url = "https://github.com/ch0mik/SQ7MRU.QSOCollector", Name = "SQ7MRU.QSOCollector" },
                        License = new License() { Url = "https://github.com/ch0mik/SQ7MRU.QSOCollector/blob/master/LICENSE", Name = "Apache License v2.0" }
                    });

                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });
                });
            }

            //Sqlite
            services.AddDbContext<QSOColletorContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            //Sqlite + LazyLoading
            //https://docs.microsoft.com/en-us/ef/core/querying/related-data + http://ralms.net/efcore/lazyload/
            //services.AddDbContext<QSOColletorContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());
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
                });
                
            }

            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}