import apiClient from './api'

export interface TokenResponse {
  token: string
  expiresAt: string
  sessionId: string
}

export interface TokenInfo {
  sessionId: string
  createdAt: string
  expiresAt: string
  requestCount: number
  lastUsedAt?: string
  isExpired: boolean
}

class TokenService {
  private readonly TOKEN_KEY = 'anonymousToken'
  private readonly SESSION_ID_KEY = 'sessionId'
  private readonly TOKEN_EXPIRY_KEY = 'tokenExpiry'

  /**
   * Generate a new anonymous token
   */
  async generateToken(sessionId?: string): Promise<TokenResponse> {
    try {
      const response = await apiClient.post('/token/generate', {
        sessionId: sessionId || this.getSessionId()
      })

      const tokenData: TokenResponse = response.data

      // Store token and related data
      this.storeToken(tokenData.token, tokenData.expiresAt, tokenData.sessionId)

      return tokenData
    } catch (error) {
      console.error('Error generating token:', error)
      throw new Error('Failed to generate anonymous token')
    }
  }

  /**
   * Validate current token
   */
  async validateToken(token?: string): Promise<boolean> {
    try {
      const tokenToValidate = token || this.getToken()
      if (!tokenToValidate) {
        return false
      }

      const response = await apiClient.post('/token/validate', tokenToValidate)
      return response.data.isValid
    } catch (error) {
      console.error('Error validating token:', error)
      return false
    }
  }

  /**
   * Refresh current token
   */
  async refreshToken(token?: string): Promise<boolean> {
    try {
      const tokenToRefresh = token || this.getToken()
      if (!tokenToRefresh) {
        return false
      }

      const response = await apiClient.post('/token/refresh', tokenToRefresh)
      return response.status === 200
    } catch (error) {
      console.error('Error refreshing token:', error)
      return false
    }
  }

  /**
   * Get token information
   */
  async getTokenInfo(token?: string): Promise<TokenInfo | null> {
    try {
      const tokenToCheck = token || this.getToken()
      if (!tokenToCheck) {
        return null
      }

      const response = await apiClient.get(`/token/info/${tokenToCheck}`)
      return response.data
    } catch (error) {
      console.error('Error getting token info:', error)
      return null
    }
  }

  /**
   * Get current token from storage
   */
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY)
  }

  /**
   * Get current session ID
   */
  getSessionId(): string {
    let sessionId = localStorage.getItem(this.SESSION_ID_KEY)
    if (!sessionId) {
      sessionId = this.generateSessionId()
      localStorage.setItem(this.SESSION_ID_KEY, sessionId)
    }
    return sessionId
  }

  /**
   * Check if current token is expired
   */
  isTokenExpired(): boolean {
    const expiry = localStorage.getItem(this.TOKEN_EXPIRY_KEY)
    if (!expiry) {
      return true
    }

    return new Date() > new Date(expiry)
  }

  /**
   * Check if we have a valid token
   */
  hasValidToken(): boolean {
    const token = this.getToken()
    return !!(token && !this.isTokenExpired())
  }

  /**
   * Ensure we have a valid token (generate if needed)
   */
  async ensureValidToken(): Promise<string> {
    if (this.hasValidToken()) {
      const token = this.getToken()!
      const isValid = await this.validateToken(token)
      if (isValid) {
        return token
      }
    }

    // Generate new token
    const tokenData = await this.generateToken()
    return tokenData.token
  }

  /**
   * Refresh token or generate new one if refresh fails
   */
  async refreshOrGenerateToken(): Promise<string> {
    const currentToken = this.getToken()

    if (currentToken) {
      const refreshed = await this.refreshToken(currentToken)
      if (refreshed) {
        return currentToken
      }
    }

    // Refresh failed or no token, generate new one
    const tokenData = await this.generateToken()
    return tokenData.token
  }

  /**
   * Clear all token data
   */
  clearToken(): void {
    localStorage.removeItem(this.TOKEN_KEY)
    localStorage.removeItem(this.TOKEN_EXPIRY_KEY)
    // Keep session ID for potential reuse
  }

  /**
   * Store token data in localStorage
   */
  private storeToken(token: string, expiresAt: string, sessionId: string): void {
    localStorage.setItem(this.TOKEN_KEY, token)
    localStorage.setItem(this.TOKEN_EXPIRY_KEY, expiresAt)
    localStorage.setItem(this.SESSION_ID_KEY, sessionId)
  }

  /**
   * Generate a unique session ID
   */
  private generateSessionId(): string {
    return `session_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`
  }
}

export default new TokenService()
