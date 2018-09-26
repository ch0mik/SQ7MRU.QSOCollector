using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SQ7MRU.QSOCollector
{
    public class Startup
    {
        public DbContext context;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //InMemory : Only for Test/Debug
            //services.AddDbContext<QSOColletorContext>(options => options.UseInMemoryDatabase("SQ7MRU"));

            //Sqlite
            services.AddDbContext<QSOColletorContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            //Sqlite + LazyLoading
            //https://docs.microsoft.com/en-us/ef/core/querying/related-data + http://ralms.net/efcore/lazyload/
            //services.AddDbContext<QSOColletorContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")).UseLazyLoadingProxies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}