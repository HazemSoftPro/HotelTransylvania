# Employees Table Documentation

## Table Overview
The `employees` table stores information about hotel staff members who work at different branches and manage various hotel operations.

## Column Definitions

| Column Name | Data Type | Constraints | Description |
|-------------|-----------|-------------|-------------|
| id | integer | PRIMARY KEY, NOT NULL | Unique identifier for the employee |
| branch_id | integer | NOT NULL, FOREIGN KEY | Reference to the branch where employee works |
| first_name | varchar(100) | NOT NULL | Employee's first name |
| last_name | varchar(100) | NOT NULL | Employee's last name |
| email | varchar(255) | NULLABLE | Employee's email address |
| phone | varchar(50) | NULLABLE | Employee's phone number |
| hire_date | date | NOT NULL | Date when employee was hired |
| position | varchar(100) | NOT NULL | Job position or role |
| user_id | varchar(100) | NULLABLE, UNIQUE | System user account identifier |

## Primary Key
- `id` - Auto-incrementing integer that uniquely identifies each employee

## Foreign Keys
- `branch_id` → branches.id (Each employee belongs to exactly one branch)

## Indexes
- PRIMARY KEY on `id` column
- FOREIGN KEY index on `branch_id`
- UNIQUE index on `user_id` (if system access is granted)
- INDEX on `email` (for contact and authentication)
- INDEX on `position` (for role-based queries)
- COMPOSITE INDEX on `(branch_id, position)` (for staff management per branch)

## Relationships
- **Many-to-One**: Employees → Branches (each employee works at exactly one branch)
- **One-to-Many**: Employees → Reservations (employees can manage multiple reservations - implicit relationship)

## Business Rules & Constraints
- Employee must be assigned to a valid branch
- Employee name (first and last) is required
- Hire date cannot be in the future
- Position is required for role management
- User ID is optional but must be unique if provided (for system access)
- Email should be unique within the same branch if used for authentication

## Sample Position Types
Typical employee positions include:
- "Front Desk Manager"
- "Housekeeping Supervisor"
- "Concierge"
- "Maintenance Technician"
- "Restaurant Manager"
- "General Manager"
- "Night Auditor"
- "Reservation Agent"

## Sample Data Context
Employee records typically include:
- Front desk staff who handle check-ins/check-outs
- Housekeeping staff responsible for room maintenance
- Management personnel overseeing operations
- Support staff for various hotel services
- Technical staff for maintenance and IT support

## Notes
- The system supports branch-specific employee management
- User ID integration allows for system access control
- Position information helps with role-based permissions and scheduling
- Hire date tracking supports HR functions and reporting
- Consider adding salary information, work schedules, and performance metrics in future iterations
- GDPR considerations for employee personal data