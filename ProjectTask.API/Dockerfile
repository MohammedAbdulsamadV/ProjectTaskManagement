FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ProjectTask.API/ProjectTask.API.csproj", "ProjectTask.API/"]
RUN dotnet restore "ProjectTask.API/ProjectTask.API.csproj"

COPY . .
WORKDIR "/src/ProjectTask.API"
RUN dotnet build "ProjectTask.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectTask.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectTask.API.dll"]