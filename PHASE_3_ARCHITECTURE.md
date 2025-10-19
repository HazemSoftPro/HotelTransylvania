# Phase 3 Architecture Overview

## System Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           InnHotel Phase 3 Architecture                      │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                              Frontend Layer                                  │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌──────────────────┐         ┌──────────────────┐                          │
│  │  CheckInWizard   │         │ CheckOutWizard   │                          │
│  │                  │         │                  │                          │
│  │  1. Verify Guest │         │  1. Inspect Room │                          │
│  │  2. Assign Room  │         │  2. Add Charges  │                          │
│  │  3. Verify Pay   │         │  3. Settle Pay   │                          │
│  │  4. Requests     │         │  4. Feedback     │                          │
│  │  5. Complete     │         │  5. Complete     │                          │
│  └────────┬─────────┘         └────────┬─────────┘                          │
│           │                            │                                     │
│           └────────────┬───────────────┘                                     │
│                        │                                                     │
│                        ▼                                                     │
│           ┌────────────────────────┐                                        │
│           │  reservationService    │                                        │
│           │  - updateStatus()      │                                        │
│           │  - checkAvailability() │                                        │
│           └────────────┬───────────┘                                        │
│                        │                                                     │
└────────────────────────┼─────────────────────────────────────────────────────┘
                         │
                         │ HTTP/REST
                         │
┌────────────────────────▼─────────────────────────────────────────────────────┐
│                              API Layer (FastEndpoints)                        │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  PUT /reservations/{id}/status                                       │   │
│  │  GET /reservations/check-availability                                │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                        │                                                     │
│                        ▼                                                     │
└────────────────────────┼─────────────────────────────────────────────────────┘
                         │
                         │ MediatR
                         │
┌────────────────────────▼─────────────────────────────────────────────────────┐
│                           Use Cases Layer                                     │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌──────────────────────────────┐    ┌──────────────────────────────┐      │
│  │ UpdateReservationStatus      │    │ CheckRoomAvailability        │      │
│  │ Handler                      │    │ Handler                      │      │
│  │                              │    │                              │      │
│  │ - Validate status transition │    │ - Query reservations         │      │
│  │ - Update reservation         │    │ - Check date overlap         │      │
│  │ - Sync room status           │    │ - Return availability        │      │
│  │ - Log audit entry            │    │                              │      │
│  └──────────────┬───────────────┘    └──────────────┬───────────────┘      │
│                 │                                    │                       │
│                 └────────────┬───────────────────────┘                       │
│                              │                                               │
└──────────────────────────────┼───────────────────────────────────────────────┘
                               │
                               │
