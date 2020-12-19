using api_sis_evenda.estoque.Models;
using api_sis_evenda.vendas.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using api_sis_evenda.Vendas.Services;
using api_sis_evenda.Estoque.Infrastructure.Data.Repository;
using api_sis_evenda.Estoque.Repository;
using Microsoft.EntityFrameworkCore;
using api_sis_evenda.Estoque.Services;
using api_sis_evenda.Vendas.Infrastructure.Data.Repository;
using api_sis_evenda.Vendas.Services.api_sis_evenda.estoque.Services;

namespace api_sis_estoque
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

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSingleton<IMessagePublisher, MessagePublisher>();

            //  Estoque

            services.AddDbContext<Context>(options => options.UseInMemoryDatabase(databaseName: Configuration.GetValue<string>("Estoque:DataBase:Name")));
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoService, ProdutoService>();

            

            //  Estoque Consumer
            services.AddHostedService<EstoqueProdutoVendidoConsumerHostedService>();


            //  Vendas

            services.AddDbContext<ContextVendas>(options => options.UseInMemoryDatabase(databaseName: Configuration.GetValue<string>("Vendas:DataBase:Name")));
            services.AddScoped<IProdutoVendasRepository, ProdutoVendasRepository>();
            services.AddScoped<IProdutoVendasService, ProdutoVendasService>();

            //Vendas Consumer
            services.AddHostedService<VendasProdutoCriadoConsumerHostedService>();
            services.AddHostedService<VendasProdutoAtualizadoConsumerHostedService>();          
            

            //Swagger

            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "api_sis_evenda", Version = "v1" });
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = System.IO.Path.Combine(System.AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api_sis_evenda v1"));
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
