using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using APILivraria.Data;
using APILivraria.Repositories;
using APILivraria.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;

namespace APILivraria
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<ILivroRepository, LivroRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ILivroImpressoRepository, LivroImpressoRepository>();
            services.AddScoped<ITipoEncadernacaoRepository, TipoEncadernacaoRepository>();
            services.AddScoped<ILivroDigitalRepository, LivroDigitalRepository>();

            services.AddDbContext<BancoContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("BancoContext")));
             services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Livraria",
                    Description = "Documentação da API Livraria",
                    Contact = new OpenApiContact
                    {
                        Name = "Marcos Teixeira",
                        Email = "marcos.mv1415@gmail.com"
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Livraria v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
