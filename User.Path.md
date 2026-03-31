# User Happy Path - API Flow

## 1. User Registration & Authentication Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                     USER HAPPY PATH SEQUENCE                     │
└─────────────────────────────────────────────────────────────────┘

START
  │
  ├─► [1] POST /api/auth/signup (Public - No Auth Required)
  │   Request:
  │   {
  │     "email": "newuser@example.com",
  │     "password": "SecurePassword123!",
  │     "firstName": "John",
  │     "lastName": "Doe"
  │   }
  │   Response: 200 OK
  │   {
  │     "userId": "550e8400-e29b-41d4-a716-446655440000",
  │     "email": "newuser@example.com",
  │     "role": "user",
  │     "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  │     "refreshToken": "base64encodedrefreshtoken",
  │     "expiresIn": 900  // 15 minutes in seconds
  │   }
  │   ✓ accessToken & refreshToken auto-saved to environment
  │   ✓ userId auto-saved to environment
  │
  ├─► [2] GET /api/users (List Users - Auth Required: Admin,Coach)
  │   Headers: Authorization: Bearer {{accessToken}}
  │   Response: 403 Forbidden (User role cannot list users)
  │   → Only Admin and Coach roles can list users
  │
  ├─► [3] GET /api/users/{{userId}} (Get Own Profile - Auth Required)
  │   Headers: Authorization: Bearer {{accessToken}}
  │   Request: GET /api/users/550e8400-e29b-41d4-a716-446655440000
  │   Response: 200 OK
  │   {
  │     "id": "550e8400-e29b-41d4-a716-446655440000",
  │     "email": "newuser@example.com",
  │     "firstName": "John",
  │     "lastName": "Doe",
  │     "age": null,
  │     "height": null,
  │     "weight": null,
  │     "gender": null,
  │     "fitnessGoal": null,
  │     "fitnessLevel": null,
  │     "role": "user",
  │     "isActive": true,
  │     "createdAt": "2026-04-01T10:00:00Z"
  │   }
  │   ✓ Can access own profile (userId in token matches URL param)
  │
  ├─► [4] PUT /api/users/{{userId}} (Update Own Profile - Auth Required)
  │   Headers: 
  │     Authorization: Bearer {{accessToken}}
  │     Content-Type: application/json
  │   Request:
  │   {
  │     "firstName": "John",
  │     "lastName": "Doe",
  │     "age": 28,
  │     "height": 180.5,
  │     "weight": 75.0,
  │     "gender": "male",
  │     "fitnessGoal": "muscleGain",
  │     "fitnessLevel": "intermediate"
  │   }
  │   Response: 204 No Content
  │   ✓ Profile updated successfully
  │
  ├─► [5] POST /api/users/{{userId}}/change-password (Change Password - Auth Required)
  │   Headers: 
  │     Authorization: Bearer {{accessToken}}
  │     Content-Type: application/json
  │   Request:
  │   {
  │     "currentPassword": "SecurePassword123!",
  │     "newPassword": "NewSecurePassword456!"
  │   }
  │   Response: 204 No Content
  │   ✓ Password changed successfully
  │
  ├─► [6] DELETE /api/users/{{userId}} (Delete Own Account - Auth Required)
  │   Headers: Authorization: Bearer {{accessToken}}
  │   Request: DELETE /api/users/550e8400-e29b-41d4-a716-446655440000
  │   Response: 204 No Content
  │   ✓ User soft-deleted (IsActive = false)
  │
  └─► END

```

---

## 2. User Login (Return User)

```
┌─────────────────────────────────────────────────────────────────┐
│                      LOGIN HAPPY PATH                            │
└─────────────────────────────────────────────────────────────────┘

START
  │
  ├─► [1] POST /api/auth/login (Public - No Auth Required)
  │   Request:
  │   {
  │     "email": "newuser@example.com",
  │     "password": "NewSecurePassword456!"  // After password change
  │   }
  │   Response: 200 OK
  │   {
  │     "userId": "550e8400-e29b-41d4-a716-446655440000",
  │     "email": "newuser@example.com",
  │     "role": "user",
  │     "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  │     "refreshToken": "newbase64encodedrefreshtoken",
  │     "expiresIn": 900
  │   }
  │   ✓ New tokens generated and auto-saved to environment
  │
  ├─► [2] GET /api/users/{{userId}} (Access Profile)
  │   Headers: Authorization: Bearer {{accessToken}}
  │   Response: 200 OK
  │   ✓ Can access own profile
  │
  └─► END

```

---

## 3. Access Control & Authorization Rules

### Unauthorized Access Attempts:

```
┌─────────────────────────────────────────────────────────────────┐
│                   UNAUTHORIZED ACCESS ATTEMPTS                   │
└─────────────────────────────────────────────────────────────────┘

❌ Attempt 1: No Authorization Header
  GET /api/users/{{userId}}
  Response: 401 Unauthorized
  Message: "Authorization header missing or invalid"

❌ Attempt 2: Invalid Token
  GET /api/users/{{userId}}
  Headers: Authorization: Bearer invalid_token_123
  Response: 401 Unauthorized
  Message: "Invalid token"

❌ Attempt 3: Expired Token (After 15 minutes)
  GET /api/users/{{userId}}
  Headers: Authorization: Bearer {{accessToken}}  (expired)
  Response: 401 Unauthorized
  Message: "Token expired"

❌ Attempt 4: Wrong User ID (User trying to access another user's profile)
  GET /api/users/550e8400-e29b-41d4-a716-446655440001  (different user)
  Headers: Authorization: Bearer {{accessToken}}
  Response: 403 Forbidden
  Message: "Access denied"
  Rule: tokenUserId (from JWT) != requestUserId (URL param) AND userRole != "Admin"

❌ Attempt 5: User trying to list all users
  GET /api/users
  Headers: Authorization: Bearer {{accessToken}}
  Response: 403 Forbidden
  Message: "Access denied - Admin or Coach role required"

```

---

## 4. Request Sequence Summary

| Order | Method | Endpoint | Auth | Role Allowed | Expected Status |
|-------|--------|----------|------|--------------|-----------------|
| 1 | POST | `/api/auth/signup` | ❌ | All | 200 OK |
| 2 | GET | `/api/users` | ✅ | Admin, Coach | 403 Forbidden (User) |
| 3 | GET | `/api/users/{id}` | ✅ | All (own) | 200 OK |
| 4 | PUT | `/api/users/{id}` | ✅ | All (own) | 204 No Content |
| 5 | POST | `/api/users/{id}/change-password` | ✅ | All (own) | 204 No Content |
| 6 | DELETE | `/api/users/{id}` | ✅ | All (own) | 204 No Content |
| 7 | POST | `/api/auth/login` | ❌ | All | 200 OK |

---

## 5. Environment Variables Auto-Population

### Signup/Login Response → Environment Variables

```javascript
// Post-Response Script (Automatically Executed)
if (pm.response.code === 200) {
  const jsonData = pm.response.json();
  
  // Set tokens
  pm.environment.set("accessToken", jsonData.accessToken);
  pm.environment.set("refreshToken", jsonData.refreshToken);
  
  // Set user ID
  pm.environment.set("userId", jsonData.userId);
}
```

### Subsequent Requests Auto-Use Token

```
Authorization Header:
  Key: Authorization
  Value: Bearer {{accessToken}}
  
This automatically injects the token from environment
```

---

## 6. Token Lifecycle

```
┌──────────────────────────────────────────────────────────────┐
│              JWT TOKEN STATELESS LIFECYCLE                   │
└──────────────────────────────────────────────────────────────┘

[Signup/Login]
       │
       ├─► Generate Access Token (15 min expiry)
       ├─► Generate Refresh Token (7 day expiry)
       └─► Return both + userId to client
              │
              ├─► accessToken: Valid for 15 minutes
              │   - Stored in {{accessToken}} env variable
              │   - Used in Authorization: Bearer header
              │   - Contains: userId, email, role claims
              │
              ├─► refreshToken: Valid for 7 days
              │   - Stored in {{refreshToken}} env variable
              │   - NOT used in happy path (stateless)
              │   - Can be used with POST /api/auth/refresh if needed
              │
              └─► After 15 minutes:
                  ├─► accessToken expires
                  ├─► Requests return 401 Unauthorized
                  └─► Must login again to get new token

⚠️  NO DATABASE REVOCATION
    - Tokens are stateless (no blacklist)
    - Valid until natural JWT expiration
    - Logout not supported in happy path
```

---

## 7. Postman Environment Setup

```json
{
  "baseUrl": "http://localhost:5123",
  "accessToken": "",           // ← Auto-filled by Signup/Login
  "refreshToken": "",          // ← Auto-filled by Signup/Login
  "userId": "",                // ← Auto-filled by Signup/Login
  "userEmail": "newuser@example.com",
  "userPassword": "SecurePassword123!"
}
```

**Usage in Postman:**
1. Run `[1] Signup` → tokens auto-saved
2. Run `[3] Get User By ID` → automatically uses `{{accessToken}}`
3. All protected endpoints use `Authorization: Bearer {{accessToken}}`

---

## 8. Error Scenarios & Fixes

| Scenario | Error | Fix |
|----------|-------|-----|
| No Authorization header | 401 Unauthorized | Ensure Bearer token in header |
| Token expired (15+ min) | 401 Unauthorized | Run Login endpoint to refresh |
| Accessing another user's data | 403 Forbidden | Only own userId allowed (unless Admin) |
| User tries to list users | 403 Forbidden | Only Admin/Coach can list users |
| Invalid email/password at login | 401 Invalid credentials | Check email and password |
| Email already registered | 400 Bad Request | Use different email for signup |

---

## Summary

✅ **Happy Path for Users:**
1. Signup → Get tokens + userId
2. View own profile
3. Update own profile  
4. Change password
5. Delete own account
6. Login (next time)

🔒 **Authorization Model:**
- **Token-based**: All requests use JWT from environment
- **User-scoped**: Can only access own data
- **Admin override**: Admins can access any user
- **Stateless**: No server-side token revocation
