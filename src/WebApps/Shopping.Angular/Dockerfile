# ---------- BUILD ----------
FROM node:20-alpine AS build

WORKDIR /app

# Copia SOMENTE o package.json do Angular
COPY src/WebApps/Shopping.Angular/package*.json ./
RUN npm install

# Copia o código do Angular
COPY src/WebApps/Shopping.Angular ./

RUN npm run build -- --configuration production

# ---------- RUNTIME ----------
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

RUN rm -rf ./*
COPY --from=build /app/dist/shopping-angular ./

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
