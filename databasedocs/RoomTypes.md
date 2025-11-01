# RoomTypes Table Documentation

## Table Overview
The `room_types` table defines different categories of rooms available at hotel branches, such as Standard, Deluxe, Suite, etc.

## Column Definitions

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| id | integer | PRIMARY KEY, NOT NULL | Unique identifier for the room type |
| branch_id | integer | NOT NULL, FOREIGN KEY | Reference to the branch offering this room type |
| name | varchar(100) | NOT NULL | Name of the room type (e.g., "Standard Room", "Deluxe Suite") |
| capacity | integer | NOT NULL | Maximum number of guests the room can accommodate |
| description | text | NULLABLE | Detailed description of the room type and amenities |

## Primary Key
- `id` - Auto-incrementing integer that uniquely identifies each room type

## Foreign Keys
- `branch_id` → branches.id (Each room type belongs to exactly one branch)

## Indexes
- PRIMARY KEY on `id` column
- FOREIGN KEY index on `branch_id`
- UNIQUE index on `(branch_id, name)` (room type names must be unique within each branch)
- INDEX on `capacity` (for room search based on guest count)

## Relationships
- **Many-to-One**: RoomTypes → Branches (each room type belongs to exactly one branch)
- **One-to-Many**: RoomTypes → Rooms (each room type can have multiple individual rooms)

## Business Rules & Constraints
- Room type must belong to a valid branch
- Room type name is required and must be unique within the branch
- Capacity must be a positive integer (minimum 1 guest)
- Capacity should be realistic for hotel operations (typically 1-6 guests)
- Description helps guests understand room features and amenities

## Sample Room Type Names
Common room type categories include:
- "Standard Room"
- "Deluxe Room"
- "Executive Suite"
- "Presidential Suite"
- "Family Room"
- "Ocean View Room"
- "Studio Apartment"
- "Penthouse Suite"

## Capacity Guidelines
Typical room capacities by type:
- Standard Room: 1-2 guests
- Deluxe Room: 2-3 guests
- Suite: 2-4 guests
- Family Room: 4-6 guests
- Presidential Suite: 2-4 guests

## Sample Data Context
Room type records typically include:
- Detailed descriptions of room amenities
- Capacity information for booking system
- Branch-specific room type variations
- Pricing tier information (implicit through room rates)

## Notes
- Room types define the template for individual rooms
- Capacity information is crucial for booking validation
- Descriptions help with marketing and guest expectations
- Consider adding base pricing, amenities list, and size information in future iterations
- Room types can vary significantly between different branches
- This table enables flexible room categorization for different market segments