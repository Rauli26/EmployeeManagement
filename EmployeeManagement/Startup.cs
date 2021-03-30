using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration config) 
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddMvc(option => option.EnableEndpointRouting = false);       //To add the MVC services to the dependency injection
            services.AddMvc().AddXmlSerializerFormatters();       //To reeturn the data in xml formate.
            //services.AddSingleton < IEmployeeRepository, MockEmployeeRepository > ();  //Register MockEmployeeRepository class with the ASP.NET Core Dependency Injection container using AddSingleton() method
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())     
            {
                //To enable exception handling in development enable UseDeveloperExceptionPage Middleware in the pipeline.
                //Contains Stack Trace, Query String, Cookies and HTTP headers  
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions(); 
                //developerExceptionPageOptions.SourceCodeLineCount = 10;  
                //app.UseDeveloperExceptionPage(developerExceptionPageOptions);
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();   //Keeping UseStaticFiles() 1st middleware so that if static files can be access without going thorgh MVC routing. 
            //app.UseMvcWithDefaultRoute();   //Adding MVC Middleware to application requesting pipeline. 

            app.UseMvc(routes => {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            }
            
            );


            app.Run(async (context) =>                         
            {
                await context.Response.WriteAsync("Hello world!");
                
            });



            //app.UseRouting();

            //app.UseEndpoints(endpoints =>            
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
