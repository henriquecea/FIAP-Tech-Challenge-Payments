# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todos os projetos
COPY FCG_Payments.WebAPI ./FCG_Payments.WebAPI
COPY FCG_Payments.Application ./FCG_Payments.Application
COPY FCG_Payments.Domain ./FCG_Payments.Domain
COPY FCG_Payments.Infrastructure ./FCG_Payments.Infrastructure

# Restaurar e publicar a WebAPI
WORKDIR /src/FCG_Payments.WebAPI
RUN dotnet restore ./FCG_Payments.WebAPI.csproj
RUN dotnet publish ./FCG_Payments.WebAPI.csproj -c Release -o /app

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "FCG_Payments.WebAPI.dll"]
