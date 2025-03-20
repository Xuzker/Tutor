# Используем официальный образ ASP.NET Core для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Используем SDK образ для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем .csproj и восстанавливаем зависимости
COPY ["Tutor.csproj", "./"]
RUN dotnet restore "Tutor.csproj"

# Копируем все файлы и собираем приложение
COPY . .
RUN dotnet build "Tutor.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикуем приложение
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tutor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Финальный образ для запуска
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tutor.dll"]
