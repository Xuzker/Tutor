Tutor — Платформа для онлайн-обучения программированию
======================================================

📌 О проекте

Tutor — это современная образовательная веб-платформа, разработанная на ASP.NET Core MVC, 
которая позволяет пользователям записываться на курсы программирования, 
а администраторам и преподавателям — управлять курсами, студентами и учебным процессом.

🚀 Основные возможности

Для студентов:
- Регистрация и личный кабинет
- Просмотр и фильтрация курсов по категории, цене и дате старта
- Запись на курсы
- Система уведомлений (email и внутренняя)
- Просмотр учебных материалов

Для преподавателей:
- Создание и управление курсами
- Просмотр статистики по студентам
- Публикация заданий и материалов

Для администраторов:
- Управление пользователями и ролями
- Модерация контента
- Аналитика посещаемости и активности
- Массовые рассылки уведомлений и новостей

🛠 Технологии

Backend:
- ASP.NET Core 8.0 MVC
- Entity Framework Core
- ASP.NET Core Identity
- Kafka (брокер сообщений)
- MailKit (email-уведомления)

Frontend:
- Razor Pages
- Tailwind CSS
- AJAX
- Chart.js (визуализация статистики)

Базы данных и кэш:
- PostgreSQL

Контейнеризация и развертывание:
- Docker и Docker Compose
- Поддержка развертывания в Azure и IIS

⚙ Установка и запуск

Требования:
- .NET SDK 8.0
- PostgreSQL или SQL Server
- Docker (опционально)

Шаги:

git clone https://github.com/Xuzker/tutor.git
cd tutor

# Создание базы данных
dotnet ef database update

# Запуск локально
dotnet run

🔧 Конфигурация (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=Tutor;Trusted_Connection=True;"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.example.com",
    "SmtpPort": 587,
    "FromEmail": "noreply@tutor.com"
  },
  "Kafka": {
    "BootstrapServers": "localhost:9092"
  },
  "Auth": {
    "Google": {
      "ClientId": "your_client_id",
      "ClientSecret": "your_secret"
    }
  }
}
```
🔐 Аутентификация и авторизация

- Локальная регистрация и вход
- OAuth2 / OpenID Connect (Google, Microsoft, Facebook)
- JWT-токены для доступа к API

🚀 Развертывание

Docker:
docker-compose up -d
