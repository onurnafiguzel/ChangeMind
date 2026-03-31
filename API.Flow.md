# ChangeMind API - Complete Flow Map

## System Architecture Overview

```
┌────────────────────────────────────────────────────────────────────────┐
│                        CHANGERMIND API ARCHITECTURE                    │
└────────────────────────────────────────────────────────────────────────┘

                          ┌──────────────────┐
                          │   Postman Client │
                          └────────┬─────────┘
                                   │
                    ┌──────────────┼──────────────┐
                    │              │              │
          ┌─────────▼────┐ ┌──────▼────┐ ┌──────▼────────┐
          │ Bearer Token │ │ Base URL  │ │ Environment   │
          │ Management  │ │ {{baseUrl}}│ │ Variables     │
          └──────────────┘ └───────────┘ └───────────────┘
                    │              │              │
                    └──────────────┼──────────────┘
                                   │
                        ┌──────────▼────────────┐
                        │  ASP.NET Core 10 API │
                        │   (Port 5123)         │
                        └──────────┬────────────┘
                                   │
        ┌──────────────────────────┼──────────────────────────┐
        │                          │                          │
        │                    Middleware Stack                  │
        │   ┌─────────────────────────────────────────────┐   │
        │   │ 1. UseHttpsRedirection                      │   │
        │   │ 2. UseAuthentication (JWT Bearer)           │   │
        │   │ 3. UseAuthorization (Role-based)            │   │
        │   │ 4. MapControllers                           │   │
        │   └─────────────────────────────────────────────┘   │
        │                          │                          │
        ├──────────────────────────┼──────────────────────────┤
        │                          │                          │
    ┌───▼────────┐          ┌──────▼──────┐          ┌───▼────────┐
    │ AuthCtrl   │          │ UsersCtrl   │          │ CoachesCtrl│
    └───┬────────┘          └──────┬──────┘          └───┬────────┘
        │                          │                     │
    ┌───┴─────────────┐      ┌─────┴────────────┐  ┌───┴──────────┐
    │ Signup (Public) │      │ Get All (Auth)   │  │ Login(Public)│
    │ Login (Public)  │      │ Get By ID (Auth) │  │ Create(Admin)│
    │ Refresh(Old)    │      │ Update (Auth)    │  │ Update(Auth) │
    └─────────────────┘      │ Delete (Auth)    │  │ Delete(Auth) │
                             │ ChangePass(Auth) │  │ ChangePass..│
                             └──────────────────┘  └──────────────┘
                                     │                     │
                                     └─────────┬───────────┘
                                               │
                            ┌──────────────────▼─────────────────────┐
                            │     Application Layer (MediatR CQRS)   │
                            │  ┌─────────────────────────────────┐   │
                            │  │ Commands & Queries              │   │
                            │  │ - SignupCommand                 │   │
                            │  │ - LoginCommand                  │   │
                            │  │ - GetUsersQuery                 │   │
                            │  │ - CreateCoachCommand            │   │
                            │  │ - UpdateUserCommand             │   │
                            │  └─────────────────────────────────┘   │
                            │                                         │
                            │  ┌─────────────────────────────────┐   │
                            │  │ Services                        │   │
                            │  │ - ITokenService                 │   │
                            │  │ - IPasswordService              │   │
                            │  │ - IUserRepository               │   │
                            │  │ - ICoachRepository              │   │
                            │  └─────────────────────────────────┘   │
                            └────────────────────┬────────────────────┘
                                                 │
                            ┌────────────────────▼────────────────────┐
                            │  Infrastructure Layer                   │
                            │  ┌─────────────────────────────────┐   │
                            │  │ Database Context                │   │
                            │  │ - ChangeMindDbContext           │   │
                            │  │ - PostgreSQL via Npgsql         │   │
                            │  │ - Entity Framework Core         │   │
                            │  └─────────────────────────────────┘   │
                            │                                         │
                            │  ┌─────────────────────────────────┐   │
                            │  │ Services Implementation         │   │
                            │  │ - TokenService (JWT HS256)      │   │
                            │  │ - PasswordService (SHA256)      │   │
                            │  │ - UserRepository                │   │
                            │  │ - CoachRepository               │   │
                            │  └─────────────────────────────────┘   │
                            └────────────────────┬────────────────────┘
                                                 │
                            ┌────────────────────▼────────────────────┐
                            │  PostgreSQL Database                    │
                            │  - Users table                          │
                            │  - Coaches table                        │
                            │  - UserPhotos, CoachUsers, etc.         │
                            └─────────────────────────────────────────┘
```

