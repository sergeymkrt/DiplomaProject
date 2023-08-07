using DiplomaProject.Application.Interfaces;
using DiplomaProject.Domain.Services.External;
using DiplomaProject.Infrastructure.Persistence.InitialDataSeeding;
using DiplomaProject.Infrastructure.Shared.ExternalServices;

namespace DiplomaProject.WebApi.AutofacModules;

public class ApplicationModule: Autofac.Module
{
    public ApplicationModule()
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        #region Services
        builder.RegisterType<IdentityUserService>()
            .As<IIdentityUserService>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<AuthenticationService>()
            .As<IAuthenticationService>()
            .InstancePerLifetimeScope();
        
        builder.RegisterType<DbInitializer>()
            .As<IDbInitializer>()
            .InstancePerLifetimeScope();
        #endregion
    }
}