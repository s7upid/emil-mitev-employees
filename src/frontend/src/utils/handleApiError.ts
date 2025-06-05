export const handleApiError = (error: any): string => {
  if (error.code === "ECONNABORTED" || !error.response) {
    return "Cannot connect to the server. Please check your internet connection.";
  }

  const status = error.response?.status;
  if (status === 404) return "No employee data found.";
  if (status === 500) return "Server error, please try again later.";

  return "An unexpected error occurred.";
};
