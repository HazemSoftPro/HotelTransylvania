# InnHotel System - Comprehensive QA Testing Guide
## Quality Assurance Testing Manual for API and Desktop Client

---

## Table of Contents

1. [Introduction](#1-introduction)
2. [System Requirements & Prerequisites](#2-system-requirements--prerequisites)
3. [Testing Environment Setup](#3-testing-environment-setup)
4. [API Testing](#4-api-testing)
5. [Client Testing](#5-client-testing)
6. [Integration Testing](#6-integration-testing)
7. [Error Handling & Logging](#7-error-handling--logging)
8. [Test Case Scenarios](#8-test-case-scenarios)
9. [Troubleshooting Guide](#9-troubleshooting-guide)
10. [Defect Reporting & Resolution](#10-defect-reporting--resolution)
11. [Testing Completion Criteria](#11-testing-completion-criteria)
12. [Appendices](#12-appendices)

---

## 1. Introduction

### 1.1 Purpose
This guide provides comprehensive instructions for Quality Assurance (QA) testers to thoroughly test the InnHotel system, which consists of:
- **InnHotel API**: A .NET 9 RESTful API backend
- **InnHotel Desktop Client**: An Electron-based React desktop application
- **PostgreSQL Database**: Data persistence layer

### 1.2 Scope
This guide covers:
- System setup and configuration
- API endpoint testing (authentication, CRUD operations, search)
- Client UI and functionality testing
- Integration testing between components
- Error handling and logging verification
- Performance and security testing basics
- Defect identification and reporting

### 1.3 Testing Objectives
- Verify all API endpoints function correctly
- Validate client-server communication
- Ensure proper error handling and user feedback
- Verify authentication and authorization mechanisms
- Test data integrity and validation
- Confirm system stability under various conditions

---

## 2. System Requirements & Prerequisites

### 2.1 Hardware Requirements

| Component | Minimum | Recommended |
|-----------|---------|-------------|
| CPU | Dual-core 2.0 GHz | Quad-core 2.5 GHz+ |
| RAM | 8 GB | 16 GB |
| Storage | 10 GB free space | 20 GB free space |
| Network | Stable internet connection | High-speed broadband |

### 2.2 Software Prerequisites

#### 2.2.1 Required Software

**Operating System:**
- Windows 10/11 (64-bit)
- macOS 12+ (Monterey or later)
- Linux (Ubuntu 20.04+ or equivalent)

**Development Tools:**
```bash
# .NET 9 SDK
Version: 9.0 or later
Download: https://dotnet.microsoft.com/download/dotnet/9.0

# Node.js
Version: 20.x LTS
Download: https://nodejs.org/

# PostgreSQL
Version: 15.x or later
Download: https://www.postgresql.org/download/

# Git
Version: 2.x or later
Download: https://git-scm.com/downloads
```

#### 2.2.2 Testing Tools

**API Testing:**
- Postman (v10.x or later) - https://www.postman.com/downloads/
- cURL (command-line tool)
- REST Client (VS Code extension) - Optional

**Browser Testing:**
- Chrome/Chromium (latest version)
- Firefox (latest version)

**Database Tools:**
- pgAdmin 4 - https://www.pgadmin.org/download/
- DBeaver (optional) - https://dbeaver.io/download/

**Additional Tools:**
- Visual Studio Code - https://code.visualstudio.com/
- Git Bash (Windows) - Included with Git installation

### 2.3 Installation Verification

Run these commands to verify installations:

```bash
# Check .NET SDK
dotnet --version
# Expected output: 9.0.x or higher

# Check Node.js and npm
node --version
npm --version
# Expected: v20.x.x and 10.x.x

# Check PostgreSQL
psql --version
# Expected: psql (PostgreSQL) 15.x

# Check Git
git --version
# Expected: git version 2.x.x
```

### 2.4 Prerequisites Checklist

Before starting testing, ensure:

- [ ] All required software is installed and verified
- [ ] You have administrator/sudo privileges
- [ ] Firewall allows connections on ports: 5432 (PostgreSQL), 57679 (API), 5173 (Client)
- [ ] You have access to the GitHub repository
- [ ] You have a text editor or IDE installed
- [ ] You have at least 10 GB free disk space
- [ ] Your system meets minimum hardware requirements

---

## 3. Testing Environment Setup

### 3.1 Repository Setup

#### 3.1.1 Clone the Repository

```bash
# Clone the repository
git clone https://github.com/HazemSoftPro/HotelTransylvania.git

# Navigate to the project directory
cd HotelTransylvania

# Verify repository structure
ls -la
```

**Expected Directory Structure:**
```
HotelTransylvania/
├── innhotel-api/              # .NET API Backend
├── innhotel-desktop-client/   # Electron/React Client
├── docker-compose.yml         # Docker configuration
├── database-setup.sql         # Database initialization
├── README.md                  # Project documentation
└── troubleshooting.md         # Troubleshooting guide
```

### 3.2 Database Setup

#### 3.2.1 Start PostgreSQL Service

**Linux/macOS:**
```bash
# Start PostgreSQL
sudo systemctl start postgresql

# Enable auto-start on boot
sudo systemctl enable postgresql

# Check status
sudo systemctl status postgresql
```

**Windows:**
```powershell
# Start PostgreSQL service
net start postgresql-x64-15

# Or use Services app (services.msc)
```

#### 3.2.2 Create Database and User

```bash
# Connect to PostgreSQL as superuser
sudo -u postgres psql

# Or on Windows (using psql from command prompt)
psql -U postgres
```

**Execute these SQL commands:**
```sql
-- Create database
CREATE DATABASE innhotel_db;

-- Create user with password
CREATE USER innhotel_user WITH PASSWORD 'innhotel_secure_password_2024';

-- Grant all privileges
GRANT ALL PRIVILEGES ON DATABASE innhotel_db TO innhotel_user;

-- Grant schema privileges
\c innhotel_db
GRANT ALL ON SCHEMA public TO innhotel_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO innhotel_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO innhotel_user;

-- Exit PostgreSQL
\q
```

#### 3.2.3 Verify Database Connection

```bash
# Test connection
psql -h localhost -U innhotel_user -d innhotel_db -c "SELECT version();"

# You should see PostgreSQL version information
```

**Troubleshooting Database Connection:**
If connection fails:
1. Check PostgreSQL is running: `sudo systemctl status postgresql`
2. Verify port 5432 is listening: `sudo netstat -plntu | grep 5432`
3. Check pg_hba.conf for authentication settings
4. Ensure password is correct

### 3.3 API Setup

#### 3.3.1 Navigate to API Directory

```bash
cd HotelTransylvania/innhotel-api
```

#### 3.3.2 Configure Connection String

Create or edit the `.env` file in the API root:

```bash
# Create .env file
cat > .env << 'EOF'
ConnectionStrings__PostgreSQLConnection=Host=localhost;Port=5432;Username=innhotel_user;Password=innhotel_secure_password_2024;Database=innhotel_db
ALLOWED_ORIGINS=http://localhost:3000,http://localhost:5173,https://localhost:5173
ASPNETCORE_ENVIRONMENT=Development
EOF
```

#### 3.3.3 Restore Dependencies

```bash
# Restore NuGet packages
dotnet restore

# Expected output: Restore completed successfully
```

#### 3.3.4 Apply Database Migrations

```bash
# Navigate to Web project
cd src/InnHotel.Web

# Apply migrations
dotnet ef database update

# Expected output: Done. Applied migrations successfully
```

**Verify Migration:**
```bash
# Check applied migrations
dotnet ef migrations list

# Connect to database and verify tables
psql -h localhost -U innhotel_user -d innhotel_db -c "\dt"
```

#### 3.3.5 Build the API

```bash
# Build the project
dotnet build

# Expected output: Build succeeded. 0 Warning(s), 0 Error(s)
```

#### 3.3.6 Run the API

```bash
# Run the API
dotnet run --urls "https://localhost:57679"

# Expected output:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:57679
# info: Microsoft.Hosting.Lifetime[0]
#       Application started. Press Ctrl+C to shut down.
```

**Keep this terminal open** - the API needs to run continuously during testing.

#### 3.3.7 Verify API is Running

Open a new terminal and test:

```bash
# Test health endpoint
curl -k https://localhost:57679/health

# Expected response: Healthy
```

**Access Swagger Documentation:**
Open browser and navigate to: `https://localhost:57679/swagger`

You should see the Swagger UI with all API endpoints documented.

### 3.4 Client Setup

#### 3.4.1 Navigate to Client Directory

Open a new terminal:

```bash
cd HotelTransylvania/innhotel-desktop-client
```

#### 3.4.2 Configure Environment Variables

Create `.env` file:

```bash
cat > .env << 'EOF'
VITE_API_BASE_URL=https://localhost:57679/api
NODE_ENV=development
VITE_ENABLE_LOGGING=true
EOF
```

#### 3.4.3 Install Dependencies

```bash
# Install npm packages
npm install

# Expected output: added XXX packages in XXs
```

**If you encounter errors:**
```bash
# Clear cache and retry
npm cache clean --force
rm -rf node_modules package-lock.json
npm install
```

#### 3.4.4 Run Client in Development Mode

```bash
# Start development server
npm run dev:react

# Expected output:
# VITE v6.x.x ready in XXX ms
# ➜ Local:   http://localhost:5173/
# ➜ Network: use --host to expose
```

**Keep this terminal open** - the client needs to run during testing.

#### 3.4.5 Verify Client is Running

Open browser and navigate to: `http://localhost:5173`

You should see the InnHotel login page.

### 3.5 Docker Setup (Alternative)

If you prefer using Docker:

```bash
# Navigate to project root
cd HotelTransylvania

# Start all services
docker-compose up -d

# Check services status
docker-compose ps

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

**Docker Service URLs:**
- API: `http://localhost:57679`
- Client: `http://localhost:5173`
- PostgreSQL: `localhost:5432`

### 3.6 Environment Verification Checklist

Before proceeding to testing, verify:

- [ ] PostgreSQL is running and accessible
- [ ] Database `innhotel_db` exists with proper user permissions
- [ ] API is running on `https://localhost:57679`
- [ ] API health endpoint returns "Healthy"
- [ ] Swagger UI is accessible at `https://localhost:57679/swagger`
- [ ] Client is running on `http://localhost:5173`
- [ ] Client login page loads without errors
- [ ] No console errors in browser developer tools
- [ ] All three terminals (PostgreSQL, API, Client) are running

**Screenshot Checkpoint 1:** Take a screenshot showing:
1. API terminal with "Application started" message
2. Client terminal with "ready" message
3. Browser with login page loaded

---

## 4. API Testing

### 4.1 API Testing Overview

The InnHotel API provides RESTful endpoints for:
- Authentication (login, logout, token refresh)
- Branch management
- Room and room type management
- Guest management
- Employee management
- Reservation management
- Service management
- Dashboard statistics

### 4.2 Testing Tools Setup

#### 4.2.1 Postman Setup

1. **Download and Install Postman** from https://www.postman.com/downloads/
2. **Create a New Collection** named "InnHotel API Tests"
3. **Set Base URL Variable:**
   - Click on collection settings
   - Add variable: `baseUrl` = `https://localhost:57679/api`
   - Add variable: `accessToken` = (will be set after login)

#### 4.2.2 Import API Endpoints

You can use the provided `.http` files in `innhotel-api/http/tests/` as reference.

### 4.3 Authentication Testing

#### 4.3.1 Test Case: User Login (Positive)

**Objective:** Verify successful login with valid credentials

**Prerequisites:**
- API is running
- Database is seeded with default users

**Test Steps:**

1. **Open Postman** and create a new POST request
2. **Set URL:** `{{baseUrl}}/auth/login`
3. **Set Headers:**
   ```
   Content-Type: application/json
   ```
4. **Set Body (raw JSON):**
   ```json
   {
     "email": "super@innhotel.com",
     "password": "Sup3rP@ssword!"
   }
   ```
5. **Send Request**

**Expected Results:**
- Status Code: `200 OK`
- Response Body contains:
  ```json
  {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "email": "super@innhotel.com",
    "roles": ["SuperAdmin"]
  }
  ```
- Response time: < 2 seconds
- `Set-Cookie` header present with refresh token

**Validation Points:**
- ✓ Access token is a valid JWT string
- ✓ Email matches the login email
- ✓ Roles array contains expected role(s)
- ✓ Refresh token cookie is set with HttpOnly flag

**Save the Access Token:**
In Postman, add this to the Tests tab:
```javascript
pm.test("Login successful", function () {
    pm.response.to.have.status(200);
    var jsonData = pm.response.json();
    pm.environment.set("accessToken", jsonData.accessToken);
});
```

**Screenshot Checkpoint 2:** Capture successful login response

#### 4.3.2 Test Case: User Login (Negative - Invalid Credentials)

**Objective:** Verify proper error handling for invalid credentials

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/auth/login`
2. **Set Body:**
   ```json
   {
     "email": "super@innhotel.com",
     "password": "WrongPassword123!"
   }
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `401 Unauthorized`
- Response Body:
  ```json
  {
    "message": "Invalid email or password",
    "errors": []
  }
  ```

**Validation Points:**
- ✓ Appropriate error status code
- ✓ Clear error message
- ✓ No sensitive information leaked
- ✓ No access token returned

#### 4.3.3 Test Case: User Login (Negative - Missing Fields)

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/auth/login`
2. **Set Body:**
   ```json
   {
     "email": "super@innhotel.com"
   }
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `400 Bad Request`
- Response Body contains validation errors:
  ```json
  {
    "message": "Validation failed",
    "errors": [
      {
        "field": "password",
        "message": "Password is required"
      }
    ]
  }
  ```

#### 4.3.4 Test Case: Token Refresh

**Objective:** Verify refresh token functionality

**Prerequisites:**
- User is logged in
- Refresh token cookie is set

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/auth/refresh`
2. **Set Headers:**
   ```
   Cookie: refreshToken=<token_from_login>
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `200 OK`
- Response Body contains new access token:
  ```json
  {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "email": "super@innhotel.com",
    "roles": ["SuperAdmin"]
  }
  ```
- New refresh token cookie is set

**Validation Points:**
- ✓ New access token is different from previous
- ✓ Token is valid and not expired
- ✓ User information is preserved

#### 4.3.5 Test Case: Logout

**Objective:** Verify logout functionality

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/auth/logout`
2. **Set Headers:**
   ```
   Authorization: Bearer {{accessToken}}
   Cookie: refreshToken=<token>
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `200 OK`
- Refresh token cookie is cleared
- Subsequent requests with old token fail

#### 4.3.6 Test Case: Protected Endpoint Without Token

**Objective:** Verify authentication is enforced

**Test Steps:**

1. **Create GET request:** `{{baseUrl}}/branches`
2. **Do NOT set Authorization header**
3. **Send Request**

**Expected Results:**
- Status Code: `401 Unauthorized`
- Response indicates authentication required

### 4.4 Branch Management Testing

#### 4.4.1 Test Case: Create Branch (Positive)

**Objective:** Verify branch creation with valid data

**Prerequisites:**
- User is authenticated as SuperAdmin or Admin
- Access token is valid

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/branches`
2. **Set Headers:**
   ```
   Authorization: Bearer {{accessToken}}
   Content-Type: application/json
   ```
3. **Set Body:**
   ```json
   {
     "name": "Downtown Branch",
     "address": "123 Main Street, City, State 12345",
     "phoneNumber": "+1-555-0100",
     "email": "downtown@innhotel.com"
   }
   ```
4. **Send Request**

**Expected Results:**
- Status Code: `201 Created`
- Response Body:
  ```json
  {
    "id": 1,
    "name": "Downtown Branch",
    "address": "123 Main Street, City, State 12345",
    "phoneNumber": "+1-555-0100",
    "email": "downtown@innhotel.com",
    "createdAt": "2025-10-18T15:53:03Z"
  }
  ```
- Location header contains URL to new resource

**Validation Points:**
- ✓ Branch ID is generated
- ✓ All fields match input data
- ✓ Timestamps are set correctly
- ✓ Email format is valid
- ✓ Phone number format is valid

**Screenshot Checkpoint 3:** Capture successful branch creation

#### 4.4.2 Test Case: Create Branch (Negative - Duplicate Name)

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/branches`
2. **Set Headers:** (with valid auth token)
3. **Set Body:** (same branch name as existing)
4. **Send Request**

**Expected Results:**
- Status Code: `409 Conflict` or `400 Bad Request`
- Error message indicates duplicate branch name

#### 4.4.3 Test Case: Create Branch (Negative - Invalid Data)

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/branches`
2. **Set Body:**
   ```json
   {
     "name": "",
     "address": "123 Main St",
     "phoneNumber": "invalid-phone",
     "email": "not-an-email"
   }
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `400 Bad Request`
- Validation errors for each invalid field:
  ```json
  {
    "message": "Validation failed",
    "errors": [
      {
        "field": "name",
        "message": "Name is required"
      },
      {
        "field": "phoneNumber",
        "message": "Invalid phone number format"
      },
      {
        "field": "email",
        "message": "Invalid email format"
      }
    ]
  }
  ```

#### 4.4.4 Test Case: Get All Branches

**Objective:** Verify branch listing with pagination

**Test Steps:**

1. **Create GET request:** `{{baseUrl}}/branches?pageNumber=1&pageSize=10`
2. **Set Headers:**
   ```
   Authorization: Bearer {{accessToken}}
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `200 OK`
- Response Body:
  ```json
  {
    "items": [
      {
        "id": 1,
        "name": "Downtown Branch",
        "address": "123 Main Street",
        "phoneNumber": "+1-555-0100",
        "email": "downtown@innhotel.com"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1,
    "totalCount": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  }
  ```

**Validation Points:**
- ✓ Pagination metadata is correct
- ✓ Items array contains branch objects
- ✓ Page size is respected
- ✓ Total count matches actual records

#### 4.4.5 Test Case: Get Branch by ID

**Test Steps:**

1. **Create GET request:** `{{baseUrl}}/branches/1`
2. **Set Headers:** (with auth token)
3. **Send Request**

**Expected Results:**
- Status Code: `200 OK`
- Response contains single branch object with ID 1

#### 4.4.6 Test Case: Get Branch by ID (Negative - Not Found)

**Test Steps:**

1. **Create GET request:** `{{baseUrl}}/branches/99999`
2. **Send Request**

**Expected Results:**
- Status Code: `404 Not Found`
- Error message: "Branch not found"

#### 4.4.7 Test Case: Update Branch

**Objective:** Verify branch update functionality

**Test Steps:**

1. **Create PUT request:** `{{baseUrl}}/branches/1`
2. **Set Headers:** (with auth token)
3. **Set Body:**
   ```json
   {
     "id": 1,
     "name": "Downtown Branch - Updated",
     "address": "456 New Street",
     "phoneNumber": "+1-555-0200",
     "email": "downtown.updated@innhotel.com"
   }
   ```
4. **Send Request**

**Expected Results:**
- Status Code: `200 OK` or `204 No Content`
- Branch is updated in database

**Verification:**
- Retrieve branch by ID and verify changes

#### 4.4.8 Test Case: Delete Branch

**Objective:** Verify branch deletion

**Test Steps:**

1. **Create DELETE request:** `{{baseUrl}}/branches/1`
2. **Set Headers:** (with auth token)
3. **Send Request**

**Expected Results:**
- Status Code: `204 No Content` or `200 OK`
- Branch is removed from database

**Verification:**
- Attempt to retrieve deleted branch (should return 404)

### 4.5 Room Management Testing

#### 4.5.1 Test Case: Create Room Type

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/roomtypes`
2. **Set Body:**
   ```json
   {
     "name": "Deluxe Suite",
     "description": "Spacious suite with king bed and city view",
     "basePrice": 299.99,
     "capacity": 2,
     "amenities": ["WiFi", "TV", "Mini Bar", "Air Conditioning"]
   }
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `201 Created`
- Room type is created with generated ID

#### 4.5.2 Test Case: Create Room

**Prerequisites:**
- At least one branch exists
- At least one room type exists

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/rooms`
2. **Set Body:**
   ```json
   {
     "roomNumber": "101",
     "floor": 1,
     "branchId": 1,
     "roomTypeId": 1,
     "status": "Available"
   }
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `201 Created`
- Room is created and linked to branch and room type

#### 4.5.3 Test Case: Search Rooms

**Test Steps:**

1. **Create GET request:** `{{baseUrl}}/rooms/search?status=Available&floor=1`
2. **Send Request**

**Expected Results:**
- Status Code: `200 OK`
- Returns only rooms matching search criteria

### 4.6 Guest Management Testing

#### 4.6.1 Test Case: Create Guest

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/guests`
2. **Set Body:**
   ```json
   {
     "firstName": "John",
     "lastName": "Doe",
     "email": "john.doe@example.com",
     "phoneNumber": "+1-555-0123",
     "address": "789 Guest Street",
     "dateOfBirth": "1990-01-15",
     "idNumber": "ID123456789"
   }
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `201 Created`
- Guest record is created

**Validation Points:**
- ✓ Email is unique
- ✓ Date of birth is valid
- ✓ ID number is stored securely

#### 4.6.2 Test Case: Search Guests

**Test Steps:**

1. **Create GET request:** `{{baseUrl}}/guests/search?searchTerm=John`
2. **Send Request**

**Expected Results:**
- Returns guests matching search term in name or email

### 4.7 Reservation Management Testing

#### 4.7.1 Test Case: Create Reservation

**Prerequisites:**
- Guest exists
- Room is available
- Dates are valid

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/reservations`
2. **Set Body:**
   ```json
   {
     "guestId": 1,
     "checkInDate": "2025-11-01",
     "checkOutDate": "2025-11-05",
     "numberOfGuests": 2,
     "rooms": [
       {
         "roomId": 1,
         "pricePerNight": 299.99
       }
     ],
     "services": [],
     "totalAmount": 1199.96,
     "status": "Confirmed"
   }
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `201 Created`
- Reservation is created
- Room status changes to "Reserved"
- Total amount is calculated correctly

**Validation Points:**
- ✓ Check-in date is before check-out date
- ✓ Room is available for selected dates
- ✓ Total amount calculation is correct
- ✓ Guest exists in system

#### 4.7.2 Test Case: Create Reservation (Negative - Room Not Available)

**Test Steps:**

1. **Create reservation for already booked room**
2. **Send Request**

**Expected Results:**
- Status Code: `409 Conflict`
- Error message: "Room is not available for selected dates"

#### 4.7.3 Test Case: Search Reservations

**Test Steps:**

1. **Create GET request:** `{{baseUrl}}/reservations/search?status=Confirmed&checkInDate=2025-11-01`
2. **Send Request**

**Expected Results:**
- Returns reservations matching criteria

### 4.8 Employee Management Testing

#### 4.8.1 Test Case: Create Employee

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/employees`
2. **Set Body:**
   ```json
   {
     "firstName": "Jane",
     "lastName": "Smith",
     "email": "jane.smith@innhotel.com",
     "phoneNumber": "+1-555-0456",
     "position": "Receptionist",
     "branchId": 1,
     "hireDate": "2025-10-01",
     "salary": 45000.00
   }
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `201 Created`
- Employee record is created

### 4.9 Service Management Testing

#### 4.9.1 Test Case: Create Service

**Test Steps:**

1. **Create POST request:** `{{baseUrl}}/services`
2. **Set Body:**
   ```json
   {
     "name": "Airport Shuttle",
     "description": "Round-trip airport transportation",
     "price": 50.00,
     "category": "Transportation"
   }
   ```
3. **Send Request**

**Expected Results:**
- Status Code: `201 Created`
- Service is created

### 4.10 Dashboard Testing

#### 4.10.1 Test Case: Get Dashboard Statistics

**Test Steps:**

1. **Create GET request:** `{{baseUrl}}/dashboard/statistics`
2. **Send Request**

**Expected Results:**
- Status Code: `200 OK`
- Response contains:
  ```json
  {
    "totalReservations": 10,
    "totalGuests": 25,
    "totalRevenue": 15000.00,
    "occupancyRate": 75.5,
    "availableRooms": 15,
    "checkInsToday": 3,
    "checkOutsToday": 2
  }
  ```

### 4.11 API Testing Checklist

Complete this checklist for each endpoint:

**Authentication Endpoints:**
- [ ] POST /api/auth/login - Valid credentials
- [ ] POST /api/auth/login - Invalid credentials
- [ ] POST /api/auth/login - Missing fields
- [ ] POST /api/auth/refresh - Valid token
- [ ] POST /api/auth/refresh - Expired token
- [ ] POST /api/auth/logout - Successful logout
- [ ] POST /api/auth/register - Create new user (Admin only)

**Branch Endpoints:**
- [ ] POST /api/branches - Create branch
- [ ] GET /api/branches - List all branches
- [ ] GET /api/branches/{id} - Get branch by ID
- [ ] PUT /api/branches/{id} - Update branch
- [ ] DELETE /api/branches/{id} - Delete branch

**Room Type Endpoints:**
- [ ] POST /api/roomtypes - Create room type
- [ ] GET /api/roomtypes - List room types
- [ ] GET /api/roomtypes/{id} - Get room type
- [ ] PUT /api/roomtypes/{id} - Update room type
- [ ] DELETE /api/roomtypes/{id} - Delete room type

**Room Endpoints:**
- [ ] POST /api/rooms - Create room
- [ ] GET /api/rooms - List rooms
- [ ] GET /api/rooms/{id} - Get room
- [ ] PUT /api/rooms/{id} - Update room
- [ ] DELETE /api/rooms/{id} - Delete room
- [ ] GET /api/rooms/search - Search rooms

**Guest Endpoints:**
- [ ] POST /api/guests - Create guest
- [ ] GET /api/guests - List guests
- [ ] GET /api/guests/{id} - Get guest
- [ ] PUT /api/guests/{id} - Update guest
- [ ] DELETE /api/guests/{id} - Delete guest
- [ ] GET /api/guests/search - Search guests

**Reservation Endpoints:**
- [ ] POST /api/reservations - Create reservation
- [ ] GET /api/reservations - List reservations
- [ ] GET /api/reservations/{id} - Get reservation
- [ ] PUT /api/reservations/{id} - Update reservation
- [ ] DELETE /api/reservations/{id} - Cancel reservation
- [ ] GET /api/reservations/search - Search reservations

**Employee Endpoints:**
- [ ] POST /api/employees - Create employee
- [ ] GET /api/employees - List employees
- [ ] GET /api/employees/{id} - Get employee
- [ ] PUT /api/employees/{id} - Update employee
- [ ] DELETE /api/employees/{id} - Delete employee
- [ ] GET /api/employees/search - Search employees

**Service Endpoints:**
- [ ] POST /api/services - Create service
- [ ] GET /api/services - List services
- [ ] GET /api/services/{id} - Get service
- [ ] PUT /api/services/{id} - Update service
- [ ] DELETE /api/services/{id} - Delete service

**Dashboard Endpoints:**
- [ ] GET /api/dashboard/statistics - Get statistics

---

## 5. Client Testing

### 5.1 Client Testing Overview

The InnHotel Desktop Client is an Electron-based React application that provides a user interface for hotel management operations.

### 5.2 Pre-Testing Setup

#### 5.2.1 Browser Developer Tools

Open Developer Tools in your browser:
- **Chrome/Chromium:** Press `F12` or `Ctrl+Shift+I` (Windows/Linux) / `Cmd+Option+I` (Mac)
- **Firefox:** Press `F12` or `Ctrl+Shift+I`

**Monitor these tabs during testing:**
- **Console:** Check for JavaScript errors
- **Network:** Monitor API requests and responses
- **Application:** Inspect local storage, cookies, and session data

### 5.3 Authentication Testing

#### 5.3.1 Test Case: Login Page Load

**Objective:** Verify login page loads correctly

**Test Steps:**

1. Open browser and navigate to `http://localhost:5173`
2. Observe page loading

**Expected Results:**
- Login page displays within 2 seconds
- No console errors
- All UI elements are visible:
  - Email input field
  - Password input field
  - "Login" button
  - InnHotel logo/branding
- Page is responsive and properly styled

**Screenshot Checkpoint 4:** Capture login page

#### 5.3.2 Test Case: Successful Login

**Objective:** Verify user can log in with valid credentials

**Test Steps:**

1. Enter email: `super@innhotel.com`
2. Enter password: `Sup3rP@ssword!`
3. Click "Login" button
4. Observe behavior

**Expected Results:**
- Loading indicator appears briefly
- User is redirected to dashboard
- No error messages displayed
- User information is displayed (name, role)
- Navigation menu is visible
- Access token is stored (check Application tab > Local Storage)

**Validation Points:**
- ✓ Redirect happens within 2 seconds
- ✓ Dashboard loads successfully
- ✓ User role is displayed correctly
- ✓ No console errors

**Screenshot Checkpoint 5:** Capture dashboard after login

#### 5.3.3 Test Case: Failed Login - Invalid Credentials

**Test Steps:**

1. Enter email: `super@innhotel.com`
2. Enter password: `WrongPassword`
3. Click "Login" button

**Expected Results:**
- Error message displayed: "Invalid email or password"
- User remains on login page
- Password field is cleared
- No redirect occurs
- Error message is styled appropriately (red color, icon)

#### 5.3.4 Test Case: Failed Login - Empty Fields

**Test Steps:**

1. Leave email field empty
2. Leave password field empty
3. Click "Login" button

**Expected Results:**
- Validation errors displayed for both fields
- "Email is required" message
- "Password is required" message
- Login button may be disabled
- No API request is made

#### 5.3.5 Test Case: Failed Login - Invalid Email Format

**Test Steps:**

1. Enter email: `not-an-email`
2. Enter password: `Sup3rP@ssword!`
3. Click "Login" button

**Expected Results:**
- Validation error: "Invalid email format"
- No API request is made

#### 5.3.6 Test Case: Logout

**Objective:** Verify logout functionality

**Prerequisites:**
- User is logged in

**Test Steps:**

1. Click on user profile/menu
2. Click "Logout" button
3. Observe behavior

**Expected Results:**
- User is redirected to login page
- Access token is removed from storage
- All user data is cleared
- Cannot access protected pages without re-login

### 5.4 Dashboard Testing

#### 5.4.1 Test Case: Dashboard Load

**Objective:** Verify dashboard displays correctly

**Prerequisites:**
- User is logged in

**Test Steps:**

1. Navigate to dashboard (default after login)
2. Observe dashboard content

**Expected Results:**
- Dashboard loads within 3 seconds
- Statistics cards are displayed:
  - Total Reservations
  - Total Guests
  - Total Revenue
  - Occupancy Rate
  - Available Rooms
- Charts/graphs are rendered (if applicable)
- Recent activity list is displayed
- All data is fetched from API

**Validation Points:**
- ✓ Numbers are formatted correctly (currency, percentages)
- ✓ No loading spinners stuck
- ✓ No "undefined" or "null" values displayed

**Screenshot Checkpoint 6:** Capture dashboard with statistics

#### 5.4.2 Test Case: Dashboard Real-Time Updates

**Objective:** Verify dashboard updates with real-time data

**Test Steps:**

1. Keep dashboard open
2. In another browser tab, create a new reservation via API
3. Observe dashboard

**Expected Results:**
- Dashboard statistics update automatically (via SignalR)
- No page refresh required
- Updated values are highlighted or animated

### 5.5 Branch Management Testing

#### 5.5.1 Test Case: View Branches List

**Test Steps:**

1. Click "Branches" in navigation menu
2. Observe branches listing page

**Expected Results:**
- Branches list page loads
- Table displays branches with columns:
  - ID
  - Name
  - Address
  - Phone Number
  - Email
  - Actions (Edit, Delete)
- Pagination controls are visible
- Search box is available

**Screenshot Checkpoint 7:** Capture branches listing

#### 5.5.2 Test Case: Create New Branch

**Test Steps:**

1. Click "Add Branch" button
2. Fill in form:
   - Name: "Test Branch"
   - Address: "123 Test Street"
   - Phone: "+1-555-0999"
   - Email: "test@innhotel.com"
3. Click "Save" button

**Expected Results:**
- Form validation passes
- Success message displayed
- New branch appears in list
- Form is cleared or modal closes
- User is redirected to branches list

**Validation Points:**
- ✓ Form fields are validated
- ✓ Required fields are marked
- ✓ Email format is validated
- ✓ Phone format is validated

#### 5.5.3 Test Case: Create Branch - Validation Errors

**Test Steps:**

1. Click "Add Branch" button
2. Leave all fields empty
3. Click "Save" button

**Expected Results:**
- Validation errors displayed for each required field
- Form is not submitted
- Error messages are clear and specific

#### 5.5.4 Test Case: Edit Branch

**Test Steps:**

1. Click "Edit" button on a branch
2. Modify branch name
3. Click "Save" button

**Expected Results:**
- Branch details are updated
- Success message displayed
- Updated data appears in list

#### 5.5.5 Test Case: Delete Branch

**Test Steps:**

1. Click "Delete" button on a branch
2. Confirm deletion in dialog

**Expected Results:**
- Confirmation dialog appears
- After confirmation, branch is deleted
- Success message displayed
- Branch is removed from list

#### 5.5.6 Test Case: Search Branches

**Test Steps:**

1. Enter search term in search box
2. Observe results

**Expected Results:**
- Results are filtered in real-time
- Only matching branches are displayed
- Search is case-insensitive

### 5.6 Room Management Testing

#### 5.6.1 Test Case: View Rooms List

**Test Steps:**

1. Click "Rooms" in navigation
2. Observe rooms listing

**Expected Results:**
- Rooms list displays with:
  - Room Number
  - Floor
  - Branch
  - Room Type
  - Status (Available, Occupied, Maintenance)
  - Actions
- Status is color-coded
- Filters are available (by status, floor, branch)

#### 5.6.2 Test Case: Create Room

**Test Steps:**

1. Click "Add Room" button
2. Fill in form:
   - Room Number: "201"
   - Floor: 2
   - Branch: Select from dropdown
   - Room Type: Select from dropdown
   - Status: "Available"
3. Click "Save"

**Expected Results:**
- Room is created successfully
- Appears in rooms list
- Linked to correct branch and room type

#### 5.6.3 Test Case: Filter Rooms by Status

**Test Steps:**

1. Select "Available" from status filter
2. Observe results

**Expected Results:**
- Only available rooms are displayed
- Count is updated

#### 5.6.4 Test Case: Change Room Status

**Test Steps:**

1. Click on a room
2. Change status from "Available" to "Maintenance"
3. Save changes

**Expected Results:**
- Status is updated
- Color coding changes
- Status change is reflected immediately

### 5.7 Guest Management Testing

#### 5.7.1 Test Case: View Guests List

**Test Steps:**

1. Navigate to "Guests" page
2. Observe guests listing

**Expected Results:**
- Guests list displays with:
  - Name
  - Email
  - Phone Number
  - Total Reservations
  - Actions

#### 5.7.2 Test Case: Create Guest

**Test Steps:**

1. Click "Add Guest" button
2. Fill in form:
   - First Name: "John"
   - Last Name: "Doe"
   - Email: "john.doe@example.com"
   - Phone: "+1-555-0123"
   - Address: "123 Guest St"
   - Date of Birth: "1990-01-15"
   - ID Number: "ID123456"
3. Click "Save"

**Expected Results:**
- Guest is created
- Validation passes for all fields
- Email is unique

#### 5.7.3 Test Case: Search Guests

**Test Steps:**

1. Enter "John" in search box
2. Observe results

**Expected Results:**
- Guests matching "John" in name or email are displayed
- Search is real-time

#### 5.7.4 Test Case: View Guest Details

**Test Steps:**

1. Click on a guest name
2. Observe guest details page

**Expected Results:**
- Guest information is displayed
- Reservation history is shown
- Total spent is calculated

### 5.8 Reservation Management Testing

#### 5.8.1 Test Case: Create Reservation

**Test Steps:**

1. Click "New Reservation" button
2. Fill in form:
   - Select Guest (or create new)
   - Check-in Date: Future date
   - Check-out Date: After check-in
   - Number of Guests: 2
   - Select Room(s)
   - Add Services (optional)
3. Review total amount
4. Click "Confirm Reservation"

**Expected Results:**
- Reservation is created
- Total amount is calculated correctly
- Room status changes to "Reserved"
- Confirmation message displayed
- Reservation appears in list

**Validation Points:**
- ✓ Check-in date is not in the past
- ✓ Check-out date is after check-in date
- ✓ Selected room is available
- ✓ Total calculation includes room price × nights + services

#### 5.8.2 Test Case: Create Reservation - Room Not Available

**Test Steps:**

1. Attempt to create reservation for already booked room
2. Submit form

**Expected Results:**
- Error message: "Room is not available for selected dates"
- Reservation is not created
- Alternative rooms are suggested (if feature exists)

#### 5.8.3 Test Case: View Reservations Calendar

**Test Steps:**

1. Navigate to "Reservations" page
2. Switch to calendar view

**Expected Results:**
- Calendar displays reservations
- Each reservation shows:
  - Guest name
  - Room number
  - Check-in/out dates
- Can navigate between months
- Can click on reservation for details

#### 5.8.4 Test Case: Check-In Guest

**Test Steps:**

1. Find reservation with today's check-in date
2. Click "Check-In" button
3. Confirm action

**Expected Results:**
- Reservation status changes to "CheckedIn"
- Room status changes to "Occupied"
- Check-in time is recorded

#### 5.8.5 Test Case: Check-Out Guest

**Test Steps:**

1. Find checked-in reservation
2. Click "Check-Out" button
3. Review charges
4. Process payment
5. Confirm check-out

**Expected Results:**
- Reservation status changes to "CheckedOut"
- Room status changes to "Available"
- Final bill is generated
- Check-out time is recorded

#### 5.8.6 Test Case: Cancel Reservation

**Test Steps:**

1. Select a reservation
2. Click "Cancel" button
3. Confirm cancellation

**Expected Results:**
- Confirmation dialog appears
- After confirmation, reservation status changes to "Cancelled"
- Room becomes available again

### 5.9 Employee Management Testing

#### 5.9.1 Test Case: View Employees List

**Test Steps:**

1. Navigate to "Employees" page
2. Observe employees listing

**Expected Results:**
- Employees list displays with:
  - Name
  - Position
  - Branch
  - Email
  - Hire Date
  - Actions

#### 5.9.2 Test Case: Create Employee

**Test Steps:**

1. Click "Add Employee" button
2. Fill in form
3. Click "Save"

**Expected Results:**
- Employee is created
- Linked to branch
- User account is created (if applicable)

### 5.10 Service Management Testing

#### 5.10.1 Test Case: View Services List

**Test Steps:**

1. Navigate to "Services" page
2. Observe services listing

**Expected Results:**
- Services list displays with:
  - Name
  - Description
  - Price
  - Category
  - Actions

#### 5.10.2 Test Case: Create Service

**Test Steps:**

1. Click "Add Service" button
2. Fill in form
3. Click "Save"

**Expected Results:**
- Service is created
- Available for reservations

### 5.11 UI/UX Testing

#### 5.11.1 Test Case: Responsive Design

**Test Steps:**

1. Resize browser window to different sizes:
   - Desktop (1920x1080)
   - Laptop (1366x768)
   - Tablet (768x1024)
   - Mobile (375x667)
2. Observe layout

**Expected Results:**
- Layout adapts to screen size
- No horizontal scrolling
- All elements are accessible
- Navigation menu collapses on mobile

#### 5.11.2 Test Case: Navigation

**Test Steps:**

1. Click through all navigation menu items
2. Use browser back/forward buttons
3. Use breadcrumbs (if available)

**Expected Results:**
- All pages load correctly
- Browser navigation works
- Active menu item is highlighted
- Breadcrumbs show correct path

#### 5.11.3 Test Case: Form Validation

**Test Steps:**

1. Test each form in the application
2. Try submitting with invalid data
3. Try submitting with empty required fields

**Expected Results:**
- Validation errors are displayed
- Error messages are clear
- Fields are highlighted
- Form is not submitted until valid

#### 5.11.4 Test Case: Loading States

**Test Steps:**

1. Observe loading indicators during:
   - Page load
   - Data fetching
   - Form submission
2. Simulate slow network (Chrome DevTools > Network > Throttling)

**Expected Results:**
- Loading spinners/skeletons are displayed
- User cannot interact with loading elements
- Loading states are consistent across app

#### 5.11.5 Test Case: Error Handling

**Test Steps:**

1. Stop API server
2. Try to perform actions in client
3. Observe error handling

**Expected Results:**
- User-friendly error messages displayed
- No technical error details exposed
- Option to retry action
- User is not stuck in error state

### 5.12 Client Testing Checklist

**Authentication:**
- [ ] Login with valid credentials
- [ ] Login with invalid credentials
- [ ] Login with empty fields
- [ ] Logout functionality
- [ ] Session persistence
- [ ] Token refresh

**Dashboard:**
- [ ] Dashboard loads correctly
- [ ] Statistics are displayed
- [ ] Real-time updates work
- [ ] Charts render properly

**Branch Management:**
- [ ] View branches list
- [ ] Create new branch
- [ ] Edit branch
- [ ] Delete branch
- [ ] Search branches
- [ ] Form validation

**Room Management:**
- [ ] View rooms list
- [ ] Create room
- [ ] Edit room
- [ ] Delete room
- [ ] Filter by status
- [ ] Change room status

**Guest Management:**
- [ ] View guests list
- [ ] Create guest
- [ ] Edit guest
- [ ] Delete guest
- [ ] Search guests
- [ ] View guest details

**Reservation Management:**
- [ ] Create reservation
- [ ] View reservations
- [ ] Edit reservation
- [ ] Cancel reservation
- [ ] Check-in guest
- [ ] Check-out guest
- [ ] Calendar view
- [ ] Search reservations

**Employee Management:**
- [ ] View employees list
- [ ] Create employee
- [ ] Edit employee
- [ ] Delete employee
- [ ] Search employees

**Service Management:**
- [ ] View services list
- [ ] Create service
- [ ] Edit service
- [ ] Delete service

**UI/UX:**
- [ ] Responsive design
- [ ] Navigation works
- [ ] Forms validate correctly
- [ ] Loading states display
- [ ] Error handling works
- [ ] Accessibility features

---

## 6. Integration Testing

### 6.1 Integration Testing Overview

Integration testing verifies that the API, Client, and Database work together correctly.

### 6.2 End-to-End Workflows

#### 6.2.1 Test Case: Complete Reservation Workflow

**Objective:** Test the entire reservation process from guest creation to check-out

**Test Steps:**

1. **Create Guest:**
   - Open client
   - Navigate to Guests
   - Create new guest
   - Verify guest appears in list

2. **Create Reservation:**
   - Navigate to Reservations
   - Click "New Reservation"
   - Select the newly created guest
   - Select dates and room
   - Add services
   - Confirm reservation
   - Verify reservation is created

3. **Check-In:**
   - On check-in date, find reservation
   - Click "Check-In"
   - Verify status changes to "CheckedIn"
   - Verify room status is "Occupied"

4. **Check-Out:**
   - On check-out date, find reservation
   - Click "Check-Out"
   - Review charges
   - Process payment
   - Verify status changes to "CheckedOut"
   - Verify room status is "Available"

5. **Verify Data:**
   - Check database for correct records
   - Verify guest history shows reservation
   - Verify revenue is calculated correctly

**Expected Results:**
- All steps complete successfully
- Data is consistent across API, Client, and Database
- No errors occur during workflow

#### 6.2.2 Test Case: Multi-User Concurrent Access

**Objective:** Verify system handles multiple users simultaneously

**Test Steps:**

1. Open two browser windows
2. Log in as different users in each
3. Perform actions simultaneously:
   - User 1: Create reservation
   - User 2: View same room
4. Observe behavior

**Expected Results:**
- Both users can work independently
- Real-time updates are reflected
- No data conflicts occur
- Room availability is updated correctly

#### 6.2.3 Test Case: Data Consistency

**Objective:** Verify data remains consistent across operations

**Test Steps:**

1. Create a branch via API
2. Verify it appears in client
3. Update branch via client
4. Verify update in database
5. Delete branch via API
6. Verify deletion in client

**Expected Results:**
- Data is consistent across all layers
- Changes are reflected immediately
- No orphaned records

### 6.3 SignalR Real-Time Testing

#### 6.3.1 Test Case: Real-Time Notifications

**Test Steps:**

1. Open client in two browser windows
2. Log in as different users
3. In window 1, create a reservation
4. Observe window 2

**Expected Results:**
- Window 2 receives real-time notification
- Dashboard statistics update automatically
- No page refresh required

### 6.4 Performance Testing

#### 6.4.1 Test Case: API Response Time

**Test Steps:**

1. Use Postman to measure response times
2. Test each endpoint 10 times
3. Calculate average response time

**Expected Results:**
- GET requests: < 500ms
- POST requests: < 1000ms
- PUT requests: < 1000ms
- DELETE requests: < 500ms

#### 6.4.2 Test Case: Client Load Time

**Test Steps:**

1. Clear browser cache
2. Open client
3. Measure time to interactive

**Expected Results:**
- Initial load: < 3 seconds
- Subsequent page loads: < 1 second

#### 6.4.3 Test Case: Database Query Performance

**Test Steps:**

1. Connect to database
2. Run EXPLAIN ANALYZE on common queries
3. Check execution time

**Expected Results:**
- Simple queries: < 50ms
- Complex queries: < 200ms
- Indexes are used effectively

### 6.5 Security Testing

#### 6.5.1 Test Case: Authentication Required

**Test Steps:**

1. Try to access protected endpoints without token
2. Try to access client pages without login

**Expected Results:**
- API returns 401 Unauthorized
- Client redirects to login page

#### 6.5.2 Test Case: Authorization Checks

**Test Steps:**

1. Log in as regular user
2. Try to access admin-only features

**Expected Results:**
- Access is denied
- Appropriate error message displayed

#### 6.5.3 Test Case: SQL Injection Prevention

**Test Steps:**

1. Try SQL injection in search fields:
   - `' OR '1'='1`
   - `'; DROP TABLE guests; --`
2. Submit forms with malicious input

**Expected Results:**
- Input is sanitized
- No SQL errors occur
- Database is not affected

#### 6.5.4 Test Case: XSS Prevention

**Test Steps:**

1. Try to inject JavaScript in text fields:
   - `<script>alert('XSS')</script>`
   - `<img src=x onerror=alert('XSS')>`
2. Submit and view data

**Expected Results:**
- Scripts are not executed
- HTML is escaped
- Data is displayed safely

---

## 7. Error Handling & Logging

### 7.1 Error Handling Testing

#### 7.1.1 Test Case: API Error Responses

**Test Steps:**

1. Trigger various error conditions:
   - Invalid data
   - Missing required fields
   - Duplicate records
   - Not found errors
   - Server errors

**Expected Results:**
- Appropriate HTTP status codes
- Clear error messages
- Error details (in development)
- No sensitive information leaked

#### 7.1.2 Test Case: Client Error Display

**Test Steps:**

1. Trigger errors in client
2. Observe error messages

**Expected Results:**
- User-friendly error messages
- Error styling (color, icons)
- Option to dismiss or retry
- No technical jargon

#### 7.1.3 Test Case: Network Errors

**Test Steps:**

1. Disconnect network
2. Try to perform actions
3. Reconnect network

**Expected Results:**
- "Network error" message displayed
- Actions can be retried
- Data is not lost
- User is informed of connection status

### 7.2 Logging Verification

#### 7.2.1 Test Case: API Logging

**Test Steps:**

1. Perform various actions
2. Check API logs

**Expected Results:**
- Requests are logged
- Errors are logged with stack traces
- Log levels are appropriate (Info, Warning, Error)
- Sensitive data is not logged

**Log File Location:**
- Development: Console output
- File: `innhotel-api/src/InnHotel.Web/log.txt`

#### 7.2.2 Test Case: Client Logging

**Test Steps:**

1. Open browser console
2. Perform actions
3. Observe console logs

**Expected Results:**
- Important events are logged
- Errors are logged with context
- Logs are structured and readable
- No excessive logging in production

### 7.3 Error Scenarios

#### 7.3.1 Database Connection Lost

**Test Steps:**

1. Stop PostgreSQL service
2. Try to perform actions

**Expected Results:**
- API returns 503 Service Unavailable
- Client displays "Service temporarily unavailable"
- System attempts to reconnect

#### 7.3.2 API Server Down

**Test Steps:**

1. Stop API server
2. Try to use client

**Expected Results:**
- Client displays connection error
- User is informed API is unavailable
- Actions are queued (if applicable)

#### 7.3.3 Invalid Token

**Test Steps:**

1. Manually modify access token
2. Try to make authenticated request

**Expected Results:**
- API returns 401 Unauthorized
- Client redirects to login
- Token is cleared

---

## 8. Test Case Scenarios

### 8.1 User Role Testing

#### 8.1.1 SuperAdmin Role

**Permissions:**
- Full access to all features
- Can create/edit/delete all entities
- Can manage users and roles
- Can access all branches

**Test Cases:**
- [ ] Can create branches
- [ ] Can delete branches
- [ ] Can create admin users
- [ ] Can access all data
- [ ] Can modify system settings

#### 8.1.2 Admin Role

**Permissions:**
- Can manage branch-specific data
- Can create/edit/delete within assigned branch
- Cannot create other admins
- Cannot delete branches

**Test Cases:**
- [ ] Can create reservations in assigned branch
- [ ] Cannot access other branches' data
- [ ] Cannot create admin users
- [ ] Can manage employees in branch

#### 8.1.3 Receptionist Role

**Permissions:**
- Can create/view/edit reservations
- Can check-in/check-out guests
- Can view guest information
- Cannot delete records
- Cannot access financial reports

**Test Cases:**
- [ ] Can create reservations
- [ ] Can check-in guests
- [ ] Cannot delete reservations
- [ ] Cannot view financial data
- [ ] Cannot manage employees

#### 8.1.4 Manager Role

**Permissions:**
- Can view all branch data
- Can generate reports
- Can manage employees
- Cannot modify system settings

**Test Cases:**
- [ ] Can view all reservations
- [ ] Can generate reports
- [ ] Can manage employees
- [ ] Cannot create branches
- [ ] Cannot modify prices

### 8.2 Business Logic Testing

#### 8.2.1 Pricing Calculation

**Test Cases:**

1. **Base Price Calculation:**
   - Room price × number of nights
   - Verify calculation is correct

2. **Service Charges:**
   - Add services to reservation
   - Verify services are added to total

3. **Discounts:**
   - Apply discount code (if feature exists)
   - Verify discount is calculated correctly

4. **Taxes:**
   - Verify tax calculation
   - Verify tax is added to total

#### 8.2.2 Room Availability

**Test Cases:**

1. **Overlapping Reservations:**
   - Try to book room with overlapping dates
   - Verify error is shown

2. **Same-Day Booking:**
   - Book room for today
   - Verify room is immediately unavailable

3. **Future Booking:**
   - Book room for future date
   - Verify room shows as available until that date

#### 8.2.3 Reservation Status Transitions

**Test Cases:**

1. **Pending → Confirmed:**
   - Create reservation
   - Confirm payment
   - Verify status changes

2. **Confirmed → CheckedIn:**
   - Check-in guest
   - Verify status and room status change

3. **CheckedIn → CheckedOut:**
   - Check-out guest
   - Verify status and room availability

4. **Any Status → Cancelled:**
   - Cancel reservation
   - Verify room becomes available

### 8.3 Edge Cases

#### 8.3.1 Boundary Values

**Test Cases:**

1. **Date Boundaries:**
   - Check-in today, check-out today (same day)
   - Check-in in past (should fail)
   - Check-in 1 year in future

2. **Numeric Boundaries:**
   - Room capacity: 0, 1, 10, 100
   - Price: 0.01, 9999999.99
   - Number of guests: 0, 1, 100

3. **String Boundaries:**
   - Name: 1 character, 100 characters
   - Email: minimum valid, maximum length
   - Phone: various formats

#### 8.3.2 Special Characters

**Test Cases:**

1. **Names with Special Characters:**
   - O'Brien, José, François
   - Verify proper storage and display

2. **Addresses with Special Characters:**
   - Apt #123, Suite 4B
   - Verify proper handling

#### 8.3.3 Concurrent Operations

**Test Cases:**

1. **Two Users Book Same Room:**
   - Simulate simultaneous booking
   - Verify only one succeeds

2. **Update Conflicts:**
   - Two users edit same record
   - Verify conflict resolution

---

## 9. Troubleshooting Guide

### 9.1 Common Issues and Solutions

#### 9.1.1 API Issues

**Issue: API won't start**

**Symptoms:**
- Error: "Unable to start Kestrel"
- Port already in use

**Solutions:**
```bash
# Check if port is in use
sudo lsof -i :57679

# Kill process using port
sudo kill -9 <PID>

# Or use different port
dotnet run --urls "https://localhost:57680"
```

**Issue: Database connection failed**

**Symptoms:**
- Error: "Connection refused"
- Error: "Password authentication failed"

**Solutions:**
```bash
# Check PostgreSQL is running
sudo systemctl status postgresql

# Test connection
psql -h localhost -U innhotel_user -d innhotel_db

# Check connection string in appsettings.json
# Verify username, password, database name
```

**Issue: Migrations not applied**

**Symptoms:**
- Error: "Table does not exist"
- Database is empty

**Solutions:**
```bash
# Apply migrations
cd innhotel-api/src/InnHotel.Web
dotnet ef database update

# If migrations are corrupted
dotnet ef database drop --force
dotnet ef database update
```

**Issue: SSL certificate errors**

**Symptoms:**
- Error: "Unable to configure HTTPS endpoint"
- Certificate not trusted

**Solutions:**
```bash
# Clean and regenerate certificates
dotnet dev-certs https --clean
dotnet dev-certs https --trust

# Verify certificate
dotnet dev-certs https --check
```

#### 9.1.2 Client Issues

**Issue: Client won't start**

**Symptoms:**
- Error: "Cannot find module"
- Dependency errors

**Solutions:**
```bash
# Clear and reinstall dependencies
rm -rf node_modules package-lock.json
npm cache clean --force
npm install

# If still failing, check Node.js version
node --version  # Should be 20.x
```

**Issue: API connection failed**

**Symptoms:**
- Error: "Network Error"
- CORS errors in console

**Solutions:**
```bash
# Check API is running
curl -k https://localhost:57679/health

# Verify .env file
cat .env
# Should have: VITE_API_BASE_URL=https://localhost:57679/api

# Check CORS settings in API
# Verify ALLOWED_ORIGINS includes client URL
```

**Issue: Login not working**

**Symptoms:**
- Login button does nothing
- Console errors

**Solutions:**
```bash
# Check browser console for errors
# Verify API is accessible
# Check network tab for failed requests
# Verify credentials are correct
```

**Issue: White screen / blank page**

**Symptoms:**
- Page loads but shows nothing
- Console errors

**Solutions:**
```bash
# Check console for JavaScript errors
# Clear browser cache
# Rebuild client
npm run build
npm run dev:react
```

#### 9.1.3 Database Issues

**Issue: Database connection pool exhausted**

**Symptoms:**
- Error: "Timeout expired"
- Slow queries

**Solutions:**
```sql
-- Check active connections
SELECT count(*) FROM pg_stat_activity;

-- Kill idle connections
SELECT pg_terminate_backend(pid)
FROM pg_stat_activity
WHERE state = 'idle'
AND state_change < current_timestamp - INTERVAL '5 minutes';
```

**Issue: Slow queries**

**Solutions:**
```sql
-- Analyze query performance
EXPLAIN ANALYZE SELECT * FROM reservations WHERE status = 'Confirmed';

-- Create indexes if needed
CREATE INDEX idx_reservations_status ON reservations(status);
CREATE INDEX idx_reservations_dates ON reservations(check_in_date, check_out_date);
```

### 9.2 Diagnostic Scripts

#### 9.2.1 System Health Check Script

Create `health-check.sh`:

```bash
#!/bin/bash

echo "=== InnHotel System Health Check ==="
echo ""

# Check PostgreSQL
echo "1. Checking PostgreSQL..."
if systemctl is-active --quiet postgresql; then
    echo "   ✓ PostgreSQL is running"
    psql -h localhost -U innhotel_user -d innhotel_db -c "SELECT 1" > /dev/null 2>&1
    if [ $? -eq 0 ]; then
        echo "   ✓ Database connection successful"
    else
        echo "   ✗ Database connection failed"
    fi
else
    echo "   ✗ PostgreSQL is not running"
fi

# Check API
echo ""
echo "2. Checking API..."
API_RESPONSE=$(curl -k -s -o /dev/null -w "%{http_code}" https://localhost:57679/health)
if [ "$API_RESPONSE" = "200" ]; then
    echo "   ✓ API is running and healthy"
else
    echo "   ✗ API is not responding (HTTP $API_RESPONSE)"
fi

# Check Client
echo ""
echo "3. Checking Client..."
CLIENT_RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" http://localhost:5173)
if [ "$CLIENT_RESPONSE" = "200" ]; then
    echo "   ✓ Client is running"
else
    echo "   ✗ Client is not responding (HTTP $CLIENT_RESPONSE)"
fi

# Check disk space
echo ""
echo "4. Checking disk space..."
DISK_USAGE=$(df -h / | awk 'NR==2 {print $5}' | sed 's/%//')
if [ "$DISK_USAGE" -lt 90 ]; then
    echo "   ✓ Disk space OK ($DISK_USAGE% used)"
else
    echo "   ⚠ Disk space low ($DISK_USAGE% used)"
fi

# Check memory
echo ""
echo "5. Checking memory..."
MEM_USAGE=$(free | awk 'NR==2 {printf "%.0f", $3/$2 * 100}')
if [ "$MEM_USAGE" -lt 90 ]; then
    echo "   ✓ Memory OK ($MEM_USAGE% used)"
else
    echo "   ⚠ Memory high ($MEM_USAGE% used)"
fi

echo ""
echo "=== Health Check Complete ==="
```

Run with:
```bash
chmod +x health-check.sh
./health-check.sh
```

#### 9.2.2 Log Collection Script

Create `collect-logs.sh`:

```bash
#!/bin/bash

TIMESTAMP=$(date +%Y%m%d_%H%M%S)
LOG_DIR="logs_$TIMESTAMP"

mkdir -p "$LOG_DIR"

echo "Collecting logs..."

# API logs
if [ -f "innhotel-api/src/InnHotel.Web/log.txt" ]; then
    cp innhotel-api/src/InnHotel.Web/log*.txt "$LOG_DIR/"
    echo "✓ API logs collected"
fi

# PostgreSQL logs
sudo cp /var/log/postgresql/postgresql-*.log "$LOG_DIR/" 2>/dev/null
echo "✓ PostgreSQL logs collected"

# System logs
journalctl -u postgresql --since "1 hour ago" > "$LOG_DIR/postgresql-journal.log"
echo "✓ System logs collected"

# Create archive
tar -czf "logs_$TIMESTAMP.tar.gz" "$LOG_DIR"
rm -rf "$LOG_DIR"

echo "✓ Logs archived to logs_$TIMESTAMP.tar.gz"
```

### 9.3 Error Code Reference

| Error Code | Meaning | Common Causes | Solution |
|------------|---------|---------------|----------|
| 400 | Bad Request | Invalid input data | Check request body and validation |
| 401 | Unauthorized | Missing or invalid token | Login again |
| 403 | Forbidden | Insufficient permissions | Check user role |
| 404 | Not Found | Resource doesn't exist | Verify ID is correct |
| 409 | Conflict | Duplicate or conflicting data | Check for existing records |
| 500 | Internal Server Error | Server-side error | Check API logs |
| 503 | Service Unavailable | Database or service down | Check service status |

---

## 10. Defect Reporting & Resolution

### 10.1 Defect Reporting Template

When you find a bug, use this template:

```markdown
## Bug Report

**Bug ID:** BUG-001
**Date:** 2025-10-18
**Reporter:** [Your Name]
**Priority:** [Critical / High / Medium / Low]
**Status:** [New / In Progress / Resolved / Closed]

### Summary
Brief description of the issue

### Environment
- OS: Windows 10 / Ubuntu 22.04 / macOS
- Browser: Chrome 120.0
- API Version: 1.0.0
- Client Version: 1.0.0

### Steps to Reproduce
1. Step one
2. Step two
3. Step three

### Expected Behavior
What should happen

### Actual Behavior
What actually happens

### Screenshots
[Attach screenshots]

### Logs
```
[Paste relevant log entries]
```

### Additional Information
Any other relevant details
```

### 10.2 Bug Severity Levels

**Critical:**
- System crash or data loss
- Security vulnerability
- Complete feature failure
- **Action:** Fix immediately

**High:**
- Major feature not working
- Significant performance issue
- Affects multiple users
- **Action:** Fix within 24 hours

**Medium:**
- Minor feature issue
- Workaround available
- Affects some users
- **Action:** Fix within 1 week

**Low:**
- Cosmetic issue
- Minor inconvenience
- Rare occurrence
- **Action:** Fix when possible

### 10.3 Defect Tracking

Create a spreadsheet or use issue tracking tool:

| Bug ID | Summary | Severity | Status | Assigned To | Date Found | Date Resolved |
|--------|---------|----------|--------|-------------|------------|---------------|
| BUG-001 | Login fails with special characters | High | Resolved | Dev Team | 2025-10-18 | 2025-10-19 |
| BUG-002 | Dashboard stats not updating | Medium | In Progress | Dev Team | 2025-10-18 | - |

### 10.4 Resolution Verification

After a bug is marked as resolved:

1. **Verify the fix:**
   - Reproduce original issue
   - Confirm it's fixed
   - Test related functionality

2. **Regression testing:**
   - Ensure fix didn't break other features
   - Run related test cases

3. **Update documentation:**
   - Update test cases if needed
   - Document any workarounds

4. **Close the bug:**
   - Mark as "Verified" or "Closed"
   - Add resolution notes

---

## 11. Testing Completion Criteria

### 11.1 Completion Checklist

Testing is considered complete when:

**API Testing:**
- [ ] All authentication endpoints tested (login, logout, refresh)
- [ ] All CRUD operations tested for each entity
- [ ] All search endpoints tested
- [ ] Error handling verified for all endpoints
- [ ] Response times are acceptable (< 2 seconds)
- [ ] All status codes are correct
- [ ] API documentation is accurate

**Client Testing:**
- [ ] All pages load without errors
- [ ] All forms validate correctly
- [ ] All CRUD operations work through UI
- [ ] Navigation works correctly
- [ ] Responsive design verified
- [ ] Error messages are user-friendly
- [ ] Loading states display correctly

**Integration Testing:**
- [ ] End-to-end workflows complete successfully
- [ ] Real-time updates work (SignalR)
- [ ] Data consistency verified across layers
- [ ] Multi-user scenarios tested
- [ ] Performance is acceptable

**Security Testing:**
- [ ] Authentication is enforced
- [ ] Authorization checks work
- [ ] SQL injection prevented
- [ ] XSS prevented
- [ ] Sensitive data is protected

**Error Handling:**
- [ ] All error scenarios tested
- [ ] Error messages are appropriate
- [ ] Logging is working correctly
- [ ] System recovers from errors

**Documentation:**
- [ ] All test cases documented
- [ ] All bugs reported
- [ ] Test results recorded
- [ ] Known issues documented

### 11.2 Test Metrics

Track these metrics:

**Test Coverage:**
- Total test cases: ___
- Test cases passed: ___
- Test cases failed: ___
- Pass rate: ___% (target: > 95%)

**Defects:**
- Total bugs found: ___
- Critical bugs: ___
- High priority bugs: ___
- Medium priority bugs: ___
- Low priority bugs: ___
- Bugs resolved: ___
- Bugs remaining: ___

**Performance:**
- Average API response time: ___ ms
- Average page load time: ___ seconds
- Database query time: ___ ms

### 11.3 Sign-Off Criteria

The system is ready for production when:

1. **All critical and high-priority bugs are resolved**
2. **Test pass rate is > 95%**
3. **Performance meets requirements**
4. **Security testing is complete**
5. **Documentation is complete**
6. **Stakeholders have approved**

### 11.4 Final Test Report Template

```markdown
# InnHotel System - Final Test Report

**Date:** 2025-10-18
**Tester:** [Your Name]
**Version:** 1.0.0

## Executive Summary
Brief overview of testing results

## Test Coverage
- Total test cases executed: ___
- Pass rate: ___%
- Areas tested: API, Client, Integration, Security

## Defects Summary
- Total bugs found: ___
- Critical: ___
- High: ___
- Medium: ___
- Low: ___
- Resolved: ___

## Performance Results
- API response time: ___ ms (target: < 2000ms)
- Page load time: ___ seconds (target: < 3s)
- Database queries: ___ ms (target: < 200ms)

## Known Issues
List any remaining issues and workarounds

## Recommendations
- Suggestions for improvement
- Areas needing attention
- Future testing needs

## Conclusion
Overall assessment and sign-off recommendation

**Tested By:** _______________
**Date:** _______________
**Approved By:** _______________
**Date:** _______________
```

---

## 12. Appendices

### 12.1 Appendix A: API Endpoint Reference

Complete list of all API endpoints:

**Authentication:**
- POST /api/auth/login
- POST /api/auth/logout
- POST /api/auth/refresh
- POST /api/auth/register

**Branches:**
- GET /api/branches
- GET /api/branches/{id}
- POST /api/branches
- PUT /api/branches/{id}
- DELETE /api/branches/{id}

**Room Types:**
- GET /api/roomtypes
- GET /api/roomtypes/{id}
- POST /api/roomtypes
- PUT /api/roomtypes/{id}
- DELETE /api/roomtypes/{id}
- GET /api/roomtypes/search

**Rooms:**
- GET /api/rooms
- GET /api/rooms/{id}
- POST /api/rooms
- PUT /api/rooms/{id}
- DELETE /api/rooms/{id}
- GET /api/rooms/search

**Guests:**
- GET /api/guests
- GET /api/guests/{id}
- POST /api/guests
- PUT /api/guests/{id}
- DELETE /api/guests/{id}
- GET /api/guests/search

**Reservations:**
- GET /api/reservations
- GET /api/reservations/{id}
- POST /api/reservations
- PUT /api/reservations/{id}
- DELETE /api/reservations/{id}
- GET /api/reservations/search
- POST /api/reservations/{id}/checkin
- POST /api/reservations/{id}/checkout

**Employees:**
- GET /api/employees
- GET /api/employees/{id}
- POST /api/employees
- PUT /api/employees/{id}
- DELETE /api/employees/{id}
- GET /api/employees/search

**Services:**
- GET /api/services
- GET /api/services/{id}
- POST /api/services
- PUT /api/services/{id}
- DELETE /api/services/{id}
- GET /api/services/search

**Dashboard:**
- GET /api/dashboard/statistics

### 12.2 Appendix B: Test Data

**Default Users:**
```json
{
  "superAdmin": {
    "email": "super@innhotel.com",
    "password": "Sup3rP@ssword!",
    "role": "SuperAdmin"
  },
  "admin": {
    "email": "admin@innhotel.com",
    "password": "Adm1nP@ssword!",
    "role": "Admin"
  }
}
```

**Sample Branch:**
```json
{
  "name": "Downtown Branch",
  "address": "123 Main Street, City, State 12345",
  "phoneNumber": "+1-555-0100",
  "email": "downtown@innhotel.com"
}
```

**Sample Room Type:**
```json
{
  "name": "Deluxe Suite",
  "description": "Spacious suite with king bed",
  "basePrice": 299.99,
  "capacity": 2,
  "amenities": ["WiFi", "TV", "Mini Bar"]
}
```

**Sample Guest:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "+1-555-0123",
  "address": "789 Guest Street",
  "dateOfBirth": "1990-01-15",
  "idNumber": "ID123456789"
}
```

### 12.3 Appendix C: Useful Commands

**PostgreSQL:**
```bash
# Connect to database
psql -h localhost -U innhotel_user -d innhotel_db

# List tables
\dt

# Describe table
\d table_name

# Run query
SELECT * FROM branches;

# Exit
\q
```

**API:**
```bash
# Run API
cd innhotel-api/src/InnHotel.Web
dotnet run

# Build API
dotnet build

# Run tests
dotnet test

# Apply migrations
dotnet ef database update

# Create migration
dotnet ef migrations add MigrationName
```

**Client:**
```bash
# Run development server
npm run dev:react

# Build for production
npm run build

# Run Electron app
npm run dev

# Build Electron app
npm run dist:win  # Windows
npm run dist:mac  # macOS
npm run dist:linux  # Linux
```

**Docker:**
```bash
# Start all services
docker-compose up -d

# Stop all services
docker-compose down

# View logs
docker-compose logs -f

# Restart service
docker-compose restart api
```

### 12.4 Appendix D: Glossary

**Terms:**
- **API:** Application Programming Interface
- **CRUD:** Create, Read, Update, Delete
- **JWT:** JSON Web Token
- **CORS:** Cross-Origin Resource Sharing
- **ORM:** Object-Relational Mapping
- **EF Core:** Entity Framework Core
- **SignalR:** Real-time communication library
- **Swagger:** API documentation tool
- **Postman:** API testing tool

### 12.5 Appendix E: Resources

**Documentation:**
- .NET Documentation: https://docs.microsoft.com/dotnet/
- React Documentation: https://react.dev/
- PostgreSQL Documentation: https://www.postgresql.org/docs/
- Electron Documentation: https://www.electronjs.org/docs/

**Tools:**
- Postman: https://www.postman.com/
- pgAdmin: https://www.pgadmin.org/
- Visual Studio Code: https://code.visualstudio.com/

**Support:**
- GitHub Repository: https://github.com/HazemSoftPro/HotelTransylvania
- Issue Tracker: [Repository Issues]

---

## Document Information

**Version:** 1.0
**Last Updated:** 2025-10-18
**Author:** QA Team
**Status:** Final

---

**End of QA Testing Guide**