# Parameter Alignment Task - Client Side to API/Database

## 1. Analysis Phase
- [x] Examine database schema to identify all tables and their column names/types
- [x] Analyze API endpoints and their DTOs (Data Transfer Objects)
- [x] Review client-side TypeScript types and interfaces
- [x] Identify all parameter mismatches between client, API, and database

## 2. Key Findings - Parameter Mismatches

### Employee Entity
- ✅ API expects: `email` and `phone` (optional nullable fields)
- ✅ Client sends: Missing `email` and `phone` in CreateEmployeeRequest type
- ✅ Schema: Missing validation for `email` and `phone`

### Guest Entity
- ❌ API expects: `gender` and `idProofType` as STRING ("Male"/"Female", "Passport"/"DriverLicense"/"NationalId")
- ❌ Client sends: `gender` and `idProofType` as NUMBER (0/1, 0/1/2)
- ❌ Major mismatch in data type representation

### Room Entity
- ✅ API expects: `status` as RoomStatus enum (0/1/2)
- ✅ Client sends: `status` as number - CORRECT
- ✅ All other fields match correctly

### Service Entity
- ✅ API expects: `branchId`, `name`, `price`, `description`
- ✅ Client sends: All fields match correctly
- ✅ No mismatches found

## 3. Fix Implementation
- [x] Fix Guest entity - Convert client to send string values for gender and idProofType
- [x] Fix Employee entity - Add email and phone fields to client types
- [x] Update Guest schemas to validate string values
- [x] Update Guest service calls to transform data correctly
- [x] Update Guest forms and components to handle string values
- [x] Update Employee forms and components to include email and phone fields

## 4. Verification
- [x] Verify all changes compile without errors
- [x] Create a final alignment report
- [x] Document all changes made