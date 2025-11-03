# ReservationServices Table Documentation

## Table Overview
The `reservation_services` table is a junction table that links reservations with additional services, tracking which services are included in each reservation, quantities, and pricing.

## Column Definitions

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| id | integer | PRIMARY KEY, NOT NULL | Unique identifier for the reservation-service relationship |
| reservation_id | integer | NOT NULL, FOREIGN KEY | Reference to the reservation |
| service_id | integer | NOT NULL, FOREIGN KEY | Reference to the specific service |
| quantity | integer | NOT NULL, CHECK > 0 | Number of units of this service requested |
| unit_price | decimal(10,2) | NOT NULL, CHECK > 0 | Price per unit for this service in this reservation |
| total_price | decimal(10,2) | NOT NULL, CHECK >= 0 | Total price for this service (quantity × unit_price) |

## Primary Key
- `id` - Auto-incrementing integer that uniquely identifies each reservation-service relationship

## Foreign Keys
- `reservation_id` → reservations.id (Each record belongs to exactly one reservation)
- `service_id` → services.id (Each record belongs to exactly one service)

## Indexes
- PRIMARY KEY on `id` column
- FOREIGN KEY index on `reservation_id`
- FOREIGN KEY index on `service_id`
- UNIQUE index on `(reservation_id, service_id)` (prevents duplicate service entries in same reservation)
- INDEX on `total_price` (for revenue analysis)
- INDEX on `quantity` (for service usage analysis)

## Relationships
- **Many-to-One**: ReservationServices → Reservations (each record belongs to exactly one reservation)
- **Many-to-One**: ReservationServices → Services (each record belongs to exactly one service)
- This table implements a **Many-to-Many** relationship between Reservations and Services

## Business Rules & Constraints
- Must belong to a valid reservation
- Must belong to a valid service
- Quantity must be a positive integer
- Unit price must be greater than 0
- Total price must be non-negative
- Same service cannot be added twice to the same reservation (use quantity instead)
- Total price should equal quantity × unit_price (application-level validation)
- Service availability must be checked before assignment (application-level validation)

## Sample Data Context
Reservation service records typically include:
- Additional services requested by guests
- Quantity information for multiple units of the same service
- Pricing that may differ from standard service rates
- Service bookings that contribute to total reservation revenue

## Service Quantity Examples
Common quantity scenarios:
- "Spa Massage" - quantity: 2 (for 2 people)
- "Airport Shuttle" - quantity: 1 (one way trip)
- "Laundry Service" - quantity: 5 (5 items)
- "Room Service Breakfast" - quantity: 3 (3 people)
- "Meeting Room Rental" - quantity: 4 (4 hours)

## Pricing Considerations
Unit price may vary based on:
- Guest loyalty status
- Package deals and promotions
- Corporate agreements
- Seasonal pricing
- Length of stay discounts
- Bulk service discounts

## Notes
- This table enables comprehensive service tracking within reservations
- Quantity support allows for flexible service ordering
- Price variation enables dynamic pricing and promotions
- Service addition can happen at booking, check-in, or during stay
- Consider adding service delivery status, time slots, and special instructions in future iterations
- Integration with service staff scheduling and resource management
- Historical service data helps with demand forecasting and staffing
- Service cancellation policies may require partial refunds tracked in this table

## Revenue Impact
This table is crucial for:
- Calculating total reservation revenue
- Service performance analysis
- Guest preference tracking
- Staff productivity metrics
- Inventory management for consumable services
- Commission and gratuity calculations

## Data Integrity Notes
- Application must verify service availability and staffing before creating records
- Service timing conflicts must be checked for time-sensitive services
- Price validation should align with hotel pricing policies
- Consider soft delete for audit purposes rather than hard deletion
- Service changes during stays require proper tracking and potential refunds