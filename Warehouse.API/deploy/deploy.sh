#!/usr/bin/env bash
# Обновление Warehouse на сервере с переносом данных из работающего контейнера.
#
# Что лежит рядом со скриптом:
#   warehouse-image.tar.gz   — собранный образ (docker save)
#   docker-compose.yml       — запуск нового контейнера
#   .env                     — секреты (JwtConfiguration__SecretKey и т.д.)
#
# Запуск:  ./deploy.sh [имя_старого_контейнера]   (по умолчанию warehouse_app)
set -euo pipefail

OLD_CONTAINER="${1:-warehouse_app}"
HERE="$(cd "$(dirname "$0")" && pwd)"
cd "$HERE"
STAMP="$(date +%Y%m%d-%H%M%S)"
BACKUP_DIR="backups/$STAMP"   # относительно $HERE (выше уже сделан cd)
APP_UID=1654   # пользователь app в образах mcr.microsoft.com/dotnet

log() { printf '\n\033[1;34m==> %s\033[0m\n' "$*"; }

[ -f .env ] || { echo "Нет файла .env (скопируйте .env.example и задайте секрет)"; exit 1; }
[ -f warehouse-image.tar.gz ] || { echo "Нет warehouse-image.tar.gz"; exit 1; }

log "1. Загружаю новый образ"
docker load -i warehouse-image.tar.gz

log "1a. Проверяю, что новая версия запускается на этом сервере (старая продолжает работать)"
# Пробный запуск без базы и секретов: приложение должно дойти до проверки настроек и выйти.
# Если .NET не поддерживает процессор, он падает раньше с "Fatal error ... instruction sets".
PROBE="$(timeout 90 docker run --rm -e 'ConnectionStrings__SqliteConnection=Data Source=:memory:' warehouse:latest 2>&1 || true)"
if echo "$PROBE" | grep -qiE "instruction sets|Fatal error"; then
  echo "$PROBE" | head -5
  echo
  echo "Новая версия НЕ запускается на этом процессоре. Ничего не изменено, старая версия работает."
  exit 1
fi
echo "OK: среда выполнения запускается"

if [ -f data/app.db ]; then
  log "data/app.db уже существует, перенос из старого контейнера пропускаю"
else
  docker inspect "$OLD_CONTAINER" >/dev/null 2>&1 || {
    echo "Контейнер '$OLD_CONTAINER' не найден. Укажите имя: ./deploy.sh <имя> (см. docker ps -a)"; exit 1; }

  log "2. Останавливаю старый контейнер '$OLD_CONTAINER' (чтобы база не менялась во время копирования)"
  docker stop "$OLD_CONTAINER"

  log "3. Забираю базу из старого контейнера"
  mkdir -p "$BACKUP_DIR" data
  docker cp "$OLD_CONTAINER:/app/app.db" "$BACKUP_DIR/app.db"
  # SQLite в режиме WAL держит часть данных в -wal/-shm: забираем их, если есть
  docker cp "$OLD_CONTAINER:/app/app.db-wal" "$BACKUP_DIR/app.db-wal" 2>/dev/null || true
  docker cp "$OLD_CONTAINER:/app/app.db-shm" "$BACKUP_DIR/app.db-shm" 2>/dev/null || true
  cp "$BACKUP_DIR"/app.db* data/
  echo "Бэкап: $BACKUP_DIR"
  ls -la "$BACKUP_DIR"

  log "4. Переименовываю старый контейнер в '${OLD_CONTAINER}_old' (для отката)"
  docker rename "$OLD_CONTAINER" "${OLD_CONTAINER}_old"
fi

log "5. Права на папку data для пользователя контейнера (uid $APP_UID)"
chown -R "$APP_UID:$APP_UID" data 2>/dev/null || sudo chown -R "$APP_UID:$APP_UID" data

log "6. Запускаю новую версию"
docker compose up -d

log "7. Жду старта и проверяю логи"
for _ in $(seq 1 30); do
  if docker logs warehouse_app 2>&1 | grep -q "Application started"; then break; fi
  sleep 2
done
docker logs --tail 40 warehouse_app

if docker logs warehouse_app 2>&1 | grep -q "Application started"; then
  log "Готово: приложение запущено на порту 8080"
  echo "Если всё работает, старый контейнер можно удалить: docker rm ${OLD_CONTAINER}_old"
else
  log "Приложение не стартовало. Откат:"
  echo "  docker compose down"
  echo "  docker rename ${OLD_CONTAINER}_old $OLD_CONTAINER && docker start $OLD_CONTAINER"
  exit 1
fi
