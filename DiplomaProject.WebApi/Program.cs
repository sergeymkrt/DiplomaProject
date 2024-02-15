using DiplomaProject.Application;
using DiplomaProject.Infrastructure.Persistence.Extensions;
using DiplomaProject.WebApi.Extensions;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

// ========================================= Add services to the container. ============================================ //

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddWebAPIServices();

var mediatorConfig = MediatRConfigurationBuilder
    .Create(typeof(BaseCommand<>).Assembly)
    .WithAllOpenGenericHandlerTypesRegistered()
    .Build();

builder.Host.ConfigureContainer<ContainerBuilder>(b => b
    .RegisterMediatR(mediatorConfig)
    .RegisterModule(new ApplicationModule())
    .RegisterModule(new MediatorModule()));

// ========================================= Configure the HTTP request pipeline ============================================ //
var app = builder.Build();

await app.UseDatabaseMigration();
await app.UseInitialDataSeeding();

app.UseGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors();
}
else
{
    app.UseCors("Production");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();

