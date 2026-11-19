# Деплой Warehouse на сервер

## 1. Собрать образ (на машине разработчика)

Из папки `Warehouse.API/Warehouse.API` (рядом должны лежать оба репозитория):

```powershell
docker compose build
docker save warehouse:latest | gzip > deploy/warehouse-image.tar.gz   # в Git Bash
```

## 2. Скопировать на сервер

В одну папку на сервере, например `/opt/warehouse`:

- `deploy/warehouse-image.tar.gz`
- `deploy/docker-compose.yml`
- `deploy/deploy.sh`
- `.env`: создать из `.env.example` и задать свой `JwtConfiguration__SecretKey` (`openssl rand -hex 64`)

```bash
scp deploy/warehouse-image.tar.gz deploy/docker-compose.yml deploy/deploy.sh .env user@server:/opt/warehouse/
```

## 3. Запустить на сервере

```bash
cd /opt/warehouse
chmod +x deploy.sh
docker ps -a          # уточнить имя старого контейнера (обычно warehouse_app)
./deploy.sh warehouse_app
```

Скрипт выполняет шаги:

1. Загружает новый образ.
2. Останавливает старый контейнер и копирует из него `/app/app.db` (с `-wal`/`-shm`) в `backups/<дата>/` и в `data/`.
3. Переименовывает старый контейнер в `warehouse_app_old`, не удаляя его.
4. Запускает новую версию. При старте она сама применяет миграции к базе (добавляет таблицы `ProcessedFiles` и `ShortUrls`, данные не трогает).

## Откат

```bash
docker compose down
docker rename warehouse_app_old warehouse_app && docker start warehouse_app
```

Старая база не меняется: она остаётся внутри старого контейнера и в `backups/`.

## После деплоя

- Все пользователи должны войти заново, потому что сменился JWT-секрет.
- Бэкап — это просто копия папки `data/` (лучше при остановленном контейнере).
- Регистрация новых пользователей теперь доступна только администратору.
