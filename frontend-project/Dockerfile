# étape de build
FROM node:18.12.1-alpine as build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install -g npm@8.19.2
COPY . .
RUN npm run build

# étape de production
FROM nginx:stable-alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
COPY ./nginx/default.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]