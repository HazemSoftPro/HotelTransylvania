# Rooms Table Documentation

## Table Overview
The `rooms` table stores information about individual hotel rooms, including their current status, pricing, and physical characteristics.

## Column Definitions

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| id | integer | PRIMARY KEY, NOT NULL | Unique identifier for the room |
| branch_id | integer | NOT NULL, FOREIGN KEY | Reference to the branch where the room is located |
| room_type_id | integer | NOT NULL, FOREIGN KEY | Reference to the room type category |
| capacity | integer | NOT NULL | Maximum number of guests the room can accommodate |
| room_number | varchar(20) | NOT NULL | Room number as displayed to guests (e.g., "101", "A204") |
| status | varchar(20) | NOT NULL | Current room status (Available/Occupied/UnderMaintenance) |
| floor | integer | NOT NULL | Floor number where the room is located |
| manual_price | decimal(10,2) | NOT NULL, CHECK > 0 | Manual pricing override for this specific room |

## Primary Key
- `id` - Auto-incrementing integer that uniquely identifies each room

## Foreign Keys
- `branch_id` → branches.id (Each room belongs to exactly one branch)
- `room_type_id` → room_types.id (Each room belongs to exactly one room type)

## Indexes
- PRIMARY KEY on `id` column
- FOREIGN KEY index on `branch_id`
- FOREIGN KEY index on `room_type_id`
- UNIQUE index on `(branch_id, room_number)` (room numbers must be unique within each branch)
- INDEX on `status` (for room availability queries)
- INDEX on `floor` (for floor-based searches)
- COMPOSITE INDEX on `(branch_id, status)` (for availability checks by branch)

## Relationships
- **Many-to-One**: Rooms → Branches (each room belongs to exactly one branch)
- **Many-to-One**: Rooms → RoomTypes (each room belongs to exactly one room type)
- **One-to-Many**: Rooms → ReservationRooms (each room can be in multiple reservations over time)

## Business Rules & Constraints
- Room must belong to a valid branch
- Room must belong to a valid room type
- Room number must be unique within the branch
- Room status must be one of the predefined values
- Floor number must be a non-negative integer
- Manual price must be greater than 0
- Capacity should match the room type capacity but can be adjusted for specific rooms

## Enum Values

### RoomStatus
- `Available` - Room is ready for booking
- `Occupied` - Room is currently occupied by guests
- `UnderMaintenance` - Room is being repaired or renovated

## Sample Data Context
Room records typically include:
- Physical location information (floor, room number)
- Current operational status
- Specific pricing for premium locations or features
- Capacity information for booking validation

## Room Number Patterns
Common room numbering conventions:
- Standard: "101", "102", "201", "202" (first digit = floor)
- Wings: "A101", "B102", "C201" (letter = wing/section)
- Suites: "S101", "S201" (prefix = suite type)
- Specialty: "V101" (villa), "P201" (penthouse)

## Status Flow
Typical room status transitions:
1. `Available` → `Occupied` (when guest checks in)
2. `Occupied` → `Available` (when guest checks out and room is cleaned)
3. `Available` → `UnderMaintenance` (for repairs/renovation)
4. `UnderMaintenance` → `Available` (when maintenance is complete)

## Notes
- Manual price allows for premium pricing for rooms with better views, locations, or features
- Room status is critical for real-time availability systems
- Floor information helps with housekeeping and maintenance scheduling
- Room numbering should be consistent and logical for guest navigation
- Consider adding last cleaned timestamp, maintenance history, and amenity details in future iterations
- Room capacity may differ from room type capacity for specific configurations