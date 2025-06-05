import axios from "axios";

export const AppConfigClient = axios.create({
  baseURL: "https://localhost:4444/api",
  timeout: 10000,
  headers: {
    "Content-Type": "application/json",
  },
});