┌──────────────────────────────▼───────────────────────────────────────────────┐
│                           Core Domain Layer                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                    Business Services                                 │   │
│  ├─────────────────────────────────────────────────────────────────────┤   │
│  │                                                                       │   │
│  │  ┌──────────────────────────────────────────────────────────────┐  │   │
│  │  │  ReservationStatusTransitionService                           │  │   │
│  │  │  ┌────────────────────────────────────────────────────────┐  │  │   │
│  │  │  │  IsValidTransition(current, new)                       │  │  │   │
│  │  │  │  - Pending → Confirmed ✓                               │  │  │   │
│  │  │  │  - Confirmed → CheckedIn ✓                             │  │  │   │
│  │  │  │  - CheckedIn → CheckedOut ✓                            │  │  │   │
│  │  │  │  - CheckedOut → * ✗ (Terminal)                         │  │  │   │
│  │  │  └────────────────────────────────────────────────────────┘  │  │   │
│  │  │  ┌────────────────────────────────────────────────────────┐  │  │   │
│  │  │  │  ValidateBusinessRules(reservation, newStatus)         │  │  │   │
│  │  │  │  - Check-in date validation                            │  │  │   │
│  │  │  │  - Check-out date validation                           │  │  │   │
│  │  │  │  - Reservation date validation                         │  │  │   │
│  │  │  └────────────────────────────────────────────────────────┘  │  │   │
│  │  └──────────────────────────────────────────────────────────────┘  │   │
│  │                                                                       │   │
│  │  ┌──────────────────────────────────────────────────────────────┐  │   │
│  │  │  RoomStatusSyncService                                        │  │   │
│  │  │  ┌────────────────────────────────────────────────────────┐  │  │   │
│  │  │  │  SyncRoomStatus(reservation, rooms, newStatus)         │  │  │   │
│  │  │  │  - CheckedIn → Room.Status = Occupied                  │  │  │   │
│  │  │  │  - CheckedOut → Room.Status = Available                │  │  │   │
│  │  │  │  - Cancelled → Room.Status = Available                 │  │  │   │
│  │  │  └────────────────────────────────────────────────────────┘  │  │   │
│  │  └──────────────────────────────────────────────────────────────┘  │   │
│  │                                                                       │   │
│  │  ┌──────────────────────────────────────────────────────────────┐  │   │
│  │  │  RoomAvailabilityService                                      │  │   │
│  │  │  ┌────────────────────────────────────────────────────────┐  │  │   │
│  │  │  │  IsRoomAvailable(roomId, checkIn, checkOut)            │  │  │   │
│  │  │  │  - Filter active reservations                          │  │  │   │
│  │  │  │  - Check date overlap                                  │  │  │   │
│  │  │  │  - Return availability status                          │  │  │   │
│  │  │  └────────────────────────────────────────────────────────┘  │  │   │
│  │  │  ┌────────────────────────────────────────────────────────┐  │  │   │
│  │  │  │  DatesOverlap(start1, end1, start2, end2)              │  │  │   │
│  │  │  │  - Algorithm: start1 < end2 AND end1 > start2          │  │  │   │
│  │  │  └────────────────────────────────────────────────────────┘  │  │   │
│  │  │  ┌────────────────────────────────────────────────────────┐  │  │   │
│  │  │  │  CheckBulkAvailability(roomIds, dates)                 │  │  │   │
│  │  │  │  - Check multiple rooms efficiently                    │  │  │   │
│  │  │  └────────────────────────────────────────────────────────┘  │  │   │
│  │  └──────────────────────────────────────────────────────────────┘  │   │
│  │                                                                       │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                         Entities                                     │   │
│  ├─────────────────────────────────────────────────────────────────────┤   │
│  │                                                                       │   │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐              │   │
│  │  │ Reservation  │  │    Room      │  │   Waitlist   │              │   │
│  │  │              │  │              │  │              │              │   │
│  │  │ - Status     │  │ - Status     │  │ - GuestId    │              │   │
│  │  │ - CheckIn    │  │ - RoomType   │  │ - RoomType   │              │   │
│  │  │ - CheckOut   │  │ - Branch     │  │ - Dates      │              │   │
│  │  │ - Rooms      │  │              │  │ - Priority   │              │   │
│  │  └──────────────┘  └──────────────┘  └──────────────┘              │   │
│  │                                                                       │   │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐              │   │
│  │  │  Permission  │  │ RolePermission│  │  AuditLog    │              │   │
│  │  │              │  │              │  │              │              │   │
│  │  │ - Name       │  │ - RoleName   │  │ - UserId     │              │   │
│  │  │ - Category   │  │ - Permission │  │ - Action     │              │   │
│  │  │ - Desc       │  │              │  │ - Entity     │              │   │
│  │  └──────────────┘  └──────────────┘  │ - Timestamp  │              │   │
│  │                                       └──────────────┘              │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                               │
└───────────────────────────────────────────────────────────────────────────────┘
                               │
                               │
┌──────────────────────────────▼───────────────────────────────────────────────┐
│                        Infrastructure Layer                                   │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  AuditLogService                                                     │   │
│  │  - LogAsync(userId, action, entity, ...)                            │   │
│  │  - GetLogsAsync(filters, pagination)                                │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  Repository<T> (Generic)                                             │   │
│  │  - AddAsync, UpdateAsync, DeleteAsync                               │   │
│  │  - ListAsync, FirstOrDefaultAsync                                   │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                               │
└───────────────────────────────────────────────────────────────────────────────┘
                               │
                               │
