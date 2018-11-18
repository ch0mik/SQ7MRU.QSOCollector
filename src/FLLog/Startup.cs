using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SQ7MRU.FLLog.Client;
using System.IO;
using System.Threading.Tasks;

namespace SQ7MRU.FLLog
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
            services.AddMvc()
                  .AddXmlSerializerFormatters()
                  .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<LocalBufferContext>(options => 
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=LocalBuffor.db"));

            services.AddSingleton<IHostedService, Worker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }
    }
}