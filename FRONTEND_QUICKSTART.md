# ChangeMind Frontend - Quick Start Guide

## Setup

### 1. Base URL Configuration
```typescript
// environment.ts
export const API_BASE_URL = 'https://api.changemind.com';

// In development:
export const API_BASE_URL = 'http://localhost:5123';
```

### 2. Generate API Client (Optional but Recommended)
```bash
# Install OpenAPI Generator
npm install -g @openapitools/openapi-generator-cli

# Generate TypeScript Axios client from openapi.json
openapi-generator-cli generate -i ./openapi.json \
  -g typescript-axios \
  -o ./src/generated/api
```

---

## Common API Calls

### 1. User Login
```typescript
const response = await fetch('https://api.changemind.com/api/auth/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    email: 'user@example.com',
    password: 'password123'
  })
});

const data = await response.json();
// {
//   userId: "...",
//   email: "user@example.com",
//   role: "User|Coach|Admin",
//   accessToken: "eyJ...",
//   refreshToken: "eyJ...",
//   expiresIn: 900
// }

// Save tokens
localStorage.setItem('accessToken', data.accessToken);
localStorage.setItem('refreshToken', data.refreshToken);
localStorage.setItem('role', data.role);
```

### 2. Protected API Request (with JWT)
```typescript
const userId = '550e8400-e29b-41d4-a716-446655440000';
const token = localStorage.getItem('accessToken');

const response = await fetch(`https://api.changemind.com/api/users/${userId}`, {
  method: 'GET',
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
});

const user = await response.json();
```

### 3. Get All Packages
```typescript
const response = await fetch('https://api.changemind.com/api/packages?page=1&pageSize=10');
const data = await response.json();
// {
//   data: [...PackageDto[]],
//   total: number,
//   page: number,
//   pageSize: number,
//   totalPages: number
// }
```

### 4. Get Exercises (with Filters)
```typescript
const params = new URLSearchParams({
  muscleGroup: 'Chest',
  difficultyLevel: 'Beginner',
  page: '1',
  pageSize: '20'
});

const response = await fetch(
  `https://api.changemind.com/api/exercises?${params}`
);
const exercises = await response.json();
```

### 5. Process Payment (Idempotent)
```typescript
import { v4 as uuidv4 } from 'uuid';

const idempotencyKey = uuidv4();
const token = localStorage.getItem('accessToken');

const response = await fetch('https://api.changemind.com/api/payments', {
  method: 'POST',
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json',
    'Idempotency-Key': idempotencyKey  // ← REQUIRED
  },
  body: JSON.stringify({
    userId: '550e8400-e29b-41d4-a716-446655440000',
    packageId: '660e8400-e29b-41d4-a716-446655440000',
    description: 'Monthly subscription'
  })
});

// Check if response was cached (idempotent replay)
const replayed = response.headers.get('X-Idempotent-Replayed') === 'true';

const data = await response.json();
// {
//   success: true,
//   paymentId: "...",
//   message: "Payment processed successfully"
// }
```

### 6. Create Training Program (Coach Only)
```typescript
const token = localStorage.getItem('accessToken');

const response = await fetch('https://api.changemind.com/api/training-programs', {
  method: 'POST',
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    name: '12-Week Muscle Building',
    description: 'Focus on compound movements',
    userId: '550e8400-e29b-41d4-a716-446655440000',
    durationWeeks: 12,
    difficulty: 'Intermediate'
  })
});

const programId = await response.text(); // Returns UUID as plain text
```

### 7. Update Daily Exercises in Program
```typescript
const programId = '660e8400-e29b-41d4-a716-446655440000';
const token = localStorage.getItem('accessToken');

const response = await fetch(
  `https://api.changemind.com/api/training-programs/${programId}/daily-program`,
  {
    method: 'PUT',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      'Day-1': [
        {
          exerciseId: '550e8400-e29b-41d4-a716-446655440000',
          sets: 3,
          reps: '10-12',
          explanation: 'Full ROM, controlled descent'
        },
        {
          exerciseId: '660e8400-e29b-41d4-a716-446655440000',
          sets: 4,
          reps: '8-10'
        }
      ],
      'Day-2': [
        {
          exerciseId: '770e8400-e29b-41d4-a716-446655440000',
          sets: 3,
          reps: 'AMRAP'
        }
      ]
    })
  }
);
// Returns 204 No Content
```

---

## Error Handling

### Check Response Status
```typescript
const response = await fetch(url, options);

if (!response.ok) {
  const error = await response.json();
  // {
  //   title: "Not Found",
  //   detail: "User with ID 'xxx' not found.",
  //   status: 404
  // }
  
  console.error(`Error: ${error.title} - ${error.detail}`);
}
```

### Common Status Codes
| Code | Meaning | Action |
|------|---------|--------|
| 200 | Success | Process response data |
| 201 | Created | Resource created successfully |
| 204 | No Content | Success, empty response |
| 400 | Bad Request | Check validation errors in response |
| 401 | Unauthorized | Token expired/invalid - refresh or re-login |
| 403 | Forbidden | Insufficient permissions for this role |
| 404 | Not Found | Resource doesn't exist |
| 409 | Conflict | Duplicate resource or in-flight payment |

### Validation Error Response
```json
{
  "title": "Validation Failed",
  "detail": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "email": ["Email is required", "Invalid email format"],
    "password": ["Password must be at least 8 characters"]
  }
}
```

---

## Role-Based Access Control

### Check User Role (Frontend)
```typescript
const role = localStorage.getItem('role'); // "User" | "Coach" | "Admin"

if (role === 'Coach' || role === 'Admin') {
  // Show "Create Training Program" button
}

if (role === 'Admin') {
  // Show admin dashboard
}
```

### Endpoint Access by Role
| Endpoint | User | Coach | Admin |
|----------|------|-------|-------|
| GET /users/{id} | ✅ | ✅ | ✅ |
| PUT /users/{id} | ✅ | ❌ | ✅ |
| POST /training-programs | ❌ | ✅ | ✅ |
| POST /packages | ❌ | ❌ | ✅ |
| POST /payments | ✅ | ✅ | ✅ |

---

## Token Management

### Auto-Refresh on 401
```typescript
async function apiCall(endpoint, options = {}) {
  let response = await fetch(endpoint, {
    headers: {
      'Authorization': `Bearer ${localStorage.getItem('accessToken')}`
    },
    ...options
  });

  // If 401, try to refresh
  if (response.status === 401) {
    const refreshToken = localStorage.getItem('refreshToken');
    const refreshResponse = await fetch('https://api.changemind.com/api/auth/refresh', {
      method: 'POST',
      body: JSON.stringify({ refreshToken })
    });

    if (refreshResponse.ok) {
      const data = await refreshResponse.json();
      localStorage.setItem('accessToken', data.accessToken);

      // Retry original request
      response = await fetch(endpoint, {
        headers: {
          'Authorization': `Bearer ${data.accessToken}`
        },
        ...options
      });
    } else {
      // Refresh failed, logout
      localStorage.clear();
      window.location.href = '/login';
    }
  }

  return response;
}
```

### Logout
```typescript
function logout() {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('userId');
  localStorage.removeItem('role');
  window.location.href = '/login';
}
```

---

## Data Models Quick Reference

### UserDto
```typescript
interface UserDto {
  id: string;                    // UUID
  email: string;                 // user@example.com
  firstName: string;
  lastName: string;
  age?: number;                  // 13-120
  height?: number;               // cm
  weight?: number;               // kg
  gender?: 'Male' | 'Female' | 'Other';
  fitnessGoal?: string;          // "Weight Loss", "Muscle Gain", etc.
  fitnessLevel?: 'Beginner' | 'Intermediate' | 'Advanced';
  createdAt: string;             // ISO 8601 datetime
}
```

### PackageDto
```typescript
interface PackageDto {
  id: string;
  name: string;                  // "Premium", "Elite"
  description: string;
  price: number;                 // decimal (e.g., 99.99)
  durationDays: number;          // subscription length
  type: string;                  // "Monthly", "Yearly"
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
}
```

### ExerciseDto
```typescript
interface ExerciseDto {
  id: string;
  name: string;                  // "Bench Press"
  muscleGroup: string;           // "Chest", "Back", "Legs"
  difficultyLevel?: 'Beginner' | 'Intermediate' | 'Advanced';
  description?: string;
  videoUrl?: string;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
}
```

### ActiveProgramDetailDto
```typescript
interface ActiveProgramDetailDto {
  id: string;
  name: string;
  description?: string;
  durationWeeks: number;
  coachName: string;             // "John Doe"
  startDate?: string;
  endDate?: string;
  difficulty: 'Beginner' | 'Intermediate' | 'Advanced';
  status: 'InProgress' | 'Completed';
  dailyExercises?: {
    [dayKey: string]: Array<{    // "Day-1", "Day-2"
      exerciseId: string;
      sets: number;
      reps: string;              // "10-12", "AMRAP"
      explanation?: string;
    }>
  };
}
```

### PagedResult<T>
```typescript
interface PagedResult<T> {
  data: T[];
  total: number;                 // total items
  page: number;                  // current page (1-indexed)
  pageSize: number;              // items per page
  totalPages: number;            // calculated: ceil(total / pageSize)
}
```

---

## Example: Complete User Registration Flow

```typescript
// 1. Register
const registerResponse = await fetch('https://api.changemind.com/api/users', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    email: 'newuser@example.com',
    password: 'SecurePassword123!',
    firstName: 'John',
    lastName: 'Doe'
  })
});
const userId = await registerResponse.text(); // UUID

// 2. Login
const loginResponse = await fetch('https://api.changemind.com/api/auth/login', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    email: 'newuser@example.com',
    password: 'SecurePassword123!'
  })
});
const auth = await loginResponse.json();
localStorage.setItem('accessToken', auth.accessToken);
localStorage.setItem('refreshToken', auth.refreshToken);

// 3. Complete profile
const token = localStorage.getItem('accessToken');
await fetch(`https://api.changemind.com/api/users/${userId}/complete-profile`, {
  method: 'POST',
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  },
  body: JSON.stringify({
    firstName: 'John',
    lastName: 'Doe',
    age: 28,
    height: 180,
    weight: 75,
    gender: 'Male',
    fitnessGoal: 'Muscle Gain',
    fitnessLevel: 'Intermediate'
  })
});
// Returns 204 No Content

console.log('User registered and profile completed!');
```

---

## Testing with Postman/Insomnia

### 1. Import OpenAPI
File → Import → Select `openapi.json`

### 2. Login (Get Token)
```
POST http://localhost:5123/api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "Password123!"
}
```

### 3. Use Token in Subsequent Requests
- Copy `accessToken` from login response
- Add header: `Authorization: Bearer {accessToken}`

---

## Performance Tips

1. **Cache GET responses** (packages, exercises rarely change)
   ```typescript
   const getCachedPackages = () => {
     const cached = sessionStorage.getItem('packages');
     if (cached) return JSON.parse(cached);
   }
   ```

2. **Lazy load paginated data**
   ```typescript
   const [page, setPage] = useState(1);
   const { data } = await fetch(`/api/exercises?page=${page}&pageSize=20`);
   ```

3. **Use Axios/TanStack Query for caching & retry logic**
   ```bash
   npm install axios @tanstack/react-query
   ```

4. **Batch parallel requests**
   ```typescript
   const [users, coaches, packages] = await Promise.all([
     fetch('/api/users'),
     fetch('/api/coaches'),
     fetch('/api/packages')
   ]);
   ```

---

## OpenAPI Generator Usage

Generate type-safe client code:
```bash
# TypeScript Axios client
openapi-generator-cli generate \
  -i ./openapi.json \
  -g typescript-axios \
  -o ./src/generated/api-client \
  --package-name=changemind-api

# React Query hooks
openapi-generator-cli generate \
  -i ./openapi.json \
  -g typescript-react-query \
  -o ./src/generated/api-hooks
```

Then use generated client:
```typescript
import { UserApi } from './generated/api-client';

const api = new UserApi();
const user = await api.getUserById('550e8400-e29b-41d4-a716-446655440000');
```

---

## Links
- **OpenAPI Spec:** `./openapi.json` (import into Swagger UI / Postman)
- **Auth Details:** `./AUTH_GUIDE.md` (comprehensive JWT & RBAC guide)
- **API Base URL:**
  - Dev: `http://localhost:5123`
  - Prod: `https://api.changemind.com`

