using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
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
                  .AddXmlSerializerFormatters();

            //services.AddMvc(options =>
            //{
            //    options.InputFormatters.Add(new XmlSerializerInputFormatter());
            //    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            //})
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            //app.Run((httpContext) =>
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        httpContext.Response.Body = ms;
            //        var stream = new StreamReader(s).ReadToEnd();
            //        var response = httpContext.Response;
            //        response.StatusCode = 200;
            //        response.ContentType = "text/xml";
            //        response.ContentLength = ms.Length;
            //        byte[] bytes = ms.ToArray();
            //        return response.Body.WriteAsync(bytes, 0, bytes.Length);
            //    }
            //});

            app.UseMvc();
            //app.Use(async (context, next) =>
            //{
            //    using (var newBody = new MemoryStream())
            //    {
            //        context.Response.Body = newBody;
            //        await next();
            //        string xml = await FormatResponse(context.Response);

            //        await context.Response.WriteAsync(xml);
            //    }

            //});

            //app.Use(async (context, next) =>
            //{
            //    var newContent = string.Empty;

            //    using (var newBody = new MemoryStream())
            //    {

            //        context.Response.Body = newBody;
            //        await next();
            //        string xml = await FormatResponse(context.Response);


            //        context.Response.Body = new MemoryStream(Encoding.UTF8.GetBytes(xml));

            //        //newBody.Seek(0, SeekOrigin.Begin);


            //        //newContent = new StreamReader(newBody).ReadToEnd();
            //        //newContent += xml;

            //        context.Response.ContentLength = context.Response.Body.Length;
            //        context.Response.ContentType = "text/xml";
            //        await context.Response.WriteAsync(xml);
            //    }
            //});

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