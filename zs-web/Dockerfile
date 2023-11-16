# Stage 1: Build Angular application
FROM node:18 AS builder

ARG ENVIRONMENT=production
ARG NPM_REGISTRY=https://registry.npmjs.org/

WORKDIR /app

# Copy package.json and yarn.lock
COPY package.json yarn.lock ./

# Set the registry for Yarn
RUN yarn config set registry $NPM_REGISTRY

# Install dependencies using Yarn
RUN yarn install

# Copy the rest of the application code
COPY . .

# Build the Angular application
RUN yarn build --configuration=$ENVIRONMENT

# Stage 2: Create the production image
FROM nginx:alpine

# Copy config file to NGINX directory
COPY ./nginx/nginx.conf /etc/nginx/conf.d/default.conf

# Copy the static files from the builder stage to the NGINX public directory
COPY --from=builder /app/dist /usr/share/nginx/html

# Expose port 80 (default NGINX port)
EXPOSE 80

# Start NGINX server
CMD ["nginx", "-g", "daemon off;"]