---

## 1. Authentication Flow (Stateless JWT)

```
┌────────────────────────────────────────────────────────────────────┐
│               AUTHENTICATION & TOKEN FLOW                          │
└────────────────────────────────────────────────────────────────────┘

CLIENT INITIATES AUTH
  │
  ├─► POST /api/auth/signup or POST /api/auth/login
  │   (Public endpoints - no token required)
  │
  │   Request:
  │   {
  │     "email": "user@example.com",
  │     "password": "SecurePassword123!"
  │   }
  │
  └─► API PROCESSES:
      │
      ├─► [1] Validate input (email, password format)
      │
      ├─► [2] Hash password with SHA256
      │       (for storage verification only)
      │
      ├─► [3] Check email uniqueness
      │
      ├─► [4] Create User/Coach entity in DB
      │
      └─► [5] Generate Tokens:
          │
          ├─► ACCESS TOKEN:
          │   - HS256 signed JWT
          │   - Claims: NameIdentifier (userId), Email, Role
          │   - Expires: 15 minutes
          │   - Encoding: Base64 header.payload.signature
          │
          └─► REFRESH TOKEN:
              - Random 64 bytes
              - Base64 encoded
              - Expires: 7 days
              - Stateless (no DB storage)

RESPONSE TO CLIENT:
  {
    "userId": "guid",
    "email": "user@example.com",
    "role": "user|coach|admin",
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "base64encodedstring",
    "expiresIn": 900  // seconds
  }

CLIENT STORES IN POSTMAN ENVIRONMENT:
  - {{accessToken}} ← Used for all requests
  - {{refreshToken}} ← Optional, for renewal
  - {{userId}} ← For request paths

SUBSEQUENT REQUESTS:
  GET /api/users/{{userId}}
  Headers:
    Authorization: Bearer {{accessToken}}

API VALIDATES TOKEN:
  │
  ├─► Extract token from Authorization header
  │
  ├─► Verify signature (HS256 with secret)
  │
  ├─► Check issuer, audience, expiration
  │
  ├─► Extract claims (userId, email, role)
  │
  └─► Proceed or return 401 Unauthorized

TOKEN LIFETIME:
  - Access Token: 15 minutes (expires automatically)
  - After expiration: 401 Unauthorized on next request
  - No logout/blacklist: Token valid until expiration
  - Cannot be revoked (stateless design)

```

---

## 2. Authorization & Role Checking