┌──────────────────────────────▼───────────────────────────────────────────────┐
│                          Database Layer                                       │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │  PostgreSQL Database                                                 │   │
│  │                                                                       │   │
│  │  Tables:                                                             │   │
│  │  - Reservations (existing)                                           │   │
│  │  - Rooms (existing)                                                  │   │
│  │  - Permissions (NEW)                                                 │   │
│  │  - RolePermissions (NEW)                                             │   │
│  │  - AuditLogs (NEW)                                                   │   │
│  │  - Waitlists (NEW)                                                   │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
│                                                                               │
└───────────────────────────────────────────────────────────────────────────────┘
```

## Data Flow Diagrams

### 1. Check-In Workflow

```
┌─────────┐
│  User   │
└────┬────┘
     │
     │ 1. Opens Check-In Wizard
     ▼
┌─────────────────┐
│ CheckInWizard   │
└────┬────────────┘
     │
     │ 2. Step 1: Verify Guest
     │    - Enter ID Number
     │    - Validate Guest Info
     │
     │ 3. Step 2: Confirm Rooms
     │    - Display Assigned Rooms
     │    - Verify Room Details
     │
     │ 4. Step 3: Verify Payment
     │    - Confirm Payment Received
     │    - Check Total Cost
     │
     │ 5. Step 4: Special Requests
     │    - Collect Guest Requests
     │    - Note Any Requirements
     │
     │ 6. Step 5: Complete
     │    - Review Summary
     │    - Click "Complete Check-In"
     │
     ▼
┌─────────────────────────┐
│ reservationService      │
│ .updateStatus(id,       │
│  "CheckedIn")           │
└────┬────────────────────┘
     │
     │ HTTP PUT /reservations/{id}/status
     ▼
┌─────────────────────────┐
│ UpdateStatus Endpoint   │
└────┬────────────────────┘
     │
     │ MediatR Send
     ▼
┌──────────────────────────────┐
│ UpdateReservationStatus      │
│ Handler                      │
└────┬─────────────────────────┘
     │
     │ 1. Get Reservation
     │ 2. Validate Transition
     ▼
┌──────────────────────────────┐
│ ReservationStatusTransition  │
│ Service                      │
│ .TransitionStatus()          │
└────┬─────────────────────────┘
     │
     │ Validation Passed
     ▼
┌──────────────────────────────┐
│ RoomStatusSyncService        │
│ .SyncRoomStatus()            │
└────┬─────────────────────────┘
     │
     │ Update Rooms to "Occupied"
     ▼
┌──────────────────────────────┐
│ Database                     │
│ - Update Reservation.Status  │
│ - Update Room.Status         │
│ - Insert AuditLog            │
└──────────────────────────────┘
```

### 2. Availability Check Workflow

```
┌─────────┐
│  User   │
└────┬────┘
     │
     │ Selects Dates & Room
     ▼
┌─────────────────────────┐
│ reservationService      │
│ .checkAvailability()    │
└────┬────────────────────┘
     │
     │ HTTP GET /reservations/check-availability
     ▼
┌─────────────────────────┐
│ CheckAvailability       │
│ Endpoint                │
└────┬────────────────────┘
     │
     │ MediatR Send
     ▼
┌──────────────────────────────┐
│ CheckRoomAvailability        │
│ Handler                      │
└────┬─────────────────────────┘
     │
     │ 1. Get All Reservations
     ▼
┌──────────────────────────────┐
│ RoomAvailabilityService      │
│ .IsRoomAvailable()           │
└────┬─────────────────────────┘
     │
     │ 2. Filter Active Reservations
     │ 3. Check Date Overlap
     │ 4. Return Result
     ▼
┌──────────────────────────────┐
│ Response                     │
│ {                            │
│   "isAvailable": true/false  │
│ }                            │
└──────────────────────────────┘
```

### 3. Audit Logging Flow

```
┌─────────────────┐
│ Any Sensitive   │
│ Operation       │
└────┬────────────┘
     │
     │ (e.g., Status Change, Delete, etc.)
     ▼
