# Деплой HappyAddress

## Что нужно на хостинге

- Docker Web Service.
- MySQL база данных.
- Переменные окружения для подключения к БД и SMTP.

## Переменные окружения

```text
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
ConnectionStrings__DefaultConnection=server=HOST;port=3306;database=DB_NAME;user=DB_USER;password=DB_PASSWORD;
EmailSettings__SmtpHost=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__SmtpUser=your-email@gmail.com
EmailSettings__SmtpPassword=your-app-password
EmailSettings__FromEmail=your-email@gmail.com
```

Если используешь Gmail, нужен именно app password, а не обычный пароль от аккаунта.

## Быстрый вариант через Render

1. Залить проект в GitHub.
2. В Render создать MySQL или подключить внешнюю MySQL базу.
3. Создать Web Service из GitHub-репозитория.
4. Environment выбрать Docker.
5. Root Directory оставить пустым, если репозиторий открывается из корня проекта.
6. Добавить переменные окружения из списка выше.
7. Запустить деплой.

При первом запуске приложение само применит Entity Framework migrations и создаст таблицы.

## Локальная проверка перед деплоем

```powershell
dotnet publish .\HappyAddress\HappyAddress.csproj -c Release -o .\publish\HappyAddress
```

Если команда завершилась без ошибок, проект готов к публикации.
