namespace DiplomaProject.Application.Interfaces;

public interface IDbInitializer
{
    Task SeedAsync();
}