# JWT Anonymous Token Authentication Guide

## Overview

This implementation provides a public interface for obtaining anonymous JWT tokens that can be used for API authentication without requiring user identity information.

## API Endpoints

### 1. Get Anonymous Token
**Endpoint:** `POST /api/auth/anonymous-token`

**Description:** Generates a temporary JWT token for anonymous access.

**Request:**
```http
POST /api/auth/anonymous-token
Content-Type: application/json
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2025-06-13T03:04:32.4152056Z",
  "tokenType": "Bearer"
}
```

### 2. Health Check
**Endpoint:** `GET /api/auth/health`

**Description:** Verifies that the authentication service is working.

**Response:**
```json
{
  "status": "healthy",
  "timestamp": "2025-06-13T02:04:37.3846232Z"
}
```

## Token Details

### Token Claims
The anonymous JWT token contains the following claims:
- `type`: "anonymous" - Identifies this as an anonymous token
- `session_id`: Unique session identifier
- `jti`: JWT ID (unique token identifier)
- `iat`: Issued at timestamp
- `exp`: Expiration timestamp
- `iss`: Issuer (KnightfrankEvaluation)
- `aud`: Audience (KnightfrankEvaluationUsers)

### Token Expiration
- Default expiration: 60 minutes
- Configurable via `JwtSettings.AnonymousTokenExpirationMinutes` in appsettings.json

## Frontend Integration

### 1. Initialize Application with Token

```javascript
// Example: Initialize token when application starts
async function initializeApp() {
  try {
    const response = await fetch('/api/auth/anonymous-token', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      }
    });
    
    const tokenData = await response.json();
    
    // Store token in cookie
    document.cookie = `auth_token=${tokenData.token}; path=/; max-age=${60 * 60}; SameSite=Strict`;
    
    // Store expiration for token refresh logic
    localStorage.setItem('token_expires_at', tokenData.expiresAt);
    
    console.log('Anonymous token obtained successfully');
  } catch (error) {
    console.error('Failed to obtain anonymous token:', error);
  }
}

// Call on app initialization
initializeApp();
```

### 2. Use Token in API Requests

```javascript
// Example: Making authenticated API requests
function getAuthToken() {
  const cookies = document.cookie.split(';');
  const tokenCookie = cookies.find(cookie => cookie.trim().startsWith('auth_token='));
  return tokenCookie ? tokenCookie.split('=')[1] : null;
}

async function makeAuthenticatedRequest(url, options = {}) {
  const token = getAuthToken();
  
  const headers = {
    'Content-Type': 'application/json',
    ...options.headers
  };
  
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }
  
  return fetch(url, {
    ...options,
    headers
  });
}

// Example usage
async function searchProperties(searchRequest) {
  const response = await makeAuthenticatedRequest('/api/properties/search', {
    method: 'POST',
    body: JSON.stringify(searchRequest)
  });
  
  return response.json();
}
```

### 3. Token Refresh Logic

```javascript
// Example: Check if token needs refresh
function isTokenExpired() {
  const expiresAt = localStorage.getItem('token_expires_at');
  if (!expiresAt) return true;
  
  const expirationTime = new Date(expiresAt);
  const now = new Date();
  const fiveMinutesFromNow = new Date(now.getTime() + 5 * 60 * 1000);
  
  return expirationTime <= fiveMinutesFromNow;
}

// Example: Refresh token if needed
async function refreshTokenIfNeeded() {
  if (isTokenExpired()) {
    await initializeApp(); // Re-initialize to get new token
  }
}

// Set up periodic token refresh
setInterval(refreshTokenIfNeeded, 5 * 60 * 1000); // Check every 5 minutes
```

## Backend Configuration

### JWT Settings (appsettings.json)
```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "KnightfrankEvaluation",
    "Audience": "KnightfrankEvaluationUsers",
    "AnonymousTokenExpirationMinutes": 60
  }
}
```

### Protecting Endpoints

To require JWT authentication for specific endpoints, add the `[Authorize]` attribute:

```csharp
[ApiController]
[Route("api/[controller]")]
[Authorize] // Require JWT authentication for all endpoints in this controller
public class PropertiesController : ControllerBase
{
    // ... controller methods
}
```

Or protect individual methods:

```csharp
[HttpGet("{id}")]
[Authorize] // Require JWT authentication for this specific endpoint
public async Task<ActionResult<Property>> GetProperty(int id)
{
    // ... method implementation
}
```

## Security Considerations

1. **HTTPS Only**: In production, ensure all token exchanges happen over HTTPS
2. **Secure Cookies**: Use secure, HttpOnly cookies for token storage in production
3. **Token Rotation**: Implement token refresh logic to minimize exposure window
4. **Rate Limiting**: Consider implementing rate limiting on the token endpoint
5. **Secret Key**: Use a strong, randomly generated secret key in production

## Testing

### Using cURL
```bash
# Get anonymous token
curl -X POST http://localhost:12001/api/auth/anonymous-token

# Use token in API request
TOKEN=$(curl -s -X POST http://localhost:12001/api/auth/anonymous-token | jq -r '.token')
curl -H "Authorization: Bearer $TOKEN" http://localhost:12001/api/properties/filters
```

### Using Postman
1. Create a POST request to `/api/auth/anonymous-token`
2. Copy the returned token
3. Add Authorization header: `Bearer <token>` to subsequent requests

## Troubleshooting

### Common Issues
1. **401 Unauthorized**: Check that the Authorization header is properly formatted
2. **Token Expired**: Ensure token refresh logic is working correctly
3. **CORS Issues**: Verify CORS configuration includes your frontend domain
4. **Invalid Token**: Check that the secret key matches between token generation and validation

### Debug Information
- Check server logs for JWT validation errors
- Verify token claims using jwt.io
- Ensure system clocks are synchronized for token expiration