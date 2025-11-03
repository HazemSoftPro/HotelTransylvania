# Parameter and Data Type Mismatch Analysis

## Executive Summary
This document identifies all mismatches between database column names, API parameter names, and client-side parameter names in the HotelTransylvania project.

## Key Findings

### 1. Room Entity Mismatches

#### Database (PostgreSQL)
- Column names use **snake_case**:
  - `branch_id` (integer)
  - `room_type_id` (integer)
  - `room_number` (varchar(20))
  - `status` (string/enum)
  - `floor` (integer)
  - `manual_price` (numeric(10,2))

#### API Layer (C# DTOs)
- Property names use **PascalCase**:
  - `BranchId` (int)
  - `RoomTypeId` (int)
  - `RoomNumber` (string)
  - `Status` (RoomStatus enum)
  - `Floor` (int)
  - `ManualPrice` (decimal)

#### Client Layer (TypeScript)
- **Schema (roomSchema.ts)** uses **snake_case**:
  - `branch_id` (string)
  - `room_type_id` (string)
  - `room_number` (string)
  - `status` (string)
  - `floor` (number)
  - `manual_price` (number)

- **API Types (types/api/room.ts)** use **camelCase**:
  - `branchId` (number)
  - `roomTypeId` (number)
  - `roomNumber` (string)
  - `status` (RoomStatus)
  - `floor` (number)
  - `manualPrice` (number)

- **Form Values (types/room.ts)** use **camelCase**:
  - `branchId` (string)
  - `roomTypeId` (string)
  - `roomNumber` (string)
  - `status` (string)
  - `floor` (number)
  - `manualPrice` (number)

**MISMATCH**: The client schema uses snake_case while the API expects camelCase, requiring manual transformation in RoomForm.tsx

---

### 2. Guest Entity Mismatches

#### Database (PostgreSQL)
- Column names use **snake_case**:
  - `first_name` (varchar(50))
  - `last_name` (varchar(50))
  - `gender` (string/enum)
  - `id_proof_type` (string/enum)
  - `id_proof_number` (varchar(50))
  - `email` (varchar(100), nullable)
  - `phone` (varchar(20), nullable)
  - `address` (varchar(250), nullable)

#### API Layer (C# DTOs)
- Property names use **PascalCase**:
  - `FirstName` (string)
  - `LastName` (string)
  - `Gender` (Gender enum)
  - `IdProofType` (IdProofType enum)
  - `IdProofNumber` (string)
  - `Email` (string?)
  - `Phone` (string?)
  - `Address` (string?)

#### Client Layer (TypeScript)
- **Schema (guestSchema.ts)** uses **snake_case**:
  - `first_name` (string)
  - `last_name` (string)
  - `gender` (number)
  - `id_proof_type` (number)
  - `id_proof_number` (string)
  - `email` (string, optional)
  - `phone` (string, optional)
  - `address` (string, optional)

- **API Types (types/api/guest.ts)** use **camelCase**:
  - `firstName` (string)
  - `lastName` (string)
  - `gender` (Gender: 0 | 1)
  - `idProofType` (IdProofType: 0 | 1 | 2)
  - `idProofNumber` (string)
  - `email` (string, optional)
  - `phone` (string, optional)
  - `address` (string, optional)

- **GuestReq Interface** uses **camelCase** with string enums:
  - `firstName` (string)
  - `lastName` (string)
  - `gender` ('Male' | 'Female')
  - `idProofType` ('Passport' | 'DriverLicense' | 'NationalId')
  - `idProofNumber` (string)
  - `email` (string, optional)
  - `phone` (string, optional)
  - `address` (string, optional)

**MISMATCH**: The client schema uses snake_case while the API expects camelCase with string enum values, requiring manual transformation in GuestForm.tsx

---

### 3. Reservation Entity Mismatches

#### Database (PostgreSQL)
- Column names use **snake_case**:
  - `guest_id` (integer)
  - `check_in_date` (date)
  - `check_out_date` (date)
  - `reservation_date` (timestamp)
  - `status` (string/enum)
  - `total_cost` (numeric(10,2))

#### API Layer (C# DTOs)
- Property names use **PascalCase**:
  - `GuestId` (int)
  - `CheckInDate` (DateOnly)
  - `CheckOutDate` (DateOnly)
  - `ReservationDate` (DateTime)
  - `Status` (ReservationStatus enum)
  - `TotalCost` (decimal)

#### Client Layer (TypeScript)
- **Schema (reservationSchema.ts)** uses **camelCase**:
  - `guestId` (number)
  - `branchId` (number)
  - `checkInDate` (string)
  - `checkOutDate` (string)
  - `status` (enum string)
  - `roomIds` (number[])
  - `serviceIds` (number[])

- **API Types (types/api/reservation.ts)** use **camelCase**:
  - `guestId` (number)
  - `checkInDate` (string)
  - `checkOutDate` (string)
  - `rooms` (ReservationRoom[])
  - `services` (ReservationService[])
  - `status` (enum string, optional)

**MISMATCH**: The reservation schema uses `roomIds` and `serviceIds` arrays, but the API expects `rooms` and `services` arrays with detailed objects

---

## Critical Issues

### Issue 1: Inconsistent Naming Conventions
- **Database**: snake_case (PostgreSQL standard)
- **API**: PascalCase (C# standard)
- **Client Schemas**: snake_case (inconsistent with API)
- **Client API Types**: camelCase (TypeScript standard)

### Issue 2: Manual Transformations Required
The current implementation requires manual field name transformations in form components:
- RoomForm.tsx transforms snake_case to camelCase
- GuestForm.tsx transforms snake_case to camelCase and converts enum values

### Issue 3: Type Mismatches
- Room schema uses string types for IDs, requiring parseInt() conversions
- Guest schema uses numeric enums, but API expects string enum values
- Reservation schema structure doesn't match API expectations

### Issue 4: Enum Value Inconsistencies
- Guest Gender: Client uses 0/1, API expects 'Male'/'Female' strings
- Guest IdProofType: Client uses 0/1/2, API expects 'Passport'/'DriverLicense'/'NationalId' strings
- Room Status: Client uses string numbers, API expects numeric enum
- Reservation Status: Multiple status value variations

## Recommendations

### 1. Standardize Client-Side Naming
**Option A (Recommended)**: Align client schemas with API types
- Change all client schemas from snake_case to camelCase
- This matches TypeScript conventions and eliminates transformation code

**Option B**: Keep snake_case but create transformation utilities
- Create reusable transformation functions
- Maintain consistency but adds complexity

### 2. Fix Type Definitions
- Update room schema to use number types for IDs instead of strings
- Align enum definitions across client and API
- Ensure reservation schema matches API expectations

### 3. Consolidate Type Definitions
- Remove duplicate type definitions
- Create single source of truth for each entity
- Use TypeScript's utility types for variations (Omit, Pick, Partial)

### 4. Update Form Components
- Remove manual transformation logic
- Use consistent field names throughout
- Simplify form submission handlers

## Next Steps
1. Update client schemas to use camelCase
2. Fix type mismatches in schemas
3. Update form components to remove transformations
4. Test all CRUD operations
5. Document the standardized naming conventions