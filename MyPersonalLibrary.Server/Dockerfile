# Dentro MyPersonalLibrary.Server/Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia solo il file .csproj e ripristina (punto 1)
COPY MyPersonalLibrary.Server.csproj .
RUN dotnet restore

# Copia il resto dei file e pubblica (punto 2)
COPY . .
RUN dotnet publish -c Release -o /app/publish

# =========================================================================

# Usa l'immagine ufficiale ASP.NET Runtime come base finale (più piccola e sicura)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Copia l'output pubblicato dalla fase di build
COPY --from=build /app/publish .

# Variabile d'ambiente: rende l'applicazione accessibile esternamente
# $PORT viene iniettata automaticamente da Render
ENV ASPNETCORE_URLS=http://+:$PORT

# Comando di avvio
ENTRYPOINT ["dotnet", "MyPersonalLibrary.Server.dll"]