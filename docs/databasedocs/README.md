# InnHotel Database Documentation

## Overview

This directory contains comprehensive documentation for the InnHotel database schema, including detailed documentation for each table and mock data for testing purposes.

## Documentation Structure

### Table Documentation Files

Each table has its own dedicated documentation file with the following sections:

- **Table Overview**: Purpose and role in the system
- **Column Definitions**: Complete schema with data types, constraints, and descriptions
- **Primary Keys**: Identification columns
- **Foreign Keys**: Relationships with other tables
- **Indexes**: Performance optimization details
- **Relationships**: Entity relationship diagram information
- **Business Rules & Constraints**: Validation rules and limitations
- **Sample Data Context**: Real-world examples and use cases
- **Notes**: Implementation considerations and future enhancements

### Available Table Documentation

1. **[Branches.md](Branches.md)** - Hotel locations and branches
2. **[Guests.md](Guests.md)** - Customer information and personal details
3. **[Employees.md](Employees.md)** - Staff members and employee management
4. **[RoomTypes.md](RoomTypes.md)** - Room categories and classifications
5. **[Rooms.md](Rooms.md)** - Individual hotel rooms and their status
6. **[Services.md](Services.md)** - Additional hotel services and amenities
7. **[Reservations.md](Reservations.md)** - Booking records and reservation management
8. **[ReservationRooms.md](ReservationRooms.md)** - Junction table for room assignments
9. **[ReservationServices.md](ReservationServices.md)** - Junction table for service bookings

## Entity Relationships

### Core Entities
- **Branches**: Top-level entity for multi-branch management
- **Guests**: Customer information for booking and personalization
- **Employees**: Staff management and operational assignments

### Room Management
- **RoomTypes**: Categories of rooms with capacity and descriptions
- **Rooms**: Individual rooms with status and pricing

### Service Management
- **Services**: Additional services and amenities offered

### Booking System
- **Reservations**: Main booking entity with dates, costs, and status
- **ReservationRooms**: Links reservations to specific rooms
- **ReservationServices**: Links reservations to additional services

### Relationship Flow
```
Branches ──┬─→ RoomTypes ──→ Rooms ──→ ReservationRooms ──→ Reservations ──→ Guests
           │                                              ↑
           ├─→ Services ──────────────────────────────────┤
           │                                              │
           └─→ Employees ──────────────────────────────────┘
```

## Mock Data

Comprehensive mock data is available in the `../mock-data/innhotel_mock_data.sql` file with:

- **5 Branches** - Different hotel locations and types
- **14 Room Types** - Various room categories across branches
- **18 Services** - Different services per branch type
- **10 Guests** - Diverse customer profiles
- **12 Employees** - Staff across all branches
- **18 Rooms** - Individual rooms with varied statuses
- **10 Reservations** - Different booking scenarios
- **10 Reservation Rooms** - Room assignments
- **18 Reservation Services** - Service bookings

### Data Characteristics

- **Realistic Content**: Hotel-appropriate names, addresses, and services
- **Data Integrity**: All foreign key relationships maintained
- **Constraint Compliance**: All business rules and constraints respected
- **Diverse Scenarios**: Different statuses, dates, and pricing
- **Comprehensive Coverage**: All enum values and relationship types included

## Usage Instructions

### Using the Documentation

1. **Schema Understanding**: Review each table's documentation to understand the database structure
2. **Development Reference**: Use column definitions and constraints for application development
3. **Testing Planning**: Use business rules and relationships for test case design
4. **Data Modeling**: Reference relationships for new feature development

### Using the Mock Data

1. **Database Setup**: Ensure your PostgreSQL database is created with proper schemas
2. **Sequence Reset**: Uncomment and run the sequence reset commands if needed
3. **Data Import**: Execute the SQL file to populate all tables with test data
4. **Validation**: Run the provided validation queries to verify data integrity
5. **Testing**: Use the data for application testing and development

### Sample Import Process

```sql
-- Connect to your database
\c innhotel_db

-- Import the mock data
\i mock-data/innhotel_mock_data.sql

-- Verify the data was imported correctly
SELECT COUNT(*) FROM branches;
SELECT COUNT(*) FROM guests;
SELECT COUNT(*) FROM reservations;
```

## Database Features

### Enum Types

The database uses several enum types for data consistency:

- **Gender**: Male, Female
- **IdProofType**: Passport, DriverLicense, NationalId
- **RoomStatus**: Available, Occupied, UnderMaintenance
- **ReservationStatus**: Pending, Confirmed, CheckedIn, CheckedOut, Cancelled

### Constraints

- **Foreign Keys**: All relationships properly enforced
- **Unique Constraints**: Prevent duplicate entries where appropriate
- **Check Constraints**: Validate data ranges and business rules
- **Not Null**: Ensure required data is always present

### Indexes

- **Primary Keys**: All tables have proper primary key indexes
- **Foreign Keys**: Relationship columns are indexed for performance
- **Search Optimization**: Frequently queried columns have appropriate indexes
- **Composite Indexes**: Multi-column indexes for common query patterns

## Development Guidelines

### When Adding New Features

1. **Review Documentation**: Understand existing relationships and constraints
2. **Update Documentation**: Document new tables and relationships
3. **Maintain Integrity**: Ensure foreign keys and constraints are properly defined
4. **Add Mock Data**: Include appropriate test data in the mock data file
5. **Update Relationships**: Update relationship diagrams and documentation

### Data Integrity Rules

- Always validate foreign key relationships before inserting data
- Ensure enum values are valid before insertion
- Check for constraint violations before data modifications
- Use transactions for multi-table operations
- Implement proper error handling for constraint violations

## Support

For questions about the database schema or mock data:

1. Review the specific table documentation
2. Check the validation queries in the mock data file
3. Verify relationships using the provided relationship diagrams
4. Consult the business rules sections for constraint information

## Version Information

- **Documentation Version**: 1.0
- **Database Schema**: Based on InnHotel entity models
- **Mock Data**: Generated for comprehensive testing
- **Last Updated**: November 2024