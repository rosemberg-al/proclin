using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Repository.Interfaces;
using Clinicas.Infrastructure.Repository;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Application.Services;
using Clinicas.Api;

[assembly: OwinStartup(typeof(Clinica.Api.Startup))]
namespace Clinica.Api
{
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        public SimpleInjector.Container Container = null;

        public void Configuration(IAppBuilder app)
        {
            this.Container = new SimpleInjector.Container();
            InitializeContainer(Container);
            Container.Verify();

            app.Use(async (context, next) =>
            {
                using (Container.BeginExecutionContextScope())
                {
                    await next();
                }
            });

            HttpConfiguration config = new HttpConfiguration();
            config.DependencyResolver = new SimpleInjector.Integration.WebApi.SimpleInjectorWebApiDependencyResolver(this.Container);

            ConfigureOAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        private void ConfigureOAuth(IAppBuilder app)
        {

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new AuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private static void InitializeContainer(Container container)
        {
            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
            container.Register<ClinicasContext>(Lifestyle.Scoped);
            container.Register(typeof(IUnitOfWork<>), typeof(UnitOfWork<>), Lifestyle.Scoped);

            container.Register<IPacienteRepository, PacienteRepository>();
            container.Register<IPacienteService, PacienteService>();
            container.Register<IFuncionarioRepository, FuncionarioRepository>();
            container.Register<IFuncionarioService, FuncionarioService>();
            container.Register<ICadastroRepository, CadastroRepository>();
            container.Register<ICadastroService, CadastroService>();
            container.Register<IProntuarioRepository, ProntuarioRepository>();
            container.Register<IProntuarioService, ProntuarioService>();
            container.Register<IAgendaRepository, AgendaRepository>();
            container.Register<IAgendaService, AgendaService>();
            container.Register<IGuiaRepository,GuiaRepository>();
            container.Register<IGuiaService, GuiaService>();
            container.Register<IRelatorioRepository, RelatorioRepository>();
            container.Register<IRelatorioService, RelatorioService>();
            container.Register<IDashboardRepository, DashboardRepository>();
            container.Register<IDashboardService, DashboardService>();
            container.Register<IUsuarioRepository, UsuarioRepository>();
            container.Register<IUsuarioService, UsuarioService>();
            container.Register<ISegurancaRepository, SegurancaRepository>();
            container.Register<ISegurancaService, SegurancaService>();
            container.Register<IFinanceiroRepository, FinanceiroRepository>();
            container.Register<IFinanceiroService, FinanceiroService>();
            container.Register<IMedicamentoRepository, MedicamentoRepository>();
            container.Register<IMedicamentoService, MedicamentoService>();
            container.Register<IBuscaRepository, BuscaRepository>();
            container.Register<IBuscaService, BuscaService>();
            container.Register<IEstoqueRepository, EstoqueRepository>();
            container.Register<IEstoqueService, EstoqueService>();
        }

    }
}