# Используем официальный образ Node.js
FROM node:alpine AS builder

# Устанавливаем рабочую директорию внутри контейнера
WORKDIR /app

# Копируем package.json и package-lock.json для установки зависимостей
COPY package*.json .

# Устанавливаем зависимости
RUN npm install

# Копируем все файлы проекта в контейнер
COPY . .

# Собираем проект для продакшена
RUN npm run build

# Устанавливаем Nginx для обслуживания статических файлов
FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=builder /app/build /usr/share/nginx/html

# Копируем конфигурацию Nginx
COPY ./nginx.conf /etc/nginx/conf.d/default.conf

# Указываем, что контейнер будет слушать на порту 80
EXPOSE 80

# Команда для запуска Nginx
ENTRYPOINT ["nginx", "-g", "daemon off;"]
