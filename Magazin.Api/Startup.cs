using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Magazin.Data;
using Microsoft.EntityFrameworkCore;
using Magazin.Data.IRepositories;
using Magazin.Data.Repositories;
using Magazin.Domain;

namespace Magazin.Api
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
            services.AddDbContext<MagazinContext>(options =>
                options.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=Magazin;Integrated Security=SSPI; TrustServerCertificate = True; ApplicationIntent = ReadWrite", b => b.MigrationsAssembly("Magazin.Api")));

            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IFacturaRepository, FacturaRepository>();
            services.AddTransient<IDateLivrareRepository, DateLivrareRepository>();
            services.AddTransient<PublishCartWorkflow>();
            services.AddTransient<PublishFacturaWorkflow>();
            //services.AddSingleton<IEventSender, ServiceBusTopicEventSender>();

            //services.AddTransient<PublishGradeWorkflow>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Magazin.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Magazin.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
