using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParkyAPI.Data;
using ParkyAPI.Mapper;
using ParkyAPI.Repository;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ParkyAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) //  ���� ������ ���� ���������� �� ����� ������ ����������
        {
            //�������� ��������� ��������
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(options => // ���� ���������� �� �� ��������
            options.UseSqlServer(Configuration.GetConnectionString("constr"))  // �������� ����������� ������
            );
            services.AddAutoMapper(typeof(ParkyMappings)); // ��� ������� �������� �� ��������� ��� �������� ������ �������� 
            services.AddScoped<INationalParkRepository, NationalParkRepository>(); // ���� ���������� ���� ��������� ���� ������� ������



            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "ParkyAPI", Version = "v1" });
                c.SwaggerDoc("ParkiOpenAPISpec", new OpenApiInfo // ��� �� �������� ������������� ���� �� ���� ���� ���� ��������
                {
                    Title = "ParkyAPI",
                    Version = "1",
                    Contact = new OpenApiContact()
                    {
                        Email="muhannad.oqba@gmail.com",
                        Name ="Muhannad Taha",
                        Url = new Uri("https://www.facebook.com/muhannadtaha.official")
                    },
                    Description = "Parki API Documentaion",
                    License = new OpenApiLicense()
                    {
                        Url = new Uri("https://www.facebook.com/muhannadtaha.official"),
                        Name = "Esolution Teams License"

                    }
                });;

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // ��� ��������� ���� ���� ������� ������ �� �� �������� ��� ����
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile); //��� ���� ����� ������ ���� ��� ����� ������ �� �� ���� �������� AppContext.BaseDirectory
                c.IncludeXmlComments(xmlCommentsFullPath);
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/ParkiOpenAPISpec/swagger.json", "ParkyAPI 1"));
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
