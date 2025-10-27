import { isDevEnv } from "./isDevEnv";

export function getApiBaseUrl() {
  return import.meta.env.VITE_API_BASE_URL || (isDevEnv() ? "http://localhost:57679/api" : "");
}
