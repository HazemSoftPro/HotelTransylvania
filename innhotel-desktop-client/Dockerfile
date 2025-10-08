# syntax=docker/dockerfile:1
FROM node:20-alpine AS base

WORKDIR /app

COPY package.json pnpm-lock.yaml .npmrc ./
RUN corepack enable && pnpm install --frozen-lockfile

COPY . .

EXPOSE 5173

ENV HOST=0.0.0.0
CMD ["pnpm", "run", "dev:react", "--", "--host", "0.0.0.0", "--port", "5173"]
