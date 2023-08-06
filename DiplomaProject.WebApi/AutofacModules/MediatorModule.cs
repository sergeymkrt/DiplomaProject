using DiplomaProject.Application.UseCases.Authentication.Commands;

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

        builder.RegisterAssemblyTypes(typeof(RegisterUserCommand).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<>));
        
        builder.RegisterAssemblyTypes(typeof(RegisterUserCommand.RegisterUserCommandHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<>));
        
        builder.RegisterAssemblyTypes(typeof(LoginUserCommand).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<>));
        
        builder.RegisterAssemblyTypes(typeof(LoginUserCommand.LoginUserCommandHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<>));
        //
        // builder.RegisterAssemblyTypes(typeof(SimpleASomeFieldUpdatedDomainEventHandler).GetTypeInfo().Assembly)
        //     .AsClosedTypesOf(typeof(INotificationHandler<>));

        builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));
    }
}