```
┌────────────────────────────────────────────────────────────────────┐
│            AUTHORIZATION & ROLE-BASED ACCESS CONTROL              │
└────────────────────────────────────────────────────────────────────┘

REQUEST HITS ENDPOINT:
  GET /api/users/{{userId}}
  
  ├─ Check 1: [Authorize] attribute
  │   ├─► Token present? YES → Continue
  │   └─► Token present? NO → 401 Unauthorized
  │
  ├─ Check 2: Token validity
  │   ├─► Signature valid? YES → Continue
  │   └─► Signature valid? NO → 401 Unauthorized
  │
  ├─ Check 3: Token not expired
  │   ├─► Now < ExpiresAt? YES → Continue
  │   └─► Now < ExpiresAt? NO → 401 Unauthorized
  │
  ├─ Check 4: Role validation [if roles specified]
  │   ├─► Role in [Admin,Coach]? YES → Continue
  │   └─► Role in [Admin,Coach]? NO → 403 Forbidden
  │
  └─ Check 5: Resource ownership [custom logic]
      ├─► tokenUserId == resourceUserId? YES → Continue
      ├─► userRole == "Admin"? YES → Continue
      └─► Otherwise → 403 Forbidden

┌─────────────────────────────────────────────────────────────┐
│  ENDPOINTS & REQUIRED AUTHORIZATION                        │
├─────────────────────────────────────────────────────────────┤
│ Public (No Auth):                                           │
│   POST /api/auth/signup                                     │
│   POST /api/auth/login                                      │
│   POST /api/coaches/login                                   │
├─────────────────────────────────────────────────────────────┤
│ Admin Only:                                                 │
│   POST /api/coaches            [Authorize(Roles="Admin")]   │
├─────────────────────────────────────────────────────────────┤
│ Admin or Coach:                                             │
│   GET /api/users               [Authorize(Roles="Admin,...")]│
│   GET /api/coaches             [Authorize(Roles="Admin,...")]│
├─────────────────────────────────────────────────────────────┤
│ Any Authenticated User:                                     │
│   GET /api/users/{id}          [Authorize]                  │
│   PUT /api/users/{id}          [Authorize]                  │
│   DELETE /api/users/{id}       [Authorize]                  │
│   POST /api/users/{id}/...     [Authorize]                  │
│   (Same for /api/coaches/{id}) [Authorize]                  │
│                                                             │
│   + Custom logic:                                           │
│   - tokenUserId == {id} OR userRole == "Admin" allowed      │
│   - Others → 403 Forbidden                                  │
└─────────────────────────────────────────────────────────────┘

```

---

## 3. Request/Response Sequence Diagrams

### User Registration → Access Profile

```
CLIENT                              API                             DB
  │                                 │                              │
  ├──────────POST /api/auth/signup──▶                              │
  │    { email, password, name}      │                              │
  │                                  │                              │
  │                                  ├──────Validate & Hash────────│
  │                                  │                              │
  │                                  ├──────INSERT User───────────▶│
  │                                  │                              │
  │                                  │◀──────userId (guid)──────────┤
  │                                  │                              │
  │                                  ├──Create JWT Token────────────┤
  │                                  │ (15 min expiry)              │
  │                                  │                              │
  │◀─────200 OK + Tokens────────────│                              │
  │ {accessToken, refreshToken}      │                              │
  │                                  │                              │
  │ [Save to environment]            │                              │
  │ {{accessToken}}                  │                              │
  │ {{userId}}                       │                              │
  │                                  │                              │
  ├──GET /api/users/{{userId}}──────▶                              │
  │ Bearer {{accessToken}}           │                              │
  │                                  ├──Validate Token─────────────┤
  │                                  │                              │
  │                                  ├──Check ownership────────────│
  │                                  │ (tokenUserId == pathId)      │
  │                                  │                              │
  │                                  ├──SELECT User───────────────▶│
  │                                  │                              │
  │                                  │◀──User data─────────────────┤
  │◀─────200 OK + User DTO───────────│                              │
  │                                  │                              │
  └─────────────────────────────────── ──────────────────────────── ─

```

### Authorization Failure Scenarios

