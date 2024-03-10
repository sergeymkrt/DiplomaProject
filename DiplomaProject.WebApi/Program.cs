using DiplomaProject.Infrastructure.Persistence.Extensions;
using DiplomaProject.WebApi.Extensions;

// ========================================= Add services to the container. ============================================ //

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddWebAPIServices()
    .ConfigureExternalServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

// ========================================= Configure the HTTP request pipeline ============================================ //
var app = builder.Build();

await app.UseDatabaseMigration();
await app.UseInitialDataSeeding();

app.UseGlobalExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
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

app.MapGet("/", context =>
{
    context.Response.Redirect("./swagger", permanent: false);
    return Task.CompletedTask;
});

await app.RunAsync();

