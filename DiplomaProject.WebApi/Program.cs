using DiplomaProject.Application.Extensions;
using DiplomaProject.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

// ========================================= Add services to the container. ============================================ //

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddMapster();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<WritableDbContext, AppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(AppContext).GetTypeInfo().Assembly.GetName().Name);
        }));

builder.Services.AddIdentity<User, Role>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<AppContext>()
    .AddDefaultTokenProviders();

builder.Services.AddDbContext<AppQueryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(AppQueryContext).GetTypeInfo().Assembly.GetName().Name);
        }));


builder.Host.ConfigureContainer<ContainerBuilder>(b => b
    .RegisterModule(new ApplicationModule())
    .RegisterModule(new MediatorModule()));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ========================================= Configure the HTTP request pipeline ============================================ //
var app = builder.Build();

// await SeedInitialDataAsync(app);

app.UseGlobalExceptionHandler();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


static async Task SeedInitialDataAsync(WebApplication webApp)
{
    var scopeFactory = webApp.Services.GetRequiredService<IServiceScopeFactory>();
    using var scope = scopeFactory.CreateScope();
    var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();

    await dbInitializer.SeedAsync();
}