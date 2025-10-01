FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SistemaBancario.Api/SistemaBancario.Api.csproj", "SistemaBancario.Api/"]
COPY ["SistemaBancario.Aplicacao/SistemaBancario.Aplicacao.csproj", "SistemaBancario.Aplicacao/"]
COPY ["SistemaBancario.Infraestrutura/SistemaBancario.Infraestrutura.csproj", "SistemaBancario.Infraestrutura/"]
COPY ["SistemaBancario.Dominio/SistemaBancario.Dominio.csproj", "SistemaBancario.Dominio/"]
RUN dotnet restore "SistemaBancario.Api/SistemaBancario.Api.csproj"
COPY . .
WORKDIR /src/SistemaBancario.Api
RUN dotnet publish "SistemaBancario.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SistemaBancario.Api.dll"]
