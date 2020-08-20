using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using webapi.Data;

namespace webapi
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
            services.AddControllers();
            //informara a aplicação que eu tenho um DataContext
            // e posso informar o banco que eu vou utilizar => postgres, mysqlServer etc

            //usando em memoria
            //services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionString")));

            /*tornar esse DataContext disponivel aos nossos controllers
            injeção de dependencia
            O AddScoped irá garantir que irei ter somente 1 DataContext por requisição
            cada DataContext cria uma conexão com o banco, nesse caso como terei somente 1, eu nunca terei 2 conexões abertas
            facilitando assim abertura e fechamento de conexão*/
            services.AddScoped<DataContext, DataContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //"variáveis de ambiente"
            //usar bastante em desenvolvimento
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //força a API a responder ao https
            //todo dominio hospedado no azure, por padrão ja tem https
            app.UseHttpsRedirection();
            //roteamento
            app.UseRouting();
            
            app.UseAuthorization();
            //url, como o cliente irá acessar
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
