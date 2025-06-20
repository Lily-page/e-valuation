import axios, { type AxiosInstance, type AxiosRequestConfig } from 'axios'

// API Configuration
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:12001/api'

// Create axios instance
const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Request interceptor to add token
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('anonymousToken')
    if (token) {
      config.headers['X-Anonymous-Token'] = token
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Response interceptor to handle token expiration
apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      // Token expired or invalid, try to refresh or get new token
      const tokenService = await import('./tokenService')
      await tokenService.default.refreshOrGenerateToken()

      // Retry the original request
      const originalRequest = error.config
      if (!originalRequest._retry) {
        originalRequest._retry = true
        const newToken = localStorage.getItem('anonymousToken')
        if (newToken) {
          originalRequest.headers['X-Anonymous-Token'] = newToken
          return apiClient(originalRequest)
        }
      }
    }
    return Promise.reject(error)
  }
)

export default apiClient
