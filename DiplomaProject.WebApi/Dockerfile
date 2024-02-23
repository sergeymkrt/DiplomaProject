FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DiplomaProject.WebApi/DiplomaProject.WebApi.csproj", "DiplomaProject.WebApi/"]
COPY ["DiplomaProject.Application/DiplomaProject.Application.csproj", "DiplomaProject.Application/"]
COPY ["DiplomaProject.Domain/DiplomaProject.Domain.csproj", "DiplomaProject.Domain/"]
COPY ["DiplomaProject.Infrastructure.Shared/DiplomaProject.Infrastructure.Shared.csproj", "DiplomaProject.Infrastructure.Shared/"]
COPY ["DiplomaProject.Infrastructure.Persistence/DiplomaProject.Infrastructure.Persistence.csproj", "DiplomaProject.Infrastructure.Persistence/"]
COPY ["DiplomaProject.Packages/DiplomaProject.Packages.csproj", "DiplomaProject.Packages/"]
RUN dotnet restore "DiplomaProject.WebApi/DiplomaProject.WebApi.csproj"
COPY . .
WORKDIR "/src/DiplomaProject.WebApi"
RUN dotnet build "DiplomaProject.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DiplomaProject.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DiplomaProject.WebApi.dll"]
