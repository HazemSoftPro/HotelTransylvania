# ReservationRooms Table Documentation

## Table Overview
The `reservation_rooms` table is a junction table that links reservations with specific rooms, storing information about which rooms are included in each reservation at what price.

## Column Definitions

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| id | integer | PRIMARY KEY, NOT NULL | Unique identifier for the reservation-room relationship |
| reservation_id | integer | NOT NULL, FOREIGN KEY | Reference to the reservation |
| room_id | integer | NOT NULL, FOREIGN KEY | Reference to the specific room |
| price_per_night | decimal(10,2) | NOT NULL, CHECK > 0 | Price charged per night for this room in this reservation |

## Primary Key
- `id` - Auto-incrementing integer that uniquely identifies each reservation-room relationship

## Foreign Keys
- `reservation_id` → reservations.id (Each record belongs to exactly one reservation)
- `room_id` → rooms.id (Each record belongs to exactly one room)

## Indexes
- PRIMARY KEY on `id` column
- FOREIGN KEY index on `reservation_id`
- FOREIGN KEY index on `room_id`
- UNIQUE index on `(reservation_id, room_id)` (prevents duplicate room assignments in same reservation)
- INDEX on `price_per_night` (for pricing analysis)

## Relationships
- **Many-to-One**: ReservationRooms → Reservations (each record belongs to exactly one reservation)
- **Many-to-One**: ReservationRooms → Rooms (each record belongs to exactly one room)
- This table implements a **Many-to-Many** relationship between Reservations and Rooms

## Business Rules & Constraints
- Must belong to a valid reservation
- Must belong to a valid room
- Price per night must be greater than 0
- Same room cannot be assigned twice to the same reservation
- Room availability must be checked before assignment (application-level validation)
- Price may differ from room's manual price based on promotions, discounts, or seasonal rates

## Sample Data Context
Reservation room records typically include:
- Specific room assignments for confirmed bookings
- Pricing information that may differ from standard room rates
- Links between reservations and actual room inventory

## Pricing Considerations
Price per night can vary based on:
- Seasonal demand
- Promotional discounts
- Length of stay discounts
- Corporate rates
- Loyalty program benefits
- Room-specific features (view, location, size)

## Notes
- This table enables flexible room assignment within reservations
- Price variation allows for dynamic pricing strategies
- Junction design supports multiple rooms per reservation (e.g., family bookings)
- Room assignment is typically done at confirmation time, not booking time
- Consider adding check-in/check-out status per room, special requests, and room change history in future iterations
- Integration with housekeeping system through room assignments
- Historical pricing data helps with revenue management and forecasting
- Room changes during stays require updates to this table with proper audit trails

## Data Integrity Notes
- Application must verify room availability before creating records
- Date overlap checking must consider all reservation periods for each room
- Price validation should align with hotel pricing policies
- Consider soft delete for audit purposes rather than hard deletion