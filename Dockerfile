# 1. Imagen base para la ejecución (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# 2. Imagen SDK para compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG NUGET_GITHUB_TOKEN
ARG GITHUB_ACTOR
WORKDIR /src

# Copiar archivos de proyecto y configuración de NuGet
COPY ["shopniu-identity.csproj", "./"]
COPY ["nuget.config", "./"]

# Autenticarse contra GitHub Packages para poder restaurar el paquete privado
RUN dotnet nuget update source github --username $GITHUB_ACTOR --password $NUGET_GITHUB_TOKEN --store-password-in-clear-text

# Restaurar dependencias
RUN dotnet restore "shopniu-identity.csproj"

# Copiar el resto del código fuente
COPY . .

# Compilar
RUN dotnet build "shopniu-identity.csproj" -c Release -o /app/build

# 3. Publicación
FROM build AS publish
RUN dotnet publish "shopniu-identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "shopniu-identity.dll"]