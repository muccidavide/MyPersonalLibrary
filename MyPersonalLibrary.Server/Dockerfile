# Vedere https://aka.ms/customizecontainer per informazioni su come personalizzare il contenitore di debug e su come Visual Studio usa questo Dockerfile per compilare le immagini per un debug più rapido.

# esecuzione da Visual Studio in modalità rapida
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# compilazione
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MyPersonalLibrary.Server/MyPersonalLibrary.Server.csproj", "MyPersonalLibrary.Server/"]
RUN dotnet restore "./MyPersonalLibrary.Server/MyPersonalLibrary.Server.csproj"
COPY . .
WORKDIR "/src/MyPersonalLibrary.Server"
RUN dotnet build "./MyPersonalLibrary.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build /p:BuildClient=false 

# publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyPersonalLibrary.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false /p:BuildClient=false 

# per esecuzione
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyPersonalLibrary.Server.dll"]
