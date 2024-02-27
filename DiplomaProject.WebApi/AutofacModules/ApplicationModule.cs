using DiplomaProject.Domain.Services.Encryption;
using DiplomaProject.Infrastructure.Shared.Encryption;

namespace DiplomaProject.WebApi.AutofacModules;

public class ApplicationModule : Autofac.Module
{
    public ApplicationModule()
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        #region Services
        builder.RegisterType<CurrentUser>()
            .As<ICurrentUser>()
            .InstancePerLifetimeScope();

        builder.RegisterType<AuthenticationService>()
            .As<IAuthenticationService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<EncryptionService>()
            .As<IEncryptionService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<AzureStorageManagementService>()
            .As<IFileManagementService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<DbInitializer>()
            .As<IDbInitializer>()
            .InstancePerLifetimeScope();
        #endregion
    }
}