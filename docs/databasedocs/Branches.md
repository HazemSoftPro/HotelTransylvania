# Branches Table Documentation

## Table Overview
The `branches` table stores information about different hotel locations or branches in the InnHotel system.

## Column Definitions

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| id | integer | PRIMARY KEY, NOT NULL | Unique identifier for the branch |
| name | varchar(255) | NOT NULL | Name of the hotel branch |
| location | varchar(500) | NOT NULL | Physical location/address of the branch |

## Primary Key
- `id` - Auto-incrementing integer that uniquely identifies each branch

## Foreign Keys
- None (this is a top-level table)

## Indexes
- PRIMARY KEY on `id` column
- UNIQUE index on `name` (recommended to prevent duplicate branch names)

## Relationships
- **One-to-Many**: Branches → Rooms (each branch can have multiple rooms)
- **One-to-Many**: Branches → RoomTypes (each branch can have multiple room types)
- **One-to-Many**: Branches → Employees (each branch can have multiple employees)
- **One-to-Many**: Branches → Services (each branch can offer multiple services)
- **One-to-Many**: Branches → Reservations (each branch can have multiple reservations)

## Business Rules & Constraints
- Branch name must be unique within the system
- Location information is required for operational purposes
- Each branch serves as a logical grouping for rooms, staff, and services

## Sample Data Context
Typical branches might include:
- "Downtown Hotel" - "123 Main Street, City Center"
- "Airport Inn" - "456 Airport Road, Terminal 2"
- "Beach Resort" - "789 Ocean Boulevard, Coastal Area"

## Notes
- This table serves as the foundation for multi-branch hotel management
- All other major entities reference this table for branch association
- Consider adding columns for contact information, operating hours, and amenities in future iterations