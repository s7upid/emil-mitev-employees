FROM node:18-alpine as builder

WORKDIR /app

ARG REACT_APP_API_BASE_URL

ENV REACT_APP_API_BASE_URL=$REACT_APP_API_BASE_URL

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build

FROM nginx:stable-alpine

COPY --from=builder /app/build /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
