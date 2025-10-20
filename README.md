# 🏨 HotelTransylvania - Complete Hotel Management System

## 📋 Table of Contents
- [Introduction](#introduction)
- [Features](#features)
- [System Architecture](#system-architecture)
- [Technology Stack](#technology-stack)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Development](#development)
- [Testing](#testing)
- [Deployment](#deployment)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)
- [Support](#support)

## 🎯 Introduction

HotelTransylvania is a comprehensive, full-stack hotel management system designed to streamline hotel operations, enhance guest experiences, and provide powerful analytics for data-driven decision making. Built with modern technologies and following best practices in software architecture, this system provides a complete solution for hotel management from reservations to reporting.

The system consists of a robust .NET 9 API backend with Entity Framework Core, a modern React-based desktop client built with Electron, and a PostgreSQL database for reliable data storage. The architecture follows Domain-Driven Design principles and implements Clean Architecture patterns for maintainability and scalability.

## ✨ Features

### Core Hotel Management Features
- **Reservation Management**: Complete booking system with availability checking, reservation creation, modification, and cancellation
- **Guest Management**: Comprehensive guest profiles, history tracking, and preference management
- **Room Management**: Room inventory, status tracking, maintenance scheduling, and room type categorization
- **Rate Management**: Dynamic pricing, seasonal rates, and special offer management
- **Check-in/Check-out**: Streamlined front desk operations with automated processes

### Advanced Features
- **Search & Filter**: Advanced search capabilities across all entities with multiple filter criteria
- **Analytics Dashboard**: Real-time metrics, occupancy rates, revenue tracking, and guest analytics
- **Reporting System**: Comprehensive reports including occupancy, revenue, guest demographics, and operational metrics
- **Multi-property Support**: Manage multiple hotel properties from a single interface
- **User Management**: Role-based access control with different permission levels

### User Experience Features
- **Responsive Design**: Modern, intuitive interface optimized for desktop use
- **Keyboard Shortcuts**: Efficient navigation and operation shortcuts
- **Breadcrumbs**: Clear navigation path indication
- **Real-time Updates**: Live data synchronization using SignalR
- **Offline Capabilities**: Limited offline functionality with data synchronization

### Technical Features
- **RESTful API**: Well-documented, scalable API architecture
- **Real-time Communication**: SignalR integration for live updates
- **Database Transactions**: ACID compliance for data integrity
- **Caching**: Performance optimization with strategic caching
- **Logging**: Comprehensive logging and monitoring capabilities
- **Security**: JWT authentication, data encryption, and secure communication

## 🏗️ System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                    HotelTransylvania System                     │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────────┐    ┌──────────────────────────────┐  │
│  │   Desktop Client    │    │        Web Dashboard         │  │
│  │  (Electron/React)   │    │      (Future Enhancement)    │  │
│  └──────────┬──────────┘    └──────────────┬───────────────┘  │
│             │                                │                   │
│             └──────────────┬─────────────────┘                   │
│                            │                                     │
│  ┌─────────────────────────▼───────────────────────────────┐    │
│  │                   API Gateway                           │    │
│  │              (.NET 9 Web API)                           │    │
│  ├─────────────────────────┬───────────────────────────────┤    │
│  │                         │                               │    │
│  │  ┌──────────┐  ┌───────▼────────┐  ┌──────────────┐  │    │
│  │  │Use Cases │  │Domain Services │  │Infrastructure│  │    │
│  │  │  Layer   │  │     Layer      │  │    Layer     │  │    │
│  │  └──────────┘  └────────────────┘  └──────────────┘  │    │
│  └─────────────────────────┬───────────────────────────────┘    │
│                            │                                     │
│  ┌─────────────────────────▼───────────────────────────────┐    │
│  │              PostgreSQL Database                        │    │
│  │              (Primary Data Store)                       │    │
│  └─────────────────────────────────────────────────────────┘    │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Clean Architecture Layers

1. **Presentation Layer** (Desktop Client)
   - React components with TypeScript
   - Electron wrapper for desktop deployment
   - State management with Zustand
   - Form validation with React Hook Form and Zod

2. **Application Layer** (API)
   - RESTful controllers
   - Use case orchestration
   - Input validation and sanitization
   - Response formatting and error handling

3. **Domain Layer** (Core Business Logic)
   - Domain entities and value objects
   - Business rules and constraints
   - Domain events and services
   - Repository interfaces

4. **Infrastructure Layer** (Data Access)
   - Entity Framework Core implementation
   - Repository pattern implementation
   - External service integrations
   - Caching and logging services

## 🛠️ Technology Stack

### Backend Technologies
- **.NET 9**: Latest version of Microsoft's cross-platform framework
- **ASP.NET Core**: High-performance web framework
- **Entity Framework Core**: Modern object-database mapper
- **PostgreSQL**: Advanced open-source relational database
- **SignalR**: Real-time web functionality
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Input validation library
- **Serilog**: Structured logging framework
- **JWT**: JSON Web Token authentication
- **Swagger/OpenAPI**: API documentation and testing

### Frontend Technologies
- **React 19**: Modern UI library with latest features
- **TypeScript**: Type-safe JavaScript development
- **Electron**: Cross-platform desktop application framework
- **Vite**: Fast build tool and development server
- **Tailwind CSS**: Utility-first CSS framework
- **Radix UI**: Unstyled, accessible UI components
- **React Router**: Client-side routing
- **React Hook Form**: Performant form handling
- **Zod**: Schema validation
- **Zustand**: Lightweight state management
- **Axios**: HTTP client for API communication
- **Lucide React**: Beautiful icon library

### Development Tools
- **Docker**: Containerization for consistent deployment
- **GitHub Actions**: Continuous integration and deployment
- **ESLint**: JavaScript/TypeScript linting
- **Prettier**: Code formatting
- **Husky**: Git hooks for code quality
- **Playwright**: End-to-end testing framework

## 🚀 Installation

### Prerequisites

Before installing HotelTransylvania, ensure you have the following software installed on your system:

- **Operating System**: Windows 10+, macOS 10.15+, or Ubuntu 20.04+
- **.NET 9 SDK**: [Download from Microsoft](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Node.js 20.x LTS**: [Download from Node.js](https://nodejs.org/)
- **PostgreSQL 15.x**: [Download from PostgreSQL](https://www.postgresql.org/download/)
- **Git**: [Download from Git](https://git-scm.com/downloads)
- **8GB+ RAM**: Recommended for optimal performance
- **5GB+ Free Disk Space**: For installation and data storage

### Quick Start (Automated Installation)

1. **Clone the Repository**
   ```bash
   git clone https://github.com/HazemSoftPro/HotelTransylvania.git
   cd HotelTransylvania
   ```

2. **Run the Automated Setup Script**
   ```bash
   # Make the script executable (Linux/macOS)
   chmod +x quick-start.sh
   
   # Run the automated setup
   ./quick-start.sh
   ```

3. **Follow the Interactive Setup**
   - The script will guide you through the installation process
   - It will automatically install dependencies, set up the database, and configure the environment

### Manual Installation

#### Step 1: Database Setup

1. **Create PostgreSQL Database**
   ```sql
   -- Connect to PostgreSQL as superuser
   psql -U postgres
   
   -- Create database and user
   CREATE DATABASE innhotel_db;
   CREATE USER innhotel_user WITH PASSWORD 'your_secure_password';
   GRANT ALL PRIVILEGES ON DATABASE innhotel_db TO innhotel_user;
   \q
   ```

2. **Run Database Migration Script**
   ```bash
   # Navigate to the API directory
   cd innhotel-api
   
   # Apply database migrations
   dotnet ef database update
   ```

#### Step 2: API Setup

1. **Navigate to API Directory**
   ```bash
   cd innhotel-api
   ```

2. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

3. **Build the API**
   ```bash
   dotnet build
   ```

4. **Configure Environment Variables**
   Create a `.env` file in the `innhotel-api/src/InnHotel.Web` directory:
   ```env
   ConnectionStrings__PostgreSQLConnection=Host=localhost;Port=5432;Username=innhotel_user;Password=your_secure_password;Database=innhotel_db
   ALLOWED_ORIGINS=http://localhost:3000,http://localhost:5173,https://localhost:5173
   JWT_SECRET=your_jwt_secret_key_here
   JWT_EXPIRATION_HOURS=24
   ```

#### Step 3: Desktop Client Setup

1. **Navigate to Client Directory**
   ```bash
   cd innhotel-desktop-client
   ```

2. **Install Dependencies**
   ```bash
   npm install
   ```

3. **Configure Environment Variables**
   Create a `.env` file in the client directory:
   ```env
   VITE_API_BASE_URL="https://localhost:57679"
   NODE_ENV=development
   ```

### Docker Installation

For containerized deployment, use the provided Docker configuration:

1. **Start Docker Services**
   ```bash
   docker-compose up -d
   ```

2. **Access Services**
   - API: `https://localhost:57679`
   - PostgreSQL: `localhost:5432`
   - Client: Electron app (run separately)

## ⚙️ Configuration

### API Configuration

The API uses a hierarchical configuration system with the following sources:

1. **appsettings.json**: Base configuration
2. **appsettings.Development.json**: Development-specific settings
3. **Environment Variables**: Override all other settings

#### Key Configuration Sections

```json
{
  "ConnectionStrings": {
    "PostgreSQLConnection": "Host=localhost;Port=5432;Database=innhotel_db;Username=innhotel_user;Password=your_password"
  },
  "JwtSettings": {
    "Secret": "your_secret_key",
    "ExpirationHours": 24,
    "Issuer": "HotelTransylvania",
    "Audience": "HotelTransylvania-Users"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AllowedOrigins": ["http://localhost:3000", "http://localhost:5173"]
}
```

### Client Configuration

The desktop client configuration is managed through environment variables:

```env
# API Configuration
VITE_API_BASE_URL="https://localhost:57679"
VITE_API_TIMEOUT=30000

# Application Settings
VITE_APP_NAME="HotelTransylvania"
VITE_APP_VERSION="1.0.0"
VITE_ENVIRONMENT="development"

# Feature Flags
VITE_ENABLE_ANALYTICS=true
VITE_ENABLE_OFFLINE_MODE=true
```

### Database Configuration

PostgreSQL configuration should be optimized for your environment:

```sql
-- Performance optimization settings
ALTER SYSTEM SET shared_buffers = '256MB';
ALTER SYSTEM SET effective_cache_size = '1GB';
ALTER SYSTEM SET maintenance_work_mem = '64MB';
ALTER SYSTEM SET checkpoint_completion_target = 0.9;
ALTER SYSTEM SET wal_buffers = '16MB';
ALTER SYSTEM SET default_statistics_target = 100;
```

## 🎮 Usage

### Starting the Application

1. **Start the API Server**
   ```bash
   cd innhotel-api/src/InnHotel.Web
   dotnet run
   ```

2. **Start the Desktop Client**
   ```bash
   cd innhotel-desktop-client
   npm run dev
   ```

3. **Access the Application**
   - Desktop Client: Electron app will open automatically
   - API Swagger Documentation: `https://localhost:57679/swagger`
   - Health Check: `https://localhost:57679/health`

### Basic Operations

#### Creating a Reservation

1. **Navigate to Reservations Module**
   - Click on "Reservations" in the main navigation
   - Click "New Reservation" button

2. **Fill in Guest Information**
   - Enter guest details (name, contact information)
   - Select check-in and check-out dates
   - Choose room type and specific room

3. **Complete Reservation**
   - Review reservation details
   - Add any special requests or notes
   - Confirm booking

#### Managing Rooms

1. **View Room Status**
   - Navigate to "Rooms" module
   - View real-time room status dashboard
   - Filter by availability, type, or status

2. **Update Room Information**
   - Select a room from the list
   - Edit room details (type, amenities, pricing)
   - Update maintenance status

#### Guest Management

1. **Search for Guests**
   - Use the search bar with name, email, or phone
   - Apply filters for check-in dates, room preferences
   - View guest history and preferences

2. **Update Guest Profile**
   - Select guest from search results
   - Edit contact information
   - Add notes or special requirements

### Advanced Features

#### Analytics Dashboard

Access comprehensive analytics through the dashboard:

- **Occupancy Rates**: Real-time and historical occupancy data
- **Revenue Analysis**: Daily, monthly, and yearly revenue trends
- **Guest Demographics**: Visitor origin, preferences, and behavior
- **Performance Metrics**: Key performance indicators and trends

#### Reports Generation

Generate detailed reports:

1. **Select Report Type**
   - Occupancy reports
   - Revenue reports
   - Guest demographic reports
   - Operational efficiency reports

2. **Configure Parameters**
   - Date range selection
   - Property selection (for multi-property)
   - Filter criteria

3. **Export Options**
   - PDF format for sharing
   - Excel format for analysis
   - CSV format for data processing

## 📚 API Documentation

### Authentication

All API endpoints (except authentication) require a valid JWT token:

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin@hotel.com",
  "password": "your_password"
}
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2024-12-31T23:59:59Z",
  "user": {
    "id": "123",
    "username": "admin@hotel.com",
    "roles": ["Admin", "Manager"]
  }
}
```

Include the token in subsequent requests:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Key Endpoints

#### Reservations API
- `GET /api/reservations` - List all reservations
- `POST /api/reservations` - Create new reservation
- `GET /api/reservations/{id}` - Get reservation details
- `PUT /api/reservations/{id}` - Update reservation
- `DELETE /api/reservations/{id}` - Cancel reservation

#### Rooms API
- `GET /api/rooms` - List all rooms
- `POST /api/rooms` - Add new room
- `GET /api/rooms/{id}` - Get room details
- `PUT /api/rooms/{id}` - Update room information
- `GET /api/rooms/available` - Get available rooms

#### Guests API
- `GET /api/guests` - List all guests
- `POST /api/guests` - Register new guest
- `GET /api/guests/{id}` - Get guest details
- `PUT /api/guests/{id}` - Update guest information
- `GET /api/guests/search` - Search guests

For complete API documentation, visit the Swagger UI at `https://localhost:57679/swagger` when the API is running.

## 🔧 Development

### Setting Up Development Environment

1. **Install Development Tools**
   - Visual Studio 2022 or VS Code with C# extension
   - Node.js and npm
   - PostgreSQL management tool (pgAdmin or DBeaver)
   - Git client

2. **Clone and Setup**
   ```bash
   git clone https://github.com/HazemSoftPro/HotelTransylvania.git
   cd HotelTransylvania
   ./quick-start.sh
   ```

3. **Development Workflow**
   - Create feature branches from `main`
   - Follow commit message conventions
   - Write unit tests for new features
   - Run integration tests before committing

### Code Structure

```
HotelTransylvania/
├── innhotel-api/
│   ├── src/
│   │   ├── InnHotel.Core/          # Domain entities and interfaces
│   │   ├── InnHotel.UseCases/      # Application logic and use cases
│   │   ├── InnHotel.Infrastructure/# Data access and external services
│   │   └── InnHotel.Web/           # API controllers and configuration
│   └── tests/                      # Unit, integration, and functional tests
├── innhotel-desktop-client/
│   ├── src/
│   │   ├── components/             # Reusable UI components
│   │   ├── pages/                  # Application pages
│   │   ├── services/               # API communication services
│   │   ├── store/                  # State management
│   │   └── electron/               # Electron main process
│   └── public/                     # Static assets
└── docs/                           # Documentation and guides
```

### Coding Standards

#### C# Guidelines
- Follow Microsoft C# coding conventions
- Use dependency injection for all services
- Implement async/await for I/O operations
- Write XML documentation for public APIs
- Use meaningful variable and method names

#### TypeScript/JavaScript Guidelines
- Follow Airbnb JavaScript style guide
- Use TypeScript for type safety
- Implement proper error handling
- Write JSDoc comments for functions
- Use functional components with React Hooks

### Database Development

#### Entity Framework Migrations

1. **Create Migration**
   ```bash
   cd innhotel-api/src/InnHotel.Web
   dotnet ef migrations add MigrationName --startup-project . --project ../InnHotel.Infrastructure
   ```

2. **Apply Migration**
   ```bash
   dotnet ef database update --startup-project . --project ../InnHotel.Infrastructure
   ```

3. **Revert Migration**
   ```bash
   dotnet ef database update PreviousMigration --startup-project . --project ../InnHotel.Infrastructure
   ```

## 🧪 Testing

### Running Tests

#### Backend Tests
```bash
cd innhotel-api

# Run all tests
dotnet test

# Run unit tests only
dotnet test tests/InnHotel.UnitTests

# Run integration tests
dotnet test tests/InnHotel.IntegrationTests

# Run functional tests
dotnet test tests/InnHotel.FunctionalTests
```

#### Frontend Tests
```bash
cd innhotel-desktop-client

# Run unit tests
npm test

# Run end-to-end tests
npm run test:e2e

# Run tests with coverage
npm run test:coverage
```

### Test Coverage

The project maintains high test coverage standards:

- **Unit Tests**: >90% coverage for business logic
- **Integration Tests**: >80% coverage for API endpoints
- **E2E Tests**: Critical user flows covered

### Performance Testing

Performance benchmarks are included for critical operations:

```bash
cd innhotel-api/tests/InnHotel.PerformanceTests
dotnet run --configuration Release
```

## 🚀 Deployment

### Production Deployment

#### Prerequisites
- Production PostgreSQL database
- SSL certificate for HTTPS
- Domain name configured
- Server with minimum 8GB RAM

#### API Deployment

1. **Build for Production**
   ```bash
   cd innhotel-api/src/InnHotel.Web
   dotnet publish -c Release -o ./publish
   ```

2. **Configure Production Settings**
   Update `appsettings.Production.json`:
   ```json
   {
     "ConnectionStrings": {
       "PostgreSQLConnection": "Host=prod-server;Port=5432;Database=innhotel_prod;Username=innhotel_prod;Password=secure_password"
     },
     "JwtSettings": {
       "Secret": "production_secret_key"
     },
     "Logging": {
       "LogLevel": {
         "Default": "Warning",
         "Microsoft.AspNetCore": "Error"
       }
     }
   }
   ```

3. **Deploy to Server**
   ```bash
   # Copy published files to server
   scp -r ./publish user@server:/var/www/innhotel-api
   
   # Setup as systemd service
   sudo systemctl enable innhotel-api
   sudo systemctl start innhotel-api
   ```

#### Desktop Client Distribution

1. **Build for Different Platforms**
   ```bash
   cd innhotel-desktop-client
   
   # Windows
   npm run dist:win
   
   # macOS
   npm run dist:mac
   
   # Linux
   npm run dist:linux
   ```

2. **Code Signing**
   - Obtain code signing certificates
   - Configure electron-builder with certificates
   - Sign the application for security

### Docker Production Deployment

1. **Build Production Images**
   ```bash
   docker-compose -f docker-compose.prod.yml build
   ```

2. **Deploy with Docker Swarm/Kubernetes**
   ```bash
   docker stack deploy -c docker-compose.prod.yml innhotel
   ```

## 🔧 Troubleshooting

### Common Issues

#### API Issues

**Problem**: Database connection failed
```
Error: Unable to connect to PostgreSQL database
```
**Solution**:
1. Verify PostgreSQL service is running
2. Check connection string in appsettings.json
3. Ensure database exists and user has permissions
4. Check firewall settings for port 5432

**Problem**: JWT authentication fails
```
Error: 401 Unauthorized
```
**Solution**:
1. Verify JWT secret is configured correctly
2. Check token expiration time
3. Ensure proper token format in Authorization header
4. Validate user credentials in database

**Problem**: API returns 500 Internal Server Error
```
Error: An unhandled exception occurred
```
**Solution**:
1. Check API logs in console or log files
2. Verify all dependencies are installed
3. Check database migration status
4. Review recent code changes

#### Desktop Client Issues

**Problem**: Client cannot connect to API
```
Error: Network request failed
```
**Solution**:
1. Verify API is running on correct port
2. Check CORS configuration in API
3. Ensure correct API URL in client .env file
4. Check firewall/antivirus settings

**Problem**: Electron app won't start
```
Error: Failed to load main process
```
**Solution**:
1. Delete node_modules and reinstall dependencies
2. Rebuild native modules: `npm run rebuild`
3. Check Electron version compatibility
4. Verify main process file exists

**Problem**: UI components not rendering correctly
```
Error: Module not found or rendering issues
```
**Solution**:
1. Clear npm cache: `npm cache clean --force`
2. Delete dist folder and rebuild
3. Check for missing dependencies
4. Verify import paths are correct

### Performance Issues

**Problem**: Slow API response times
**Solution**:
1. Enable database query logging
2. Add database indexes for frequently queried columns
3. Implement caching for expensive operations
4. Optimize LINQ queries to prevent N+1 problems

**Problem**: Desktop client memory usage high
**Solution**:
1. Implement virtual scrolling for large lists
2. Optimize React component re-renders
3. Use React.memo for expensive components
4. Implement proper cleanup in useEffect hooks

### Database Issues

**Problem**: Migration failures
**Solution**:
1. Check migration history consistency
2. Manually apply missing migrations
3. Restore database from backup if needed
4. Use `dotnet ef migrations script` to debug

**Problem**: Database performance degradation
**Solution**:
1. Update table statistics: `ANALYZE`
2. Rebuild indexes: `REINDEX`
3. Check for long-running queries
4. Optimize database configuration parameters

### Getting Help

If you encounter issues not covered here:

1. **Check Documentation**: Review all documentation files in the `/docs` directory
2. **Search Issues**: Look for similar issues in the GitHub repository
3. **Create Issue**: Report new issues with detailed information:
   - Environment details (OS, versions)
   - Steps to reproduce
   - Error messages and logs
   - Expected vs actual behavior

## 🤝 Contributing

We welcome contributions to HotelTransylvania! Please follow these guidelines:

### Getting Started

1. **Fork the Repository**
   ```bash
   git fork https://github.com/HazemSoftPro/HotelTransylvania.git
   ```

2. **Clone Your Fork**
   ```bash
   git clone https://github.com/your-username/HotelTransylvania.git
   ```

3. **Create Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

### Development Guidelines

#### Code Style
- Follow established coding conventions
- Use meaningful variable and function names
- Write self-documenting code
- Add comments for complex logic

#### Commit Messages
Follow conventional commit format:
```
<type>(<scope>): <subject>

<body>

<footer>
```

Types:
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes
- `refactor`: Code refactoring
- `test`: Test additions or changes
- `chore`: Build process or auxiliary tool changes

Example:
```
feat(reservations): add bulk reservation import

- Implement CSV file upload for multiple reservations
- Add validation for imported data
- Create progress indicator for import process

Closes #123
```

#### Pull Request Process

1. **Ensure All Tests Pass**
   ```bash
   cd innhotel-api && dotnet test
   cd innhotel-desktop-client && npm test
   ```

2. **Update Documentation**
   - Update README.md if needed
   - Add/update API documentation
   - Include code comments

3. **Submit Pull Request**
   - Provide clear description of changes
   - Reference related issues
   - Include screenshots for UI changes
   - Request review from maintainers

### Code Review Guidelines

- Review for functionality and correctness
- Check for security vulnerabilities
- Verify test coverage
- Ensure documentation is updated
- Validate performance implications

### Issue Reporting

When reporting issues, include:

1. **Bug Reports**
   - Clear description of the problem
   - Steps to reproduce
   - Expected vs actual behavior
   - Environment details
   - Screenshots if applicable

2. **Feature Requests**
   - Description of the feature
   - Use case and justification
   - Proposed implementation approach
   - Impact on existing functionality

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🆘 Support

For support and questions:

### Documentation
- Comprehensive guides in `/docs` directory
- API documentation at `/swagger` endpoint
- Troubleshooting guide in this README

### Community
- GitHub Discussions for questions
- Issue tracker for bug reports
- Wiki for additional documentation

### Commercial Support
For commercial support, custom development, or enterprise features, please contact:
- Email: support@hoteltransylvania.com
- Website: https://hoteltransylvania.com/support

---

## 🙏 Acknowledgments

- **Hazem Mohammed** - Original project author and lead developer
- **NinjaTech AI Team** - Development support and documentation
- **Contributors** - All contributors who have helped improve this project
- **Open Source Community** - For the amazing tools and libraries used in this project

---

<div align="center">
  <p>Built with ❤️ by the HotelTransylvania Team</p>
  <p><a href="https://github.com/HazemSoftPro/HotelTransylvania">⭐ Star us on GitHub</a></p>
</div>
