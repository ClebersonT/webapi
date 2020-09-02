using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            //comprimir tudo que for application/json
            //antes de mandar pra tela, um html por exemplo tem a habilidade de descompactar isso
            services.AddResponseCompression(options =>{
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json"});
            });
            //adiciona por padrão o cabeçalho de cache na aplicação toda!
            //services.AddResponseCaching();
            services.AddControllers();

            //gerar uma chave simétrica, gera em um formato de bytes
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters{
                    //irá validar se tem uma key
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
             
            //informara a aplicação que eu tenho um DataContext
            // e posso informar o banco que eu vou utilizar => postgres, mysqlServer etc

            //usando em memoria
            //services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));
            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("connectionStrings")));

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
            
            //adicionado após a configuração do token
            //que user é
            app.UseAuthentication();

            //o que o user irá poder fazer na aplicação
            app.UseAuthorization();
            //url, como o cliente irá acessar
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