```
┌──────────────────────────────────────────────────────────┐
│  SCENARIO 1: No Authorization Header                     │
├──────────────────────────────────────────────────────────┤
│ GET /api/users/{{userId}}                                │
│ (Headers: [none])                                        │
│                                                          │
│ Response: 401 Unauthorized                               │
│ Message: "Authorization header missing"                  │
└──────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│  SCENARIO 2: Invalid Token                               │
├──────────────────────────────────────────────────────────┤
│ GET /api/users                                           │
│ Headers: Authorization: Bearer invalid_token             │
│                                                          │
│ Response: 401 Unauthorized                               │
│ Message: "Invalid token signature"                       │
└──────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│  SCENARIO 3: Expired Token (15+ min)                     │
├──────────────────────────────────────────────────────────┤
│ GET /api/users/{{userId}}                                │
│ Headers: Authorization: Bearer {{accessToken}}(expired)  │
│                                                          │
│ Response: 401 Unauthorized                               │
│ Message: "Token has expired"                             │
│                                                          │
│ Fix: Re-login to get new token                           │
└──────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│  SCENARIO 4: User Accessing Another User's Data          │
├──────────────────────────────────────────────────────────┤
│ GET /api/users/550e8400-e29b-41d4-a716-446655440002     │
│ Headers: Authorization: Bearer {{userAccessToken}}       │
│ (token userId = 550e8400-e29b-41d4-a716-446655440001)   │
│                                                          │
│ Response: 403 Forbidden                                  │
│ Message: "Access denied"                                 │
│                                                          │
│ Rule: tokenUserId != requestUserId AND role != "Admin"   │
└──────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────┐
│  SCENARIO 5: User Trying Admin Operation                 │
├──────────────────────────────────────────────────────────┤
│ POST /api/coaches                                        │
│ Headers: Authorization: Bearer {{userAccessToken}}       │
│ (token role = "user")                                    │
│                                                          │
│ Response: 403 Forbidden                                  │
│ Message: "Admin role required"                           │
│                                                          │
│ Rule: Endpoint requires [Authorize(Roles="Admin")]       │
└──────────────────────────────────────────────────────────┘

```

---

## 4. Database Schema (Relevant to Auth)

```
┌──────────────────────────────────────────────────────────┐
│                    USERS TABLE                            │
├──────────────────────────────────────────────────────────┤
│ Column              │ Type        │ Constraints           │
├─────────────────────┼─────────────┼──────────────────────┤
│ Id                  │ UUID        │ PRIMARY KEY           │
│ Email               │ VARCHAR(255)│ UNIQUE, NOT NULL      │
│ PasswordHash        │ VARCHAR(512)│ NOT NULL              │
│ FirstName           │ VARCHAR(100)│ NOT NULL              │
│ LastName            │ VARCHAR(100)│ NOT NULL              │
│ Role                │ VARCHAR(50) │ NOT NULL (enum)       │
│ IsActive            │ BOOLEAN     │ DEFAULT true          │
│ CreatedAt           │ TIMESTAMP   │ DEFAULT NOW()         │
│ UpdatedAt           │ TIMESTAMP   │ NULL                  │
│ (age, height, etc.) │ ...         │ Fitness tracking      │
└──────────────────────────────────────────────────────────┘

Role Enum Values: "User", "Coach", "Admin"

┌──────────────────────────────────────────────────────────┐
│                    COACHES TABLE                          │
├──────────────────────────────────────────────────────────┤
│ Column              │ Type        │ Constraints           │
├─────────────────────┼─────────────┼──────────────────────┤
│ Id                  │ UUID        │ PRIMARY KEY           │
│ Email               │ VARCHAR(255)│ UNIQUE, NOT NULL      │
│ PasswordHash        │ VARCHAR(512)│ NOT NULL              │
│ FirstName           │ VARCHAR(100)│ NOT NULL              │
│ LastName            │ VARCHAR(100)│ NOT NULL              │
│ Role                │ VARCHAR(50) │ NOT NULL (always Coach│
│ Specialization      │ VARCHAR(100)│ NULL (enum)           │
│ IsActive            │ BOOLEAN     │ DEFAULT true          │
│ CreatedAt           │ TIMESTAMP   │ DEFAULT NOW()         │
│ UpdatedAt           │ TIMESTAMP   │ NULL                  │
└──────────────────────────────────────────────────────────┘

Role Enum Values: "Coach" (always)
Specialization: "Strength", "Cardio", "Flexibility", etc.

⚠️  NOTE: RefreshToken storage removed (stateless design)
    - Tokens expire naturally (no revocation needed)
    - Reduces DB overhead
    - Simpler architecture

```

---

## 5. Complete API Endpoint Reference

