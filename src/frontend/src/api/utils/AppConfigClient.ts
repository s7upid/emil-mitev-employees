import axios from "axios";
const baseURL = process.env.REACT_APP_API_BASE_URL;

export const AppConfigClient = axios.create({
  baseURL,
  timeout: 10000,
  headers: {
    "Content-Type": "application/json",
  },
});