┌──────────────────────────────┐
│ Handler/Service              │
└────┬─────────────────────────┘
     │
     │ Call AuditLogService
     ▼
┌──────────────────────────────┐
│ AuditLogService              │
│ .LogAsync()                  │
└────┬─────────────────────────┘
     │
     │ Create AuditLog Entry
     ▼
┌──────────────────────────────┐
│ AuditLog Entity              │
│ - UserId                     │
│ - UserName                   │
│ - Action                     │
│ - EntityType                 │
│ - EntityId                   │
│ - OldValues                  │
│ - NewValues                  │
│ - Timestamp                  │
│ - IpAddress                  │
└────┬─────────────────────────┘
     │
     │ Save to Database
     ▼
┌──────────────────────────────┐
│ AuditLogs Table              │
└──────────────────────────────┘
```

## Permission System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                      Permission System                           │
└─────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────┐
│                         Roles                                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  Administrator    Manager    Receptionist    Housekeeper    Accountant
│       │              │             │              │              │
│       │              │             │              │              │
│       └──────────────┴─────────────┴──────────────┴──────────────┘
│                              │
│                              │ Has Many
│                              ▼
├─────────────────────────────────────────────────────────────────┤
│                    RolePermissions                               │
├─────────────────────────────────────────────────────────────────┤
│                              │
│                              │ Maps To
│                              ▼
├─────────────────────────────────────────────────────────────────┤
│                      Permissions                                 │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────┐ │
│  │  Reservations    │  │     Rooms        │  │    Guests    │ │
│  │  - View          │  │  - View          │  │  - View      │ │
│  │  - Create        │  │  - Create        │  │  - Create    │ │
│  │  - Update        │  │  - Update        │  │  - Update    │ │
│  │  - Delete        │  │  - Delete        │  │  - Delete    │ │
│  │  - CheckIn       │  │  - UpdateStatus  │  │              │ │
│  │  - CheckOut      │  │                  │  │              │ │
│  └──────────────────┘  └──────────────────┘  └──────────────┘ │
│                                                                   │
│  ┌──────────────────┐  ┌──────────────────┐  ┌──────────────┐ │
│  │   Employees      │  │    Financial     │  │    System    │ │
│  │  - View          │  │  - ViewReports   │  │  - ViewLogs  │ │
│  │  - Create        │  │  - ProcessPay    │  │  - Settings  │ │
│  │  - Update        │  │  - ViewRevenue   │  │  - Branches  │ │
│  │  - Delete        │  │                  │  │              │ │
│  │  - ManageRoles   │  │                  │  │              │ │
│  └──────────────────┘  └──────────────────┘  └──────────────┘ │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘
```

## Technology Stack

```
┌─────────────────────────────────────────────────────────────────┐
│                      Technology Stack                            │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  Frontend:                                                       │
│  - React 19                                                      │
│  - TypeScript (Strict Mode)                                     │
│  - Tailwind CSS                                                  │
│  - shadcn/ui Components                                          │
│  - Zustand (State Management)                                    │
│  - Axios (HTTP Client)                                           │
│                                                                   │
│  Backend:                                                        │
│  - .NET 9                                                        │
│  - C# 12                                                         │
│  - FastEndpoints                                                 │
│  - MediatR (CQRS)                                                │
│  - Entity Framework Core                                         │
│  - Ardalis.GuardClauses                                          │
│  - Ardalis.Result                                                │
│                                                                   │
│  Database:                                                       │
│  - PostgreSQL                                                    │
│                                                                   │
│  Architecture:                                                   │
│  - Clean Architecture                                            │
│  - CQRS Pattern                                                  │
│  - Repository Pattern                                            │
│  - Domain-Driven Design                                          │
│                                                                   │
└─────────────────────────────────────────────────────────────────┘
```

---

**Document Version:** 1.0  
**Created:** October 19, 2025  
**Purpose:** Visual architecture documentation for Phase 3