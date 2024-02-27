using Autofac;
using DiplomaProject.Domain.Services.DomainServices.Files;
using DiplomaProject.Domain.Services.DomainServices.Keys;

namespace DiplomaProject.Domain.AggregatesModel;

public class DomainServiceModule : Autofac.Module
{
    public DomainServiceModule()
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        #region Services
        builder.RegisterType<FileDomainService>()
            .As<IFileDomainService>()
            .InstancePerLifetimeScope();

        builder.RegisterType<KeyDomainService>()
            .As<IKeyDomainService>()
            .InstancePerLifetimeScope();
        #endregion
    }
}