# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
# HTTP port
# HTTPS port (genellikle debug veya yerel kullanım için)

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebApplication1.csproj", "."]
RUN dotnet restore "./WebApplication1.csproj"

COPY . .
WORKDIR "/src/."
RUN dotnet build "./WebApplication1.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApplication1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install sqlite3
# APT komutları root yetkisi gerektirir, bu yüzden USER app'den önce olmalı
RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

# Create the SQLite database (and optionally a table)
RUN sqlite3 /app/mydatabase.db "CREATE TABLE IF NOT EXISTS test (id INTEGER PRIMARY KEY, name TEXT);"

# Şimdi kullanıcıyı app olarak ayarlayabiliriz.
USER app
ENTRYPOINT ["dotnet", "WebApplication1.dll"]

# ... (Python aşaması aynı kalacak) ...
FROM python:3.10-buster AS python_base
WORKDIR /app/python_app
COPY python_app/requirements.txt .
RUN pip install --no-cache-dir --verbose -r requirements.txt
COPY python_app/ .
EXPOSE 5000
# CMD ["python", "app.py"]