| # | Method | Endpoint | Auth | Role | Purpose |
|---|--------|----------|------|------|---------|
| 1 | POST | `/api/auth/signup` | ❌ | - | User self-registration |
| 2 | POST | `/api/auth/login` | ❌ | - | User login |
| 3 | POST | `/api/coaches/login` | ❌ | - | Coach login |
| 4 | GET | `/api/users` | ✅ | Admin,Coach | List all users |
| 5 | GET | `/api/users/{id}` | ✅ | Any (own) | Get user profile |
| 6 | POST | `/api/users` | ❌ | - | Create user (via signup) |
| 7 | PUT | `/api/users/{id}` | ✅ | Any (own) | Update user profile |
| 8 | DELETE | `/api/users/{id}` | ✅ | Any (own) | Soft delete user |
| 9 | POST | `/api/users/{id}/change-password` | ✅ | Any (own) | Change password |
| 10 | GET | `/api/coaches` | ✅ | Admin,Coach | List all coaches |
| 11 | GET | `/api/coaches/{id}` | ✅ | Admin,Coach (own) | Get coach profile |
| 12 | POST | `/api/coaches` | ✅ | Admin | Create coach (admin only) |
| 13 | PUT | `/api/coaches/{id}` | ✅ | Admin,Coach (own) | Update coach profile |
| 14 | DELETE | `/api/coaches/{id}` | ✅ | Admin,Coach (own) | Soft delete coach |
| 15 | POST | `/api/coaches/{id}/change-password` | ✅ | Admin,Coach (own) | Change password |

---

## 6. Development Checklist

```
✅ Build Project
  dotnet build

⏳ Create Database Migration
  dotnet ef migrations add AddAuthenticationFields --project src/ChangeMind.Infrastructure

⏳ Update Database
  dotnet ef database update --project src/ChangeMind.Infrastructure

⏳ Run API
  dotnet run --project src/ChangeMind.Api

⏳ Import Postman Collection & Environment
  - postman_collection.json
  - postman_environment.json

⏳ Test Happy Paths
  - User.Path.md
  - Coach.Path.md

⏳ Verify Authorization
  - Test 403 Forbidden scenarios
  - Test 401 Unauthorized scenarios
  - Test role-based access control

```

---

## 7. Quick Reference - Common Tasks

### Get Authenticated User Info from Token

```csharp
var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
var email = User.FindFirst(ClaimTypes.Email)?.Value;
var role = User.FindFirst(ClaimTypes.Role)?.Value;

// In endpoint with Postman
// Bearer token automatically decoded by JwtBearerDefaults middleware
```

### Add Authorization to Endpoint

```csharp
// Any authenticated user
[Authorize]
[HttpGet("{id:guid}")]
public async Task<ActionResult<UserDto>> GetUserById(Guid id) { }

// Specific roles
[Authorize(Roles = "Admin,Coach")]
[HttpGet]
public async Task<ActionResult<PagedResult<UserDto>>> GetUsers() { }
```

### Check Resource Ownership

```csharp
private bool IsAuthorizedForUser(Guid userId)
{
    var tokenUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var userRoleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

    if (Guid.TryParse(tokenUserIdClaim, out var tokenUserId))
    {
        return tokenUserId == userId || userRoleClaim == "Admin";
    }
    return false;
}
```

### Generate JWT Token

```csharp
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
    new Claim(ClaimTypes.Email, email),
    new Claim(ClaimTypes.Role, role.ToString())
};

var token = new JwtSecurityToken(
    issuer: jwtSettings.Issuer,
    audience: jwtSettings.Audience,
    claims: claims,
    expires: DateTime.UtcNow.AddMinutes(15),
    signingCredentials: credentials);

return new JwtSecurityTokenHandler().WriteToken(token);
```

---

## Summary

✅ **System Features:**
- Stateless JWT authentication (no blacklist)
- Role-based authorization (Admin, Coach, User)
- Resource ownership checks
- Auto-token management in Postman
- SHA256 password hashing
- 15-min access token + 7-day refresh token
- PostgreSQL database with EF Core

📚 **Documentation:**
- `User.Path.md` → User happy path flows
- `Coach.Path.md` → Coach happy path flows
- `API.Flow.md` → This file, complete system overview

🔒 **Security:**
- HS256 JWT signature verification
- Authorization header validation
- Role-based access control
- Resource ownership enforcement
- No sensitive data in JWT claims
