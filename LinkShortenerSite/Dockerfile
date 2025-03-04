# Используем .NET SDK для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

RUN pwd
RUN ls
# Копируем .csproj файл из папки LinkShortenerSite в контейнер
COPY LinkShortenerSite/LinkShortenerSite.csproj LinkShortenerSite/

# Восстанавливаем зависимости
RUN dotnet restore "LinkShortenerSite/LinkShortenerSite.csproj"

# Копируем все файлы проекта, включая папку Protos
# Обратите внимание, что тут может быть ошибка, если папка LinkShortenerSite не находится в контексте сборки
RUN pwd
RUN ls
COPY LinkShortenerSite/. LinkShortenerSite/

# Переходим в директорию с проектом и строим приложение
WORKDIR "/src/LinkShortenerSite"
RUN dotnet build "LinkShortenerSite.csproj" -c Release -o /app/build
RUN pwd
RUN ls

# Используем базовый образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "LinkShortenerSite.csproj" -c Release -o /app/publish

# Финальный образ для запуска
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "LinkShortenerSite.dll"]
