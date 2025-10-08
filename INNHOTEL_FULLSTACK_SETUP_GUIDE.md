# InnHotel Full-Stack Setup Guide
## Complete Step-by-Step Tutorial for API & Desktop Client Integration

### Table of Contents
1. [Prerequisites](#prerequisites)
2. [System Requirements](#system-requirements)
3. [Database Setup](#database-setup)
4. [API Configuration](#api-configuration)
5. [Desktop Client Setup](#desktop-client-setup)
6. [Integration & Testing](#integration--testing)
7. [Security Configuration](#security-configuration)
8. [Troubleshooting](#troubleshooting)
9. [Production Deployment](#production-deployment)

---

## Prerequisites

### Required Software
- **.NET 9 SDK** (Latest version)
- **Node.js 20.x LTS** or higher
- **PostgreSQL 15.x** or higher
- **Git** (for repository cloning)
- **Visual Studio 2022** (recommended) or **Visual Studio Code**
- **Postman** (for API testing)

### System Requirements
- **Operating System**: Windows 10/11, macOS 12+, or Linux (Ubuntu 20.04+)
- **RAM**: 8GB minimum, 16GB recommended
- **Storage**: 5GB free space for repositories and dependencies
- **Network**: Internet connection for package downloads

### Development Environment Assumptions
- **OS**: This guide assumes Windows 10/11 or Ubuntu 22.04
- **Shell**: PowerShell (Windows) or Bash (Linux/macOS)
- **Package Managers**: npm (Node.js), dotnet (.NET), apt/yum/brew (system packages)

---

## System Requirements Installation

### 1. Install .NET 9 SDK

#### Windows
```powershell
# Download from https://dotnet.microsoft.com/download/dotnet/9.0
# Or use winget
winget install Microsoft.DotNet.SDK.9
```

#### Linux (Ubuntu)
```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET 9
sudo apt-get update
sudo apt-get install -y dotnet-sdk-9.0
```

#### macOS
```bash
# Using Homebrew
brew install dotnet-sdk
```

### 2. Install Node.js 20.x LTS

#### Windows
```powershell
# Using winget
winget install OpenJS.NodeJS.LTS

# Or download from https://nodejs.org/
```

#### Linux (Ubuntu)
```bash
# Using NodeSource repository
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt-get install -y nodejs
```

#### macOS
```bash
# Using Homebrew
brew install node@20
```

### 3. Install PostgreSQL

#### Windows
```powershell
# Download from https://www.postgresql.org/download/windows/
# Or use winget
winget install PostgreSQL.PostgreSQL
```

#### Linux (Ubuntu)
```bash
# Install PostgreSQL
sudo apt update
sudo apt install -y postgresql postgresql-contrib

# Start and enable PostgreSQL
sudo systemctl start postgresql
sudo systemctl enable postgresql
```

#### macOS
```bash
# Using Homebrew
brew install postgresql
brew services start postgresql
```

### 4. Verify Installations
```bash
# Check .NET
dotnet --version

# Check Node.js and npm
node --version
npm --version

# Check PostgreSQL
psql --version
```

---

## Database Setup

### 1. PostgreSQL Configuration

#### Create Database and User
```sql
-- Connect to PostgreSQL as superuser
sudo -u postgres psql

-- Create database
CREATE DATABASE innhotel_db;

-- Create user with secure password
CREATE USER innhotel_user WITH PASSWORD 'innhotel_secure_password_2024';

-- Grant privileges
GRANT ALL PRIVILEGES ON DATABASE innhotel_db TO innhotel_user;

-- Exit PostgreSQL
\q
```

#### Configure PostgreSQL for Remote Connections (Optional)
```bash
# Edit postgresql.conf
sudo nano /etc/postgresql/15/main/postgresql.conf

# Add or modify:
listen_addresses = 'localhost,*'

# Edit pg_hba.conf
sudo nano /etc/postgresql/15/main/pg_hba.conf

# Add:
host    innhotel_db    innhotel_user    0.0.0.0/0    md5
```

### 2. Database Schema Design

The InnHotel system uses the following database schema:

#### Core Entities
- **Branches**: Hotel branch management
- **Rooms**: Room inventory and types
- **Guests**: Customer information
- **Reservations**: Booking management
- **Employees**: Staff management
- **Services**: Additional hotel services
- **ApplicationUsers**: Authentication system
- **RefreshTokens**: Token management

#### Entity Relationships
```
Branches 1---* Rooms
Branches 1---* Employees
Guests 1---* Reservations
Reservations *---* Rooms (via ReservationRooms)
Reservations *---* Services (via ReservationServices)
```

### 3. Database Migration

#### Run Entity Framework Migrations
```bash
# Navigate to API project
cd innhotel-api/src/InnHotel.Web

# Update database
dotnet ef database update --project ../InnHotel.Infrastructure/InnHotel.Infrastructure.csproj

# If migrations don't exist, create initial migration
dotnet ef migrations add InitialCreate --project ../InnHotel.Infrastructure/InnHotel.Infrastructure.csproj
```

---

## API Configuration

### 1. Environment Configuration

#### Create .env File
```bash
# Copy example environment file
cd innhotel-api
cp .env.example .env

# Edit .env file
nano .env
```

#### Environment Variables
```bash
# Database Connection
ConnectionStrings__PostgreSQLConnection=Host=localhost;Port=5432;Username=innhotel_user;Password=innhotel_secure_password_2024;Database=innhotel_db

# CORS Configuration
ALLOWED_ORIGINS=http://localhost:3000,http://localhost:5173,https://localhost:5173

# JWT Configuration (Add these)
JWT__SecretKey=your-super-secret-key-here-must-be-32-characters
JWT__Issuer=InnHotelAPI
JWT__Audience=InnHotelClient
JWT__ExpirationMinutes=1440
```

### 2. Authentication & Authorization Setup

#### JWT Configuration
The API uses JWT tokens for authentication. Configuration is handled in `appsettings.json`:

```json
{
  "Jwt": {
    "SecretKey": "your-super-secret-key-here-must-be-32-characters",
    "Issuer": "InnHotelAPI",
    "Audience": "InnHotelClient",
    "ExpirationMinutes": 1440
  }
}
```

#### User Registration & Login
The system provides these authentication endpoints:
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Token refresh
- `POST /api/auth/logout` - User logout

### 3. Launch Settings

#### Configure Launch URLs
```json
// src/InnHotel.Web/Properties/launchSettings.json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:57679",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### 4. Start the API

#### Development Mode
```bash
# Navigate to API project
cd innhotel-api/src/InnHotel.Web

# Run with hot reload
dotnet watch run

# Or run directly
dotnet run --urls "https://localhost:57679"
```

#### Verify API is Running
- **Swagger UI**: https://localhost:57679/swagger
- **Health Check**: https://localhost:57679/health

---

## Desktop Client Setup

### 1. Environment Configuration

#### Create .env File
```bash
# Navigate to client project
cd innhotel-desktop-client

# Copy example environment file
cp .env.example .env

# Edit .env file
nano .env
```

#### Environment Variables
```bash
# Development
NODE_ENV=development
VITE_API_BASE_URL="https://localhost:57679"
VITE_ENABLE_LOGGING=true

# Production (update for deployment)
NODE_ENV=production
VITE_API_BASE_URL="https://your-api-domain.com"
VITE_ENABLE_LOGGING=false
```

### 2. Install Dependencies

```bash
# Navigate to client project
cd innhotel-desktop-client

# Install all dependencies
npm install

# Install specific dependencies if needed
npm install axios @types/node --save-dev
```

### 3. Development Server

#### Start Development Mode
```bash
# Start both React and Electron
npm run dev

# Or start separately
npm run dev:react  # Starts Vite server
npm run dev:electron  # Starts Electron app
```

#### Verify Client is Running
- **Electron App**: Will open automatically
- **React Dev Server**: http://localhost:5173 (for browser testing)

### 4. Build for Production

#### Windows Build
```bash
# Build Windows executable
npm run dist:win

# Output locations:
# - dist/win-unpacked/innhotel-desktop-client.exe (portable)
# - dist/innhotel-desktop-client Setup 0.0.0.exe (installer)
```

#### Cross-Platform Builds
```bash
# macOS
npm run dist:mac

# Linux
npm run dist:linux
```

---

## Integration & Testing

### 1. API Integration Testing

#### Test Authentication Flow
```bash
# Test registration
curl -X POST https://localhost:57679/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@innhotel.com",
    "password": "Admin@123",
    "confirmPassword": "Admin@123"
  }'

# Test login
curl -X POST https://localhost:57679/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@innhotel.com",
    "password": "Admin@123"
  }'
```

#### Test API Endpoints
```bash
# Test branches endpoint
curl -X GET https://localhost:57679/api/branches \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Test rooms endpoint
curl -X GET https://localhost:57679/api/rooms \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 2. Client-Side Integration

#### API Service Configuration
The client uses Axios for API calls with automatic token handling:

```typescript
// src/services/api.ts
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor for auth
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

#### Test Client Integration
1. Open the desktop client
2. Navigate to login page
3. Use credentials created during API testing
4. Verify data loads correctly

### 3. End-to-End Testing Checklist

#### Database Connection
- [ ] PostgreSQL is running
- [ ] Database `innhotel_db` exists
- [ ] User `innhotel_user` has correct permissions
- [ ] Migrations are applied successfully

#### API Testing
- [ ] API starts without errors
- [ ] Swagger UI is accessible
- [ ] Authentication endpoints work
- [ ] Protected endpoints require authentication
- [ ] CORS allows client connections

#### Client Testing
- [ ] Client starts without errors
- [ ] Client connects to API
- [ ] Login functionality works
- [ ] Data loads from API
- [ ] Forms submit correctly

---

## Security Configuration

### 1. Development Security

#### HTTPS Certificates
```bash
# Trust development certificates (Windows)
dotnet dev-certs https --trust

# Trust development certificates (Linux/macOS)
dotnet dev-certs https
```

#### Environment Variables Security
```bash
# Never commit sensitive data
echo ".env" >> .gitignore
echo "appsettings.Development.json" >> .gitignore
```

### 2. Production Security

#### API Security Checklist
- [ ] Use HTTPS in production
- [ ] Configure CORS for production domains only
- [ ] Use strong JWT secrets
- [ ] Implement rate limiting
- [ ] Use environment variables for secrets
- [ ] Enable request logging

#### Client Security Checklist
- [ ] Use production API URLs
- [ ] Disable development logging
- [ ] Implement input validation
- [ ] Use secure storage for tokens
- [ ] Enable application updates

### 3. Database Security

#### Production Database Setup
```sql
-- Create production user with limited privileges
CREATE USER innhotel_app WITH PASSWORD 'secure_production_password';
GRANT CONNECT ON DATABASE innhotel_db TO innhotel_app;
GRANT USAGE ON SCHEMA public TO innhotel_app;
GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO innhotel_app;
```

---

## Troubleshooting

### Common Issues and Solutions

#### 1. PostgreSQL Connection Issues

**Problem**: Cannot connect to PostgreSQL
```bash
# Error: connection refused
```

**Solution**:
```bash
# Check PostgreSQL status
sudo systemctl status postgresql

# Restart PostgreSQL
sudo systemctl restart postgresql

# Check if PostgreSQL is listening
sudo netstat -plntu | grep postgres
```

#### 2. .NET EF Migration Issues

**Problem**: Migrations fail to apply
```bash
# Error: relation does not exist
```

**Solution**:
```bash
# Reset database
dotnet ef database drop --force
dotnet ef database update

# Or create new migration
dotnet ef migrations add FixMigration --project ../InnHotel.Infrastructure/InnHotel.Infrastructure.csproj
dotnet ef database update
```

#### 3. CORS Issues

**Problem**: Client cannot connect to API
```bash
# Error: CORS policy error
```

**Solution**:
```bash
# Update .env file with correct URLs
ALLOWED_ORIGINS=http://localhost:3000,http://localhost:5173,https://localhost:5173
```

#### 4. Certificate Issues

**Problem**: HTTPS certificate errors
```bash
# Error: certificate not trusted
```

**Solution**:
```bash
# Regenerate certificates
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

#### 5. Node.js Module Issues

**Problem**: npm install fails
```bash
# Error: node-gyp rebuild fails
```

**Solution**:
```bash
# Clear npm cache
npm cache clean --force

# Delete node_modules and reinstall
rm -rf node_modules package-lock.json
npm install
```

### Debug Mode

#### Enable Detailed Logging
```bash
# API logging
export ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_URLS="https://localhost:57679"

# Client logging
export VITE_ENABLE_LOGGING=true
```

#### Check Logs
```bash
# API logs
cd innhotel-api/src/InnHotel.Web
dotnet run > api.log 2>&1 &

# Client logs
cd innhotel-desktop-client
npm run dev > client.log 2>&1 &
```

---

## Production Deployment

### 1. API Deployment

#### Docker Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/InnHotel.Web/InnHotel.Web.csproj", "src/InnHotel.Web/"]
COPY ["src/InnHotel.Core/InnHotel.Core.csproj", "src/InnHotel.Core/"]
COPY ["src/InnHotel.Infrastructure/InnHotel.Infrastructure.csproj", "src/InnHotel.Infrastructure/"]
COPY ["src/InnHotel.UseCases/InnHotel.UseCases.csproj", "src/InnHotel.UseCases/"]
RUN dotnet restore "src/InnHotel.Web/InnHotel.Web.csproj"
COPY . .
WORKDIR "/src/src/InnHotel.Web"
RUN dotnet build "InnHotel.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InnHotel.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnHotel.Web.dll"]
```

#### Environment Variables for Production
```bash
# Production .env
ConnectionStrings__PostgreSQLConnection=Host=prod-db-host;Port=5432;Username=prod_user;Password=secure_password;Database=innhotel_prod
ALLOWED_ORIGINS=https://yourdomain.com
JWT__SecretKey=production-secret-key-32-chars-minimum
ASPNETCORE_ENVIRONMENT=Production
```

### 2. Client Deployment

#### Build Configuration
```bash
# Production build
cd innhotel-desktop-client
npm run build

# Update environment variables for production
VITE_API_BASE_URL="https://api.yourdomain.com"
VITE_ENABLE_LOGGING=false
```

#### Distribution
- Upload to GitHub Releases
- Use auto-updater
- Code sign executables

---

## Quick Start Script

For immediate setup, use the provided quick-start script:

```bash
# Make script executable
chmod +x quick-start.sh

# Run quick start
./quick-start.sh
```

---

## Maintenance Guide

### 1. Regular Updates

#### Update Dependencies
```bash
# Update .NET packages
cd innhotel-api
dotnet list package --outdated
dotnet update

# Update Node.js packages
cd innhotel-desktop-client
npm update
npm audit fix
```

#### Database Maintenance
```sql
-- Regular cleanup
VACUUM ANALYZE;
REINDEX DATABASE innhotel_db;

-- Backup database
pg_dump -U innhotel_user -h localhost innhotel_db > backup_$(date +%Y%m%d).sql
```

### 2. Monitoring

#### Health Checks
```bash
# API health check
curl -f https://localhost:57679/health || echo "API is down"

# Database connection check
pg_isready -h localhost -p 5432
```

### 3. Backup Procedures

#### Database Backup
```bash
# Automated backup script
#!/bin/bash
DATE=$(date +%Y%m%d_%H%M%S)
pg_dump -U innhotel_user -h localhost innhotel_db > "backup_${DATE}.sql"
```

---

## Support and Resources

### Documentation Links
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Electron Documentation](https://electronjs.org/docs)
- [React Documentation](https://reactjs.org/docs)

### Community Support
- GitHub Issues: Report bugs on respective repositories
- Discord: Join the InnHotel development community
- Stack Overflow: Tag questions with #innhotel

---

## Summary

This guide provides a complete setup process for the InnHotel full-stack system. By following these steps, you should have:

1. ✅ PostgreSQL database with proper schema
2. ✅ .NET 9 API running on https://localhost:57679
3. ✅ Electron desktop client connected to the API
4. ✅ Authentication system working
5. ✅ All endpoints accessible and tested

The system is now ready for development or production deployment based on your configuration.

### Next Steps
1. Customize the database seed data
2. Add your hotel-specific configurations
3. Set up CI/CD pipelines
4. Configure monitoring and logging
5. Implement additional features as needed

Happy coding! 🚀