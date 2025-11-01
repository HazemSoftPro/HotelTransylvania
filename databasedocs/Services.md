# Services Table Documentation

## Table Overview
The `services` table defines additional services and amenities that hotels can offer to guests, such as spa treatments, room service, laundry, etc.

## Column Definitions

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| id | integer | PRIMARY KEY, NOT NULL | Unique identifier for the service |
| branch_id | integer | NOT NULL, FOREIGN KEY | Reference to the branch offering this service |
| name | varchar(200) | NOT NULL | Name of the service (e.g., "Room Service", "Spa Treatment") |
| price | decimal(10,2) | NOT NULL, CHECK >= 0 | Standard price for the service |
| description | text | NULLABLE | Detailed description of the service and what's included |

## Primary Key
- `id` - Auto-incrementing integer that uniquely identifies each service

## Foreign Keys
- `branch_id` → branches.id (Each service belongs to exactly one branch)

## Indexes
- PRIMARY KEY on `id` column
- FOREIGN KEY index on `branch_id`
- UNIQUE index on `(branch_id, name)` (service names must be unique within each branch)
- INDEX on `price` (for service pricing queries)

## Relationships
- **Many-to-One**: Services → Branches (each service belongs to exactly one branch)
- **One-to-Many**: Services → ReservationServices (each service can be included in multiple reservations)

## Business Rules & Constraints
- Service must belong to a valid branch
- Service name is required and must be unique within the branch
- Price must be a non-negative value
- Zero price can be used for complimentary services
- Description helps guests understand service details and value

## Sample Service Categories
Common hotel services include:

### Room Services
- "Room Service Breakfast"
- "In-Room Dining - Lunch"
- "In-Room Dining - Dinner"
- "Mini Bar Restocking"
- "Turndown Service"

### Wellness & Recreation
- "Spa Massage - 60 minutes"
- "Spa Facial Treatment"
- "Gym Access - Day Pass"
- "Pool Access - Day Pass"
- "Sauna Session"

### Business Services
- "Business Center Access"
- "Meeting Room Rental - Hourly"
- "Conference Room - Half Day"
- "Printing Services"
- "Secretarial Services"

### Transportation
- "Airport Shuttle - One Way"
- "Airport Shuttle - Round Trip"
- "City Tour"
- "Car Rental Service"
- "Taxi Arrangement"

### Laundry & Housekeeping
- "Laundry Service - Regular"
- "Laundry Service - Express"
- "Dry Cleaning"
- "Ironing Service"
- "Shoe Shine"

## Sample Data Context
Service records typically include:
- Detailed pricing for different service tiers
- Description of inclusions and exclusions
- Branch-specific service offerings
- Complimentary vs. paid services

## Notes
- Services enable additional revenue streams beyond room bookings
- Price flexibility allows for different service tiers and packages
- Branch-specific services cater to different market segments and locations
- Description helps with service marketing and guest understanding
- Consider adding service availability schedules, booking requirements, and staff assignments in future iterations
- Some services may be seasonal or time-dependent
- Integration with reservation system allows for pre-booking and package deals