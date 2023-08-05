namespace DiplomaProject.WebApi.AutofacModules;

public class MediatorModule : Autofac.Module
{
    public MediatorModule()
    {
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        // builder.RegisterAssemblyTypes(typeof(CreateSimpleACommand).GetTypeInfo().Assembly)
        //     .AsClosedTypesOf(typeof(IRequestHandler<,>));
        //
        // builder.RegisterAssemblyTypes(typeof(CreateSimpleACommand.CreateSimpleACommandHandler).GetTypeInfo().Assembly)
        //     .AsClosedTypesOf(typeof(IRequestHandler<>));
        //
        // builder.RegisterAssemblyTypes(typeof(SimpleASomeFieldUpdatedDomainEventHandler).GetTypeInfo().Assembly)
        //     .AsClosedTypesOf(typeof(INotificationHandler<>));

        // builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
    }
}