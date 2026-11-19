# Единый образ: Angular-фронтенд + .NET API.
# Контекст сборки — общая папка, где лежат рядом Warehouse.API и Warehouse.WebUI
# (docker-compose.yml уже задаёт context: ../..).

# ---------- 1. Сборка фронтенда ----------
FROM node:24-alpine AS web
WORKDIR /web
COPY Warehouse.WebUI/package.json Warehouse.WebUI/package-lock.json ./
RUN npm ci --no-audit --no-fund
COPY Warehouse.WebUI/ ./
RUN npx ng build --configuration production

# ---------- 2. Сборка API ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS api
WORKDIR /src
COPY Warehouse.API/Warehouse.API/Warehouse.API.csproj Warehouse.API/
RUN dotnet restore Warehouse.API/Warehouse.API.csproj
COPY Warehouse.API/Warehouse.API/ Warehouse.API/
RUN dotnet publish Warehouse.API/Warehouse.API.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ---------- 3. Финальный образ ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:7485 \
    ASPNETCORE_ENVIRONMENT=Production \
    ConnectionStrings__SqliteConnection="Data Source=/app/data/app.db"

COPY --from=api /app/publish .
COPY --from=web /web/dist/browser ./wwwroot

# База живёт в /app/data (монтируется снаружи), миграции применяются при старте
RUN mkdir -p /app/data && chown -R app:app /app/data
USER app

EXPOSE 7485
VOLUME ["/app/data"]
ENTRYPOINT ["dotnet", "Warehouse.API.dll"]
