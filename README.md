# Склад - API

Серверная часть внутренней системы учёта расходников: **бумага, картриджи, выдача сотрудникам и история**. REST API на ASP.NET Core, база SQLite, авторизация по JWT с ролями. В Docker-образе вместе с API раздаётся и веб-интерфейс.

Интерфейс живёт в отдельном репозитории: **[Warehouse.WebUI](https://github.com/13cyberpunk02/Warehouse.WebUI)**.

![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-9-512BD4)
![SQLite](https://img.shields.io/badge/SQLite-003B57?logo=sqlite&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-green)

---

## Содержание

- [Возможности](#возможности)
- [Как это устроено](#как-это-устроено)
- [Стек](#стек)
- [Структура проекта](#структура-проекта)
- [Быстрый старт для разработки](#быстрый-старт-для-разработки)
- [Конфигурация](#конфигурация)
- [Роли и доступ](#роли-и-доступ)
- [API](#api)
- [База данных и миграции](#база-данных-и-миграции)
- [Docker](#docker)
- [Деплой на сервер](#деплой-на-сервер)
- [Почему .NET 9](#почему-net-9)
- [Учётные записи по умолчанию](#учётные-записи-по-умолчанию)
- [Известные ограничения и планы](#известные-ограничения-и-планы)
- [Лицензия](#лицензия)

---

## Возможности

| Раздел | Что умеет |
|---|---|
| **Бумага** | Справочник форматов и остатков, добавление, изменение, пополнение, удаление |
| **Картриджи** | То же для моделей картриджей |
| **Принтеры** | Справочник принтеров и совместимых картриджей (многие ко многим) - чтобы при выдаче искать картридж по модели принтера |
| **Выдача** | Выдача сотруднику с автоматическим списанием остатка. Выдача, позиция и списание сохраняются одной транзакцией; нельзя выдать больше, чем есть |
| **История** | Журнал выдач по бумаге и картриджам: кто выдал, кому, когда, сколько (новые сверху) |
| **Сотрудники и отделы** | Справочник получателей и подразделений |
| **Пользователи системы** | Вход по логину и паролю (ASP.NET Identity), JWT + refresh-токен с ротацией, роли |
| **PDF-инструменты** | Разделение многостраничного PDF на отдельные страницы с выдачей ZIP-архива (в памяти, без сохранения на диск) |

## Как это устроено

```
 Браузер ──HTTP──▶ ┌──────────────── Docker: warehouse_app ────────────────┐
                   │  ASP.NET Core (Kestrel, порт 7485)                     │
                   │   ├─ /api/*            REST API (JWT, роли)            │
                   │   ├─ /                 веб-интерфейс (Angular, wwwroot)│
                   │   └─ миграции EF Core применяются при старте           │
                   └──────────────────────────┬────────────────────────────┘
                                              ▼
                               SQLite  /app/data/app.db  (том ./data)
```

- Один контейнер: API и собранный фронтенд, отдельный веб-сервер не нужен.
- База - один файл SQLite в смонтированной папке `data/`. Бэкап - копия этой папки.
- Интернет серверу не нужен: всё, включая шрифты и иконки интерфейса, лежит в образе.

## Стек

| Что | Чем |
|---|---|
| Платформа | .NET 9, ASP.NET Core Minimal API |
| Данные | Entity Framework Core 9 + SQLite |
| Пользователи и роли | ASP.NET Core Identity |
| Авторизация | JWT Bearer (HMAC-SHA512) + refresh-токены |
| Валидация | FluentValidation 12 |
| PDF | iText 9 |
| Документация API | OpenAPI + Scalar (только в режиме разработки) |
| Развёртывание | Docker, docker compose |

## Структура проекта

```
Warehouse.API/
├── Warehouse.API.sln
├── README.md
└── Warehouse.API/
    ├── Program.cs                  # точка входа: миграции, middleware, маршруты
    ├── Endpoints/                  # Minimal API: по файлу на раздел
    ├── Services/
    │   ├── Interfaces/
    │   └── Implementations/        # бизнес-логика (бумага, картриджи, выдача, принтеры…)
    ├── Data/
    │   ├── DataContext.cs          # EF Core: таблицы, связи, начальные данные
    │   ├── Entities/               # сущности БД
    │   ├── Models/                 # DTO запросов/ответов, Result, ошибки
    │   ├── Validators/             # FluentValidation
    │   └── Options/                # JwtConfiguration
    ├── Extensions/                 # регистрация сервисов, JWT, CORS, OpenAPI
    ├── Migrations/                 # миграции EF Core
    ├── Dockerfile                  # единый образ: фронтенд + API
    ├── docker-compose.yml          # локальная сборка и запуск
    ├── .env.example                # пример секретов
    └── deploy/                     # всё для установки на сервер
        ├── deploy.sh
        ├── docker-compose.yml
        └── README.md
```

## Быстрый старт для разработки

### Что нужно

- [.NET SDK 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- Для интерфейса - Node.js LTS (22 или 24) и репозиторий [Warehouse.WebUI](https://github.com/13cyberpunk02/Warehouse.WebUI)

Оба репозитория удобно держать рядом - так их ожидает и `Dockerfile`:

```
Warehouse/
├── Warehouse.API/
└── Warehouse.WebUI/
```

### 1. Секреты

В режиме разработки настройки берутся из [user-secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets) - в репозитории секретов нет.

```bash
cd Warehouse.API/Warehouse.API
dotnet user-secrets set "ConnectionStrings:SqliteConnection" "Data Source=app.db"
dotnet user-secrets set "JwtConfiguration:SecretKey" "<случайная строка от 64 символов>"
dotnet user-secrets set "JwtConfiguration:Issuer" "warehouse"
dotnet user-secrets set "JwtConfiguration:Audience" "warehouse"
dotnet user-secrets set "JwtConfiguration:ExpirationInMinutes" "30"
dotnet user-secrets set "JwtConfiguration:RefreshTokenExpirationInDays" "7"
```

Ключ можно сгенерировать так: `openssl rand -hex 64` (или в PowerShell:
`-join ((1..64) | % { '{0:x2}' -f (Get-Random -Max 256) })`).

> Если секрет не задан или короче 64 символов, приложение не запустится и сразу скажет, чего не хватает.

### 2. Запуск

```bash
dotnet run --launch-profile https
```

- API: `https://localhost:6443` (и `http://localhost:6000`)
- Интерактивная документация API (Scalar): `https://localhost:6443/scalar`
- База `app.db` создаётся и обновляется автоматически при старте.

### 3. Интерфейс

```bash
cd ../../Warehouse.WebUI
npm ci
npm start          # http://localhost:4200, обращается к https://localhost:6443/api
```

CORS для `http://localhost:4200` уже разрешён (см. [Конфигурация](#конфигурация)).

## Конфигурация

Настройки читаются стандартно для ASP.NET Core: `appsettings.json` → user-secrets (разработка) → переменные окружения (Docker). В переменных окружения `:` заменяется на `__`.

| Ключ | Переменная окружения | Описание |
|---|---|---|
| `ConnectionStrings:SqliteConnection` | `ConnectionStrings__SqliteConnection` | Строка подключения SQLite. В Docker: `Data Source=/app/data/app.db` (задано в образе) |
| `JwtConfiguration:SecretKey` | `JwtConfiguration__SecretKey` | **Обязательно.** Ключ подписи JWT, минимум 64 символа |
| `JwtConfiguration:Issuer` | `JwtConfiguration__Issuer` | Издатель токена |
| `JwtConfiguration:Audience` | `JwtConfiguration__Audience` | Аудитория токена |
| `JwtConfiguration:ExpirationInMinutes` | `JwtConfiguration__ExpirationInMinutes` | Время жизни access-токена (минуты) |
| `JwtConfiguration:RefreshTokenExpirationInDays` | `JwtConfiguration__RefreshTokenExpirationInDays` | Время жизни refresh-токена (дни) |
| `Cors:Origins` | `Cors__Origins__0`, `Cors__Origins__1`… | Разрешённые источники для CORS. По умолчанию `http://localhost:4200`. В Docker не нужен - интерфейс на том же адресе |
| `ASPNETCORE_URLS` | - | Адрес Kestrel. В образе: `http://+:7485` |

Пример для Docker - в [`.env.example`](Warehouse.API/.env.example).

## Роли и доступ

| Роль | Что доступно |
|---|---|
| `Admin` | Всё, включая регистрацию пользователей, изменение и удаление сотрудников, PDF-инструменты |
| `Paper` | Бумага: остатки, выдача, история |
| `Cartridge` | Картриджи и принтеры: остатки, выдача, история, справочник принтеров |
| любой вошедший | Сотрудники (просмотр и добавление), отделы |

Регистрация новых пользователей системы доступна **только администратору** - роль задаёт он же.

## API

Все ответы - в едином формате:

```json
{
  "value": "...",
  "isFailure": false,
  "error": { "code": "Пусто.", "message": "" }
}
```

При ошибке `isFailure: true`, а в `error.message` - понятное сообщение на русском (ошибки валидации - через перевод строки). Коды HTTP: `400` - неверный запрос/валидация, `401` - нет или истёк токен, `403` - нет прав, `404` - не найдено.

Авторизация - заголовок `Authorization: Bearer <accessToken>`. Полное описание с примерами - в Scalar (`/scalar` в режиме разработки).

<details>
<summary><b>Список эндпоинтов</b></summary>

**Вход** - `api/identity`

| Метод | Путь | Доступ | Описание |
|---|---|---|---|
| POST | `/login` | все | Вход, выдаёт `accessToken` и `refreshToken` |
| POST | `/refresh-token` | все | Обновление пары токенов (старый refresh-токен становится недействительным) |
| POST | `/registration` | Admin | Создание пользователя системы с ролью |

**Бумага** - `api/paper` (роль Paper или Admin)

| Метод | Путь | Описание |
|---|---|---|
| GET | `/get-all-papers` | Все форматы и остатки |
| GET | `/get-paper/{id}` | Один формат |
| POST | `/add-paper` | Добавить формат `{ format, quantity }` |
| PUT | `/update-paper` | Изменить `{ id, format, quantity }` |
| DELETE | `/delete-paper/{id}` | Удалить |

**Картриджи** - `api/cartridge` (вошедший пользователь)

| Метод | Путь | Описание |
|---|---|---|
| GET | `/get-all-cartridges` | Все модели и остатки |
| GET | `/get-cartridge-by-id/{id}` | Одна модель |
| POST | `/add-cartridge` | Добавить `{ model, quantity }` |
| PUT | `/update-cartridge` | Изменить `{ id, model, quantity }` (остаток 0…999) |
| DELETE | `/delete-cartridge/{id}` | Удалить (связи с принтерами удаляются автоматически) |

**Принтеры** - `api/printer` (роль Cartridge или Admin)

| Метод | Путь | Описание |
|---|---|---|
| GET | `/get-all-printers` | Принтеры с подходящими картриджами и их остатками |
| POST | `/create-printer` | `{ model, note, cartridgeIds: [] }` |
| PUT | `/update-printer/{id}` | То же тело |
| DELETE | `/delete-printer/{id}` | Удалить принтер (картриджи остаются) |

**Выдача и история** - `api/delivery` (вошедший пользователь)

| Метод | Путь | Описание |
|---|---|---|
| POST | `/add-paper-delivery` | Выдать бумагу `{ receivedUserId, itemId, itemName, quantity }` |
| POST | `/add-cartridge-delivery` | Выдать картридж (то же тело) |
| GET | `/get-all-paper-deliveries` | История выдачи бумаги |
| GET | `/get-all-cartridge-deliveries` | История выдачи картриджей |

Кто выдал, определяется по токену - передавать это в запросе не нужно.

**Сотрудники** - `api/delivery-user`

| Метод | Путь | Доступ | Описание |
|---|---|---|---|
| GET | `/get-all-delivery-users` | вошедший | Все сотрудники с отделами |
| GET | `/get-delivery-user-by-id/{id}` | вошедший | Один сотрудник |
| GET | `/get-all-delivery-users-by-department-id/{id}` | вошедший | Сотрудники отдела |
| POST | `/create-delivery-user` | вошедший | `{ lastname, firstname, patronymic, departmentId }` |
| PUT | `/update-delivery-user` | Admin | Изменить сотрудника |
| DELETE | `/delete-delivery-user/{id}` | Admin | Удалить сотрудника (история выдач сохраняется) |

**Отделы** - `api/department` (вошедший пользователь)

| Метод | Путь | Описание |
|---|---|---|
| GET | `/get-all-departments` | Отделы с сотрудниками |
| GET | `/get-department-by-id/{id}` | Один отдел |
| GET | `/get-department-by-name/{name}` | Поиск по названию |
| POST | `/add-department?name=…` | Добавить |
| PUT | `/update-department` | Переименовать `{ id, name }` |
| DELETE | `/remove-department?departmentId=…` | Удалить |

**PDF** - `api/pdf-tools` (вошедший пользователь)

| Метод | Путь | Описание |
|---|---|---|
| POST | `/split-pdf` | `multipart/form-data` с полем `file` (до 100 МБ). Ответ - ZIP со страницами |

</details>

## База данных и миграции

- SQLite, один файл `app.db`.
- **Миграции применяются автоматически при старте** - и при разработке, и в Docker. Вручную ничего запускать не нужно.
- При первом запуске создаются: отделы организации, роли `Admin`/`Paper`/`Cartridge` и стандартные пользователи (см. [ниже](#учётные-записи-по-умолчанию)).

Добавить миграцию после изменения модели:

```bash
cd Warehouse.API/Warehouse.API
dotnet tool restore                                   # локальный dotnet-ef 9 (dotnet-tools.json)
dotnet tool run dotnet-ef migrations add ИмяМиграции
```

> Проверьте сгенерированную миграцию: в ней должны быть только ваши изменения. Начальные данные (хеши паролей, `ConcurrencyStamp`) в `DataContext` специально зафиксированы, чтобы не попадать в каждую новую миграцию.

## Docker

Образ собирает **и фронтенд, и API**, поэтому контекст сборки - общая папка с обоими репозиториями (см. [структуру](#что-нужно)).

```bash
cd Warehouse.API/Warehouse.API
cp .env.example .env              # и задайте свой JwtConfiguration__SecretKey
docker compose build
docker compose up -d              # http://localhost:8080
```

- Порт: `8080` снаружи → `7485` в контейнере.
- База: папка `./db-data` рядом с compose-файлом (на сервере - `./data`, см. ниже).
- Контейнер работает от непривилегированного пользователя `app`.

Сборка образа: Node 24 собирает Angular → .NET SDK 9 публикует API → итоговый образ `aspnet:9.0` содержит только опубликованный API и статические файлы интерфейса.

## Деплой на сервер

Всё необходимое - в папке [`deploy/`](Warehouse.API/deploy), подробная инструкция - в [deploy/README.md](Warehouse.API/deploy/README.md). Коротко:

```bash
# на машине разработчика
docker compose build
docker save warehouse:latest | gzip > deploy/warehouse-image.tar.gz
scp deploy/warehouse-image.tar.gz deploy/docker-compose.yml deploy/deploy.sh .env user@server:/opt/warehouse/

# на сервере
cd /opt/warehouse && chmod +x deploy.sh && ./deploy.sh
```

Что делает `deploy.sh`:

1. Загружает образ и **пробно запускает его, не трогая работающую версию**: если новая версия не стартует на этом сервере, скрипт останавливается.
2. При первой установке забирает базу из старого контейнера, сохраняет копию в `backups/<дата>/`.
3. Старый контейнер не удаляется, а переименовывается в `*_old` - для отката.
4. Запускает новую версию; миграции применяются сами.

**Бэкап:** скопируйте папку `/opt/warehouse/data` (лучше при остановленном контейнере: `docker compose stop` → копия → `docker compose start`).

**Откат:**

```bash
docker compose down
docker rename warehouse_app_old warehouse_app && docker start warehouse_app
```

Серверу интернет не нужен: образ переносится архивом, все ресурсы интерфейса внутри.

## Почему .NET 9

Начиная с .NET 10 среда выполнения требует процессор с инструкциями SSE4.2 и POPCNT (x86-64-v2). Рабочий сервер организации на AMD Phenom II их не поддерживает, поэтому проект остаётся на **.NET 9** - последней версии, которая на нём запускается.

При переезде на сервер с процессором примерно с 2011 года и новее проект можно перевести на актуальный .NET: поменять `TargetFramework`, версии пакетов и базовые образы в `Dockerfile`.

## Учётные записи по умолчанию

При создании новой базы заводятся пользователи `admin` (Admin), `paper` (Paper) и `cartridge` (Cartridge) с паролями по умолчанию из `Data/DataContext.cs`.

> ⚠️ Эти пароли известны всем, у кого есть доступ к коду. Смените их перед использованием в рабочей среде.

## Известные ограничения и планы

- Нет экрана управления пользователями системы (создание, роли, сброс и смена пароля) - сейчас пользователей создаёт администратор через `POST /api/identity/registration`.
- Поступления на склад не журналируются отдельно: пополнение меняет остаток, но в истории видны только выдачи.
- Нет серверной пагинации истории - для объёмов небольшой организации это не проблема.
- Планы: управление пользователями, акт выдачи для печати, аналитика расхода по отделам.

## Лицензия

[MIT](LICENSE.txt)
