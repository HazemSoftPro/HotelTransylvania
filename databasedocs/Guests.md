# Guests Table Documentation

## Table Overview
The `guests` table stores comprehensive information about hotel guests and customers who make reservations or use hotel services.

## Column Definitions

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| id | integer | PRIMARY KEY, NOT NULL | Unique identifier for the guest |
| first_name | varchar(100) | NOT NULL | Guest's first name |
| last_name | varchar(100) | NOT NULL | Guest's last name |
| gender | varchar(10) | NOT NULL | Guest's gender (Male/Female) |
| id_proof_type | varchar(20) | NOT NULL | Type of identification (Passport/DriverLicense/NationalId) |
| id_proof_number | varchar(100) | NOT NULL, UNIQUE | Identification document number |
| email | varchar(255) | NULLABLE | Guest's email address |
| phone | varchar(50) | NULLABLE | Guest's phone number |
| address | text | NULLABLE | Guest's full address |

## Primary Key
- `id` - Auto-incrementing integer that uniquely identifies each guest

## Foreign Keys
- None (this is a top-level table)

## Indexes
- PRIMARY KEY on `id` column
- UNIQUE index on `id_proof_number` (to prevent duplicate identification records)
- INDEX on `email` (for quick guest lookup)
- INDEX on `phone` (for quick guest lookup)
- INDEX on `last_name` (for alphabetical searching)
- COMPOSITE INDEX on `(first_name, last_name)` (for name-based searches)

## Relationships
- **One-to-Many**: Guests → Reservations (each guest can have multiple reservations)

## Business Rules & Constraints
- Guest name (first and last) is required
- Gender must be one of: Male, Female
- ID proof type must be one of: Passport, DriverLicense, NationalId
- ID proof number must be unique across all guests
- Email and phone are optional but at least one contact method is recommended
- Valid identification is required for hotel stays and legal compliance

## Enum Values

### Gender
- `Male` - Male guest
- `Female` - Female guest

### IdProofType
- `Passport` - Passport document
- `DriverLicense` - Driver's license
- `NationalId` - National identification card

## Sample Data Context
Typical guest records include:
- International travelers with passport information
- Domestic guests with driver's license or national ID
- Business travelers with corporate contact details
- Tourists with temporary accommodation needs

## Notes
- The system supports multiple identification types for international and domestic guests
- Contact information flexibility allows for guests who prefer different communication methods
- Address information helps with billing and future marketing efforts
- Consider adding loyalty program information in future iterations
- GDPR compliance considerations for personal data storage