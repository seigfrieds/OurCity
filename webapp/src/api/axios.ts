import axios from 'axios';

// create an axios instance
const api = axios.create({
  baseURL: 'http://localhost:8000/',
  headers: {
    'Content-Type': 'application/json',
  }
});

// interceptors to unwrap ApiResponse
api.interceptors.response.use(
  (response) => {
    // Check if response matches ApiResponse structure
    if (
      response.data &&
      typeof response.data === 'object' &&
      'data' in response.data &&
      'isSuccess' in response.data &&
      'message' in response.data
    ) {
      // Unwrap the data property
      response.data = response.data.data;
    }
    return response;
  },
  (error) => {
    // Handle errors (optional: you can add custom error handling here)
    return Promise.reject(error);
  }
);

// export instance for use in project
export default api;