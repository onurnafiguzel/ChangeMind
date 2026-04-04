# Ödeme Sistemi İş Akışı Diyagramı

## 1. GENEL AKIŞ (High Level)

```
┌──────────────────┐
│   User buys      │
│   a Package      │
│  (Payment Flow)  │
└────────┬─────────┘
         │
         ▼
┌──────────────────────────────────┐
│  Payment Status: PENDING → OK    │
│  Charge: Successful (Mock)       │
└────────┬─────────────────────────┘
         │
         ├─────────────────────────────────────┐
         │                                     │
         ▼                                     ▼
    ┌─────────────┐             ┌────────────────────────┐
    │   Payments  │             │   WaitingUsers Table   │
    │   Table     │             │  (CREATE NEW RECORD)   │
    ├─────────────┤             ├────────────────────────┤
    │ id          │             │ id                     │
    │ user_id     │             │ user_id                │
    │ package_id  │             │ IsWaiting = TRUE ✓✓✓   │
    │ amount      │             │ created_at             │
    │ status ✓    │             └────────────────────────┘
    │ trans_id    │                      │
    │ created_at  │                      │
    └─────────────┘                      ▼
                          ┌──────────────────────────┐
                          │ Coach sees user in the   │
                          │ waiting list and can:    │
                          │ • View user profile      │
                          │ • Create training prog   │
                          └──────────────────────────┘
                                   │
                                   ▼
                          ┌──────────────────────────┐
                          │ Coach creates training   │
                          │ program for user        │
                          └────────┬─────────────────┘
                                   │
                                   ▼
                          ┌──────────────────────────┐
                          │ WaitingUser Record       │
                          │ IsWaiting = FALSE ✓✓✓    │
                          │ (User removed from list) │
                          └──────────────────────────┘
```

---

## 2. DETAYLI SIRASAL AKIŞ (Sequence Diagram)

```
User                API                 Handler              Database
 │                   │                     │                    │
 │  POST /payments/process                 │                    │
 ├──────────────────────────────────────>  │                    │
 │                   │                     │                    │
 │                   │  Verify User        │                    │
 │                   │────────────────────>│                    │
 │                   │  ✓ Exists           │                    │
 │                   │<────────────────────┤                    │
 │                   │                     │                    │
 │                   │  Verify Package     │                    │
 │                   │────────────────────>│                    │
 │                   │  ✓ Exists           │                    │
 │                   │<────────────────────┤                    │
 │                   │                     │                    │
 │                   │  Mock Payment Process                    │
 │                   │  (Always Succeeds)  │                    │
 │                   │<────────────────────┤                    │
 │                   │                     │                    │
 │                   │  Save Payment       │                    │
 │                   │  Status: COMPLETED  │                    │
 │                   │────────────────────────────────────────>│
 │                   │                     │                    │ INSERT
 │                   │                     │                    │ Payments
 │                   │                     │                    │
 │                   │  Create/Update WaitingUser              │
 │                   │  IsWaiting = true   │                    │
 │                   │────────────────────────────────────────>│
 │                   │                     │                    │ INSERT/UPDATE
 │                   │                     │                    │ WaitingUsers
 │                   │                     │                    │
 │                   │  Save Transaction   │                    │
 │                   │  (UnitOfWork)       │────────────────────────────>
 │                   │                     │                    │ COMMIT
 │                   │                     │                    │
 │  Success Response │                     │                    │
 │<──────────────────┤                     │                    │
 │  paymentId        │                     │                    │
 │  isSuccess: true  │                     │                    │
```

---

## 3. COACH GÖRÜŞ - WAITING USERS LISTESI

```
Coach                API              Handler            Database
 │                   │                   │                  │
 │  GET /payments/waiting-users          │                  │
 ├──────────────────────────────────────>│                  │
 │                   │                   │                  │
 │                   │  [Authorize(Coach)]                  │
 │                   │  ✓ Token válido   │                  │
 │                   │<──────────────────┤                  │
 │                   │                   │                  │
 │                   │  Query WaitingUsers Table            │
 │                   │  WHERE IsWaiting = TRUE              │
 │                   │────────────────────────────────────>│
 │                   │                   │                  │ SELECT
 │                   │                   │  Results:        │
 │                   │                   │  ┌────────────┐  │
 │                   │                   │  │ User 1     │  │
 │                   │                   │  │ Email: ... │  │
 │                   │                   │  │ Profile... │  │
 │                   │                   │  ├────────────┤  │
 │                   │                   │  │ User 2     │  │
 │                   │                   │  │ Email: ... │  │
 │                   │                   │  │ Profile... │  │
 │                   │                   │  └────────────┘  │
 │                   │                   │<──────────────────
 │                   │                   │                  │
 │  List of Waiting Users                │                  │
 │<──────────────────────────────────────┤                  │
 │  [Coach sees available users]         │                  │
```

---

## 4. COACH - TRAINING PROGRAM ATAMASI

```
Coach                API                    Handler              Database
 │                   │                        │                    │
 │  POST /training-programs                   │                    │
 │  (coachId, userId, name, duration, ...)    │                    │
 ├───────────────────────────────────────────>│                    │
 │                   │                        │                    │
 │                   │  [Authorize(Coach)]    │                    │
 │                   │  ✓ Token valid         │                    │
 │                   │<───────────────────────┤                    │
 │                   │                        │                    │
 │                   │  Verify Coach          │                    │
 │                   │────────────────────────────────────────────>│
 │                   │                        │  Exists? ✓         │
 │                   │                        │<──────────────────
 │                   │                        │                    │
 │                   │  Verify User           │                    │
 │                   │────────────────────────────────────────────>│
 │                   │                        │  Exists? ✓         │
 │                   │                        │<──────────────────
 │                   │                        │                    │
 │                   │  Create TrainingProgram                     │
 │                   │  id: guid              │                    │
 │                   │  userId: X             │                    │
 │                   │  coachId: Y            │                    │
 │                   │  startDate: now        │                    │
 │                   │────────────────────────────────────────────>│
 │                   │                        │                    │ INSERT
 │                   │                        │                    │ TrainingPrograms
 │                   │                        │                    │
 │                   │  Get WaitingUser       │                    │
 │                   │  by userId             │                    │
 │                   │────────────────────────────────────────────>│
 │                   │                        │  Found ✓           │
 │                   │                        │<──────────────────
 │                   │                        │                    │
 │                   │  Mark as Assigned      │                    │
 │                   │  IsWaiting = FALSE     │                    │
 │                   │────────────────────────────────────────────>│
 │                   │                        │                    │ UPDATE
 │                   │                        │                    │ WaitingUsers
 │                   │                        │                    │
 │                   │  Save Transaction      │                    │
 │                   │────────────────────────────────────────────>│
 │                   │                        │                    │ COMMIT
 │                   │                        │                    │
 │  Success Response │                        │                    │
 │<───────────────────────────────────────────┤                    │
 │  programId        │                        │                    │
```

---

## 5. DURUM GEÇİŞLERİ (State Transitions)

```
┌─────────────────────────────────────────────────────────────────┐
│                  WaitingUser Lifecycle                          │
└─────────────────────────────────────────────────────────────────┘

    ┌──────────────────────────────────┐
    │  PAYMENT SUCCESSFUL              │
    │  (ProcessPaymentCommand)         │
    └─────────────┬────────────────────┘
                  │
                  ▼
    ┌──────────────────────────────────┐
    │  CREATE: WaitingUser             │
    │  IsWaitingForAssignment = TRUE   │
    └─────────────┬────────────────────┘
                  │
        ┌─────────┴──────────┐
        │                    │
    [YES] Coach assigns?  [NO] Still waiting
        │                    │
        ▼                    ▼
    ┌──────────────────────────────────┐
    │  CREATE: TrainingProgram         │
    │  (CoachId, UserId, ...)          │
    └─────────────┬────────────────────┘
                  │
                  ▼
    ┌──────────────────────────────────┐
    │  UPDATE: WaitingUser             │
    │  IsWaitingForAssignment = FALSE  │
    └─────────────┬────────────────────┘
                  │
                  ▼
    ┌──────────────────────────────────┐
    │  User removed from "Waiting"     │
    │  (Coach won't see in list)       │
    └──────────────────────────────────┘


Optional: Program İptal Edilirse
    │
    ▼
    ┌──────────────────────────────────┐
    │  UPDATE: TrainingProgram         │
    │  IsActive = FALSE                │
    └─────────────┬────────────────────┘
                  │
                  ▼
    ┌──────────────────────────────────┐
    │  UPDATE: WaitingUser             │
    │  IsWaitingForAssignment = TRUE   │
    └─────────────┬────────────────────┘
                  │
                  ▼
    ┌──────────────────────────────────┐
    │  User geri görünür "Waiting"     │
    │  (Coach tekrar atama yapabilir)  │
    └──────────────────────────────────┘
```

---

## 6. DATABASE VIEW

```
┌──────────────────────────────────────────────────────────────┐
│                     PAYMENTS TABLE                           │
├─────────┬──────────┬────────────┬──────────┬────────────────┤
│ ID      │ USER_ID  │ PACKAGE_ID │ AMOUNT   │ STATUS         │
├─────────┼──────────┼────────────┼──────────┼────────────────┤
│ pay-001 │ user-001 │ pkg-basic  │ 29.99    │ COMPLETED  ✓   │
│ pay-002 │ user-002 │ pkg-std    │ 59.99    │ COMPLETED  ✓   │
│ pay-003 │ user-003 │ pkg-prem   │ 99.99    │ COMPLETED  ✓   │
└─────────┴──────────┴────────────┴──────────┴────────────────┘

┌──────────────────────────────────────────────────────────────┐
│                  WAITINGUSERS TABLE                          │
├─────────┬──────────┬──────────────────────┬─────────────────┤
│ ID      │ USER_ID  │ IsWaitingForAssign   │ CREATED_AT      │
├─────────┼──────────┼──────────────────────┼─────────────────┤
│ wait-01 │ user-001 │ true  ← In List ✓    │ 2026-04-02      │
│ wait-02 │ user-002 │ true  ← In List ✓    │ 2026-04-02      │
│ wait-03 │ user-003 │ false ← Already      │ 2026-04-02      │
│         │          │        assigned      │                 │
└─────────┴──────────┴──────────────────────┴─────────────────┘

┌──────────────────────────────────────────────────────────────┐
│              TRAININGPROGRAMS TABLE                          │
├─────────┬──────────┬──────────┬────────────────┬─────────────┤
│ ID      │ USER_ID  │ COACH_ID │ NAME           │ START_DATE  │
├─────────┼──────────┼──────────┼────────────────┼─────────────┤
│ prog-01 │ user-001 │ coach-A  │ 12 Week Str... │ 2026-04-02  │
│ prog-02 │ user-003 │ coach-B  │ 8 Week Cardio  │ 2026-04-02  │
│ prog-03 │ user-002 │ coach-A  │ 16 Week Power  │ 2026-04-02  │
└─────────┴──────────┴──────────┴────────────────┴─────────────┘
```

---

## 7. ÖRNEK SENARYO (WALKTHROUGH)

```
ZAMAN        KULLANICI         SISTEM                    TABLO DURUMLARI
─────────────────────────────────────────────────────────────────────────

09:00   User-A Paket Satın       Process Payment
        Alır (29.99$)            │
        ├──POST /payments/       │
        │  process               │
        │                        │
        │                    ├──> Payments: INSERT
        │                    │    pay-001, user-A, completed
        │                    │
        │                    ├──> WaitingUsers: INSERT
        │                    │    wait-01, user-A, TRUE ✓✓✓
        │                    │
        └────────────────────<── Success (paymentId)

        ✓ User-A appears in
          waiting list

─────────────────────────────────────────────────────────────────────────

09:15   Coach logs in        Query WaitingUsers
        │                    WHERE IsWaiting = TRUE
        │
        ├──GET /payments/
        │  waiting-users
        │
        └────────────────────<── Results:
                              [User-A, User-B, User-C]
                              (All with IsWaiting=TRUE)

        ✓ Coach sees 3 users
          ready for assignment

─────────────────────────────────────────────────────────────────────────

09:30   Coach creates       Create TrainingProgram
        program for User-A   │
        │                   │
        ├──POST /training-   ├──> TrainingPrograms: INSERT
        │  programs          │    prog-001, user-A, coach-X
        │                   │
        │                   ├──> WaitingUsers: UPDATE
        │                   │    wait-01, user-A, FALSE
        │                   │    (Assigned!)
        └───────────────────<── Success (programId)

        ✓ User-A marked as
          ASSIGNED (removed
          from waiting list)

─────────────────────────────────────────────────────────────────────────

09:35   Coach checks again  Query WaitingUsers
        │                   WHERE IsWaiting = TRUE
        │
        ├──GET /payments/
        │  waiting-users
        │
        └────────────────────<── Results:
                              [User-B, User-C]
                              (User-A gone!)

        ✓ User-A no longer
          in list (assigned)
          User-B, User-C
          still waiting

─────────────────────────────────────────────────────────────────────────
```

---

## 8. ENDPOINTS ÖZETI

```
┌─────────────────────────────────────────────────────────────┐
│                 API ENDPOINTS                               │
├──────────────┬─────────────────────────┬────────────────────┤
│ METHOD       │ ENDPOINT                │ YAPTIĞı İŞLEM      │
├──────────────┼─────────────────────────┼────────────────────┤
│ POST         │ /api/payments/process   │ • Payment oluştur  │
│              │                         │ • WaitingUser ekle │
│              │                         │   (IsWait=TRUE)    │
├──────────────┼─────────────────────────┼────────────────────┤
│ GET          │ /api/payments/          │ • List WaitingUsers│
│              │ waiting-users           │   IsWait=TRUE      │
│              │                         │ • User Profile +   │
│              │                         │   Payment Count    │
├──────────────┼─────────────────────────┼────────────────────┤
│ POST         │ /api/training-programs  │ • Program oluştur  │
│              │                         │ • Mark user as     │
│              │                         │   assigned         │
│              │                         │   (IsWait=FALSE)   │
└──────────────┴─────────────────────────┴────────────────────┘
```

---

## 9. QUERY DETAYLARI

```
PAYMENT PROCESS HANDLERİ:
┌────────────────────────────────────────────────────────────┐
│ 1. Check user exists                                       │
│ 2. Check package exists                                   │
│ 3. Create Payment(userId, packageId, amount)              │
│ 4. Mock Process: Mark as COMPLETED                        │
│ 5. Add to DB                                              │
│ 6. Check WaitingUser exists for userId                    │
│    └─ If NOT exists: CREATE with IsWait=TRUE             │
│    └─ If exists + IsWait=FALSE: UPDATE to TRUE           │
│    └─ If exists + IsWait=TRUE: No change                 │
│ 7. SaveChangesAsync (UnitOfWork)                          │
└────────────────────────────────────────────────────────────┘

WAITING USERS HANDLER:
┌────────────────────────────────────────────────────────────┐
│ 1. Query: WaitingUsers WHERE IsWaitingForAssignment=TRUE   │
│ 2. Include related User data                              │
│ 3. For each user:                                         │
│    ├─ Get user profile (email, name, age, fitness, etc)  │
│    ├─ Count completed payments                            │
│    └─ Build UserAssignmentDto                            │
│ 4. Order by CreatedAt DESC                                │
│ 5. Return List<UserAssignmentDto>                         │
└────────────────────────────────────────────────────────────┘

CREATE TRAINING PROGRAM HANDLER:
┌────────────────────────────────────────────────────────────┐
│ 1. Verify coach exists                                    │
│ 2. Verify user exists                                    │
│ 3. Create TrainingProgram                                │
│    ├─ SetUserId = userId                                 │
│    ├─ SetCoachId = coachId                               │
│    ├─ SetStartDate = now (or provided)                   │
│    ├─ SetEndDate = startDate + duration*7 days           │
│    └─ SetIsActive = true                                 │
│ 4. Get WaitingUser by userId                             │
│ 5. If found + IsWaitingForAssignment=true:               │
│    └─ Update: MarkAsAssigned() → IsWait = FALSE          │
│ 6. SaveChangesAsync (UnitOfWork)                         │
└────────────────────────────────────────────────────────────┘
```
