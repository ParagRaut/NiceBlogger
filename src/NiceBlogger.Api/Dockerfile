#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/NiceBlogger.Api/NiceBlogger.Api.csproj", "src/NiceBlogger.Api/"]
COPY ["src/NiceBlogger.UseCases/NiceBlogger.UseCases.csproj", "src/NiceBlogger.UseCases/"]
COPY ["src/NiceBlogger.Infrastructure/NiceBlogger.Infrastructure.csproj", "src/NiceBlogger.Infrastructure/"]
RUN dotnet restore "./src/NiceBlogger.Api/NiceBlogger.Api.csproj"
COPY . .
WORKDIR "/src/src/NiceBlogger.Api"
RUN dotnet build "./NiceBlogger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NiceBlogger.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NiceBlogger.Api.dll"]