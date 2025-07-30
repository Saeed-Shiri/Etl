FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["src/Domain/ArpaSync.Domain/ArpaSync.Domain.csproj", "src/Domain/ArpaSync.Domain/"]
COPY ["src/Application/ArpaSync.Application/ArpaSync.Application.csproj", "src/Application/ArpaSync.Application/"]
COPY ["src/Infrastructure/ArpaSync.Infrastructure/ArpaSync.Infrastructure.csproj", "src/Infrastructure/ArpaSync.Infrastructure/"]
COPY ["src/WebApi/ArpaSync.WebApi/ArpaSync.WebApi.csproj", "src/WebApi/ArpaSync.WebApi/"]

# Restore dependencies
RUN dotnet restore "src/WebApi/ArpaSync.WebApi/ArpaSync.WebApi.csproj"

# Copy source code
COPY . .

# Build application
WORKDIR "/src/src/WebApi/ArpaSync.WebApi"
RUN dotnet build "ArpaSync.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ArpaSync.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Create logs directory
RUN mkdir -p /app/logs

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArpaSync.WebApi.dll"]