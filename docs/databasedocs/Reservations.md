# Reservations Table Documentation

## Table Overview
The `reservations` table is the central entity in the hotel booking system, storing comprehensive information about guest reservations including dates, guests, status, and total costs.

## Column Definitions

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| id | integer | PRIMARY KEY, NOT NULL | Unique identifier for the reservation |
| guest_id | integer | NOT NULL, FOREIGN KEY | Reference to the guest making the reservation |
| branch_id | integer | NULLABLE, FOREIGN KEY | Reference to the branch for the reservation |
| check_in_date | date | NOT NULL | Date when guests will check in |
| check_out_date | date | NOT NULL | Date when guests will check out |
| reservation_date | datetime | NOT NULL | Date and time when reservation was made |
| status | varchar(20) | NOT NULL | Current reservation status |
| total_cost | decimal(10,2) | NOT NULL, CHECK >= 0 | Total cost of the reservation |

## Primary Key
- `id` - Auto-incrementing integer that uniquely identifies each reservation

## Foreign Keys
- `guest_id` → guests.id (Each reservation belongs to exactly one guest)
- `branch_id` → branches.id (Each reservation belongs to exactly one branch)

## Indexes
- PRIMARY KEY on `id` column
- FOREIGN KEY index on `guest_id`
- FOREIGN KEY index on `branch_id`
- INDEX on `status` (for status-based queries)
- INDEX on `check_in_date` (for arrival date queries)
- INDEX on `check_out_date` (for departure date queries)
- INDEX on `reservation_date` (for booking history analysis)
- COMPOSITE INDEX on `(branch_id, status)` (for branch-specific status reports)
- COMPOSITE INDEX on `(guest_id, status)` (for guest reservation history)

## Relationships
- **Many-to-One**: Reservations → Guests (each reservation belongs to exactly one guest)
- **Many-to-One**: Reservations → Branches (each reservation belongs to exactly one branch)
- **One-to-Many**: Reservations → ReservationRooms (each reservation can have multiple rooms)
- **One-to-Many**: Reservations → ReservationServices (each reservation can have multiple services)

## Business Rules & Constraints
- Reservation must belong to a valid guest
- Check-in date must be before check-out date
- Check-in date must be today or in the future
- Reservation date (booking time) cannot be in the future
- Total cost must be a non-negative value
- Status must be one of the predefined values
- Branch is required for multi-branch operations

## Enum Values

### ReservationStatus
- `Pending` - Reservation is made but not yet confirmed
- `Confirmed` - Reservation is confirmed and rooms are allocated
- `CheckedIn` - Guests have arrived and are currently staying
- `CheckedOut` - Guests have departed and reservation is complete
- `Cancelled` - Reservation was cancelled by guest or hotel

## Sample Data Context
Reservation records typically include:
- Booking timeline information
- Guest association for personalization
- Branch assignment for operational management
- Status tracking for workflow management
- Financial information for billing

## Status Flow
Typical reservation status transitions:
1. `Pending` → `Confirmed` (when booking is verified and payment/confirmation is received)
2. `Pending` → `Cancelled` (if booking is cancelled before confirmation)
3. `Confirmed` → `CheckedIn` (when guests arrive and check in)
4. `Confirmed` → `Cancelled` (if reservation is cancelled before arrival)
5. `CheckedIn` → `CheckedOut` (when guests depart)
6. `CheckedIn` → `Cancelled` (rare case of early checkout with cancellation)

## Date Validation Rules
- `check_in_date` ≥ `reservation_date` (can't book for past dates)
- `check_out_date` > `check_in_date` (minimum 1 night stay)
- `reservation_date` ≤ current time (can't make reservations in the future)

## Notes
- Total cost is calculated from room rates, service charges, and applicable taxes
- Status tracking enables proper room allocation and resource planning
- Guest relationship enables personalization and loyalty tracking
- Branch assignment facilitates multi-branch management and reporting
- Consider adding payment information, special requests, and cancellation policies in future iterations
- Integration with room availability system is critical for preventing overbooking
- Audit trail of status changes helps with operational analysis and dispute resolution