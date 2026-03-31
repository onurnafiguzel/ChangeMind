# Coach Happy Path - API Flow

## 1. Coach Authentication & Management Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                    COACH HAPPY PATH SEQUENCE                     │
└─────────────────────────────────────────────────────────────────┘

START (Assuming Admin already exists)
  │
  ├─► [1] POST /api/coaches (Create Coach by Admin - Auth Required: Admin)
  │   Headers: 
  │     Authorization: Bearer {{adminAccessToken}}
  │     Content-Type: application/json
  │   Request:
  │   {
  │     "email": "coach@example.com",
  │     "password": "CoachPass123!",
  │     "firstName": "Alex",
  │     "lastName": "Coach",
  │     "specialization": "strength"
  │   }
  │   Response: 201 Created
  │   Location: /api/coaches/550e8400-e29b-41d4-a716-446655440001
  │   {
  │     "550e8400-e29b-41d4-a716-446655440001"
  │   }
  │   ✓ Coach account created with role: "coach"
  │
  ├─► [2] POST /api/coaches/login (Coach Login - Public)
  │   Request:
  │   {
  │     "email": "coach@example.com",
  │     "password": "CoachPass123!"
  │   }
  │   Response: 200 OK
  │   {
  │     "userId": "550e8400-e29b-41d4-a716-446655440001",
  │     "email": "coach@example.com",
  │     "role": "coach",
  │     "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  │     "refreshToken": "base64encodedrefreshtoken",
  │     "expiresIn": 900
  │   }
  │   ✓ accessToken & refreshToken auto-saved
  │   ✓ coachId auto-saved as userId field
  │
  ├─► [3] GET /api/coaches (List All Coaches - Auth Required: Admin,Coach)
  │   Headers: Authorization: Bearer {{accessToken}}
  │   Query Params:
  │     ?isActiveOnly=true&page=1&pageSize=10
  │   Response: 200 OK
  │   {
  │     "data": [
  │       {
  │         "id": "550e8400-e29b-41d4-a716-446655440001",
  │         "email": "coach@example.com",
  │         "firstName": "Alex",
  │         "lastName": "Coach",
  │         "specialization": "strength",
  │         "role": "coach",
  │         "isActive": true,
  │         "createdAt": "2026-04-01T10:00:00Z"
  │       }
  │     ],
  │     "totalCount": 1,
  │     "page": 1,
  │     "pageSize": 10,
  │     "totalPages": 1
  │   }
  │   ✓ Coach can view all coaches
  │
  ├─► [4] GET /api/coaches/{{coachId}} (Get Own Profile - Auth Required)
  │   Headers: Authorization: Bearer {{accessToken}}
  │   Request: GET /api/coaches/550e8400-e29b-41d4-a716-446655440001
  │   Response: 200 OK
  │   {
  │     "id": "550e8400-e29b-41d4-a716-446655440001",
  │     "email": "coach@example.com",
  │     "firstName": "Alex",
  │     "lastName": "Coach",
  │     "specialization": "strength",
  │     "role": "coach",
  │     "isActive": true,
  │     "createdAt": "2026-04-01T10:00:00Z"
  │   }
  │   ✓ Can access own profile (coachId in token matches URL param)
  │
  ├─► [5] PUT /api/coaches/{{coachId}} (Update Own Profile - Auth Required)
  │   Headers: 
  │     Authorization: Bearer {{accessToken}}
  │     Content-Type: application/json
  │   Request:
  │   {
  │     "firstName": "Alex",
  │     "lastName": "Coach",
  │     "specialization": "cardio"
  │   }
  │   Response: 204 No Content
  │   ✓ Profile updated successfully
  │
  ├─► [6] POST /api/coaches/{{coachId}}/change-password (Change Password - Auth Required)
  │   Headers: 
  │     Authorization: Bearer {{accessToken}}
  │     Content-Type: application/json
  │   Request:
  │   {
  │     "currentPassword": "CoachPass123!",
  │     "newPassword": "NewCoachPass456!"
  │   }
  │   Response: 204 No Content
  │   ✓ Password changed successfully
  │
  ├─► [7] DELETE /api/coaches/{{coachId}} (Delete Own Account - Auth Required)
  │   Headers: Authorization: Bearer {{accessToken}}
  │   Request: DELETE /api/coaches/550e8400-e29b-41d4-a716-446655440001
  │   Response: 204 No Content
  │   ✓ Coach soft-deleted (IsActive = false)
  │
  └─► END

```

---

## 2. Coach Login (Return Coach)

```
┌─────────────────────────────────────────────────────────────────┐
│                      COACH LOGIN HAPPY PATH                      │
└─────────────────────────────────────────────────────────────────┘

START
  │
  ├─► [1] POST /api/coaches/login (Public - No Auth Required)
  │   Request:
  │   {
  │     "email": "coach@example.com",
  │     "password": "NewCoachPass456!"  // After password change
  │   }
  │   Response: 200 OK
  │   {
  │     "userId": "550e8400-e29b-41d4-a716-446655440001",
  │     "email": "coach@example.com",
  │     "role": "coach",
  │     "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  │     "refreshToken": "newbase64encodedrefreshtoken",
  │     "expiresIn": 900
  │   }
  │   ✓ New tokens generated and auto-saved to environment
  │   ✓ coachId auto-saved as {{userId}} for convenience
  │
  ├─► [2] GET /api/coaches/{{coachId}} (Access Own Profile)
  │   Headers: Authorization: Bearer {{accessToken}}
  │   Response: 200 OK
  │   ✓ Can access own profile
  │
  └─► END

```

---

## 3. Admin Creating Multiple Coaches

```
┌─────────────────────────────────────────────────────────────────┐
│           ADMIN BULK COACH CREATION HAPPY PATH                   │
└─────────────────────────────────────────────────────────────────┘

Admin Login:
  POST /api/auth/login
  {
    "email": "admin@example.com",
    "password": "AdminPass123!"
  }
  ✓ Get {{accessToken}} with role: "admin"
  │
  ├─► [1] POST /api/coaches (Create Coach 1)
  │   Authorization: Bearer {{accessToken}}
  │   Request: Coach 1 details
  │   Response: 201 Created + coachId_1
  │
  ├─► [2] POST /api/coaches (Create Coach 2)
  │   Authorization: Bearer {{accessToken}}
  │   Request: Coach 2 details
  │   Response: 201 Created + coachId_2
  │
  ├─► [3] GET /api/coaches (View All Coaches)
  │   Authorization: Bearer {{accessToken}}
  │   Response: List with all coaches
  │   ✓ Admin can see all coaches
  │
  └─► END

```

---

## 4. Access Control & Authorization Rules

### Authorized Access (Coach):

```
✅ Coach Can:
  - View own profile: GET /api/coaches/{{coachId}}
  - Update own profile: PUT /api/coaches/{{coachId}}
  - Change own password: POST /api/coaches/{{coachId}}/change-password
  - Delete own account: DELETE /api/coaches/{{coachId}}
  - List all coaches: GET /api/coaches
  - View other coaches: GET /api/coaches/{otherId}
```

### Unauthorized Access Attempts:

```
┌─────────────────────────────────────────────────────────────────┐
│                   UNAUTHORIZED ACCESS ATTEMPTS                   │
└─────────────────────────────────────────────────────────────────┘

❌ Attempt 1: Coach trying to create another coach
  POST /api/coaches
  Headers: Authorization: Bearer {{coachAccessToken}}
  Response: 403 Forbidden
  Message: "Admin role required to create coaches"

❌ Attempt 2: Coach accessing another coach's data (without permission)
  GET /api/coaches/550e8400-e29b-41d4-a716-446655440002
  Headers: Authorization: Bearer {{coachAccessToken}}
  Response: ✓ 200 OK (Coaches can view each other - might change based on requirements)
  Note: If permission restriction needed, add business logic in handler

❌ Attempt 3: Coach trying to update another coach's profile
  PUT /api/coaches/550e8400-e29b-41d4-a716-446655440002
  Headers: Authorization: Bearer {{coachAccessToken}}
  Response: 403 Forbidden
  Message: "Cannot modify other coaches' profiles"
  Rule: tokenCoachId != requestCoachId AND userRole != "Admin"

❌ Attempt 4: No authorization header
  GET /api/coaches
  Response: 401 Unauthorized
  Message: "Authorization header missing or invalid"

❌ Attempt 5: Invalid or expired token
  GET /api/coaches/{{coachId}}
  Headers: Authorization: Bearer invalid_token_123
  Response: 401 Unauthorized

```

---

## 5. Request Sequence Summary

| Order | Method | Endpoint | Auth | Role Allowed | Expected Status |
|-------|--------|----------|------|--------------|-----------------|
| 1 | POST | `/api/coaches` | ✅ | Admin only | 201 Created |
| 2 | POST | `/api/coaches/login` | ❌ | All | 200 OK |
| 3 | GET | `/api/coaches` | ✅ | Admin, Coach | 200 OK |
| 4 | GET | `/api/coaches/{id}` | ✅ | Admin, Coach (own) | 200 OK |
| 5 | PUT | `/api/coaches/{id}` | ✅ | Admin, Coach (own) | 204 No Content |
| 6 | POST | `/api/coaches/{id}/change-password` | ✅ | Admin, Coach (own) | 204 No Content |
| 7 | DELETE | `/api/coaches/{id}` | ✅ | Admin, Coach (own) | 204 No Content |

---

## 6. Environment Variables Auto-Population

### Coach Login → Environment Variables

```javascript
// Post-Response Script
if (pm.response.code === 200) {
  const jsonData = pm.response.json();
  
  // Set tokens
  pm.environment.set("accessToken", jsonData.accessToken);
  pm.environment.set("refreshToken", jsonData.refreshToken);
  
  // Set coach ID (stored as userId in response)
  pm.environment.set("coachId", jsonData.userId);
}
```

### Subsequent Requests Auto-Use Token

```
Authorization Header:
  Key: Authorization
  Value: Bearer {{accessToken}}
```

---

## 7. Role Hierarchy & Permissions

```
┌──────────────────────────────────────────────────────────────┐
│              ROLE-BASED ACCESS CONTROL (RBAC)               │
└──────────────────────────────────────────────────────────────┘

👨‍💼 ADMIN
  ├─ Create Coaches: ✓
  ├─ Manage all Coaches: ✓
  ├─ Manage all Users: ✓
  ├─ Create Users: ✓
  └─ View all data: ✓

👨‍🏫 COACH
  ├─ Create Coaches: ✗ (Admin only)
  ├─ View own Coach profile: ✓
  ├─ Update own Coach profile: ✓
  ├─ View other Coaches: ✓ (read-only)
  ├─ View all Users: ✓
  ├─ View own User data: ✓
  └─ Manage assigned Users: ✓ (depends on business logic)

👤 USER
  ├─ Create Coaches: ✗
  ├─ View own User profile: ✓
  ├─ Update own User profile: ✓
  ├─ View other Users: ✗
  ├─ View Coaches: ✗
  └─ View Users list: ✗

```

---

## 8. Error Scenarios & Fixes

| Scenario | Error | Fix |
|----------|-------|-----|
| Coach tries to create another coach | 403 Forbidden | Only Admin can create coaches |
| No authorization header | 401 Unauthorized | Ensure Bearer token in header |
| Token expired (15+ min) | 401 Unauthorized | Run Coach Login to get new token |
| Accessing another coach's sensitive data | 403 Forbidden | Can only modify own profile |
| Invalid email/password at login | 401 Invalid credentials | Check email and password |
| Email already registered | 400 Bad Request | Use different email for signup |
| Admin token used for coach operations | ✓ Works | Admin has all permissions |

---

## 9. Postman Environment Setup

```json
{
  "baseUrl": "http://localhost:5123",
  "accessToken": "",           // ← Auto-filled by Coach Login
  "refreshToken": "",          // ← Auto-filled by Coach Login
  "coachId": "",               // ← Auto-filled by Coach Login
  "coachEmail": "coach@example.com",
  "coachPassword": "CoachPass123!"
}
```

---

## Summary

✅ **Happy Path for Coaches:**
1. Admin creates coach account
2. Coach login → Get tokens + coachId
3. View own profile
4. Update own profile
5. Change password
6. View other coaches
7. Delete own account
8. Re-login with new password

🔒 **Authorization Model:**
- **Token-based**: JWT from login
- **Coach-scoped**: Can only manage own data
- **Admin override**: Admins can create and manage coaches
- **Role-based**: Different endpoints for different roles
- **Stateless**: No server-side revocation
