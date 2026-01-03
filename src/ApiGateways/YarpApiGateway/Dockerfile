# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o csproj
COPY ["src/ApiGateways/YarpApiGateway/YarpApiGateway.csproj", "src/ApiGateways/YarpApiGateway/"]

# Restore
RUN dotnet restore "src/ApiGateways/YarpApiGateway/YarpApiGateway.csproj"

# Copia tudo
COPY . .

# Build
WORKDIR "/src/src/ApiGateways/YarpApiGateway"
RUN dotnet build "YarpApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "YarpApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YarpApiGateway.dll"]