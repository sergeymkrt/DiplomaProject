using Autofac;
using DiplomaProject.Domain.AggregatesModel.FileAggregate;
using DiplomaProject.Domain.AggregatesModel.Keys;
using DiplomaProject.Infrastructure.Persistence.Repositories;

namespace DiplomaProject.Infrastructure.Persistence;

public class RepositoryModule : Autofac.Module
{
    public RepositoryModule()
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        #region Services
        builder.RegisterType<FileRepository>()
            .As<IFileRepository>()
            .InstancePerLifetimeScope();

        builder.RegisterType<KeyRepository>()
            .As<IKeyRepository>()
            .InstancePerLifetimeScope();
        #endregion
    }
}