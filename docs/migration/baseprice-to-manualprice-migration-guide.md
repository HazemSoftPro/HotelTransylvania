# Migration Guide: BasePrice to ManualPrice

## Overview

هذا الدليل يوضح كيفية الهجرة من نموذج التسعير القديم (BasePrice + PriceOverride) إلى نموذج التسعير اليدوي الجديد (ManualPrice) في نظام InnHotel.

## Pre-Migration Checklist

### 1. Backup Database
```bash
# PostgreSQL backup
pg_dump -h localhost -U postgres -d innhotel_production > backup_before_migration.sql

# Or using Docker
docker exec postgres_container pg_dump -U postgres innhotel_production > backup_before_migration.sql
```

### 2. Verify Current Data
```sql
-- Check current room pricing data
SELECT 
    r.id,
    r.room_number,
    rt.name as room_type,
    rt.base_price,
    r.price_override,
    COALESCE(r.price_override, rt.base_price) as effective_price
FROM rooms r
JOIN room_types rt ON r.room_type_id = rt.id
ORDER BY r.id;

-- Count rooms with price overrides
SELECT 
    COUNT(*) as total_rooms,
    COUNT(price_override) as rooms_with_override,
    COUNT(*) - COUNT(price_override) as rooms_using_base_price
FROM rooms;
```

### 3. Test Environment Setup
```bash
# Create test database
createdb innhotel_migration_test

# Restore production data to test environment
psql -d innhotel_migration_test < backup_before_migration.sql
```

## Migration Steps

### Step 1: Update Application Code

#### 1.1 Update Domain Models
```csharp
// OLD: Room.cs
public class Room
{
    public decimal? PriceOverride { get; private set; }
    // ...
}

// NEW: Room.cs
public class Room
{
    public decimal ManualPrice { get; private set; }
        = Guard.Against.NegativeOrZero(manualPrice, nameof(manualPrice));
    // ...
}
```

```csharp
// OLD: RoomType.cs
public class RoomType
{
    public decimal BasePrice { get; private set; }
    // ...
}

// NEW: RoomType.cs
public class RoomType
{
    // BasePrice removed completely
    // ...
}
```

#### 1.2 Update DTOs and Requests
```csharp
// OLD: CreateRoomRequest
public record CreateRoomRequest
{
    public decimal? PriceOverride { get; init; }
}

// NEW: CreateRoomRequest
public record CreateRoomRequest
{
    public decimal ManualPrice { get; init; }
}
```

#### 1.3 Update Validation Rules
```csharp
// NEW: CreateRoomValidator
public class CreateRoomValidator : AbstractValidator<CreateRoomRequest>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.ManualPrice)
            .GreaterThan(0)
            .WithMessage("Manual price must be greater than 0");
    }
}
```

### Step 2: Database Migration

#### 2.1 Generate Migration
```bash
cd innhotel-api/src/InnHotel.Web
dotnet ef migrations add ReplaceBasePriceWithManualPrice
```

#### 2.2 Review Migration Script
```csharp
public partial class ReplaceBasePriceWithManualPrice : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Step 1: Add ManualPrice column
        migrationBuilder.AddColumn<decimal>(
            name: "manual_price",
            table: "rooms",
            type: "numeric(10,2)",
            nullable: false,
            defaultValue: 0m);

        // Step 2: Populate ManualPrice with existing data
        migrationBuilder.Sql(@"
            UPDATE rooms 
            SET manual_price = COALESCE(""PriceOverride"", rt.base_price)
            FROM room_types rt 
            WHERE rooms.room_type_id = rt.id;
        ");

        // Step 3: Add constraint
        migrationBuilder.AddCheckConstraint(
            name: "CK_rooms_manual_price",
            table: "rooms",
            sql: "manual_price > 0");

        // Step 4: Drop old columns
        migrationBuilder.DropColumn(name: "PriceOverride", table: "rooms");
        migrationBuilder.DropColumn(name: "BasePrice", table: "room_types");
    }
}
```

#### 2.3 Test Migration on Test Database
```bash
# Apply migration to test database
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=innhotel_migration_test;Username=postgres;Password=postgres"
dotnet ef database update --verbose

# Verify migration results
psql -d innhotel_migration_test -c "
SELECT 
    COUNT(*) as total_rooms,
    COUNT(manual_price) as rooms_with_manual_price,
    MIN(manual_price) as min_price,
    MAX(manual_price) as max_price,
    AVG(manual_price) as avg_price
FROM rooms;
"
```

### Step 3: Update Configuration

#### 3.1 Entity Configuration
```csharp
// RoomConfiguration.cs
public void Configure(EntityTypeBuilder<Room> r)
{
    // Add ManualPrice configuration
    r.Property(x => x.ManualPrice)
      .HasColumnName("manual_price")
      .HasColumnType("numeric(10,2)")
      .IsRequired();

    // Add check constraint
    r.ToTable(t => {
        t.HasCheckConstraint("CK_rooms_manual_price", "manual_price > 0");
    });
}
```

#### 3.2 Update Seed Data
```csharp
// SeedData.cs
public static void Initialize(AppDbContext context)
{
    if (!context.Rooms.Any())
    {
        var rooms = new[]
        {
            new Room(1, 1, "101", RoomStatus.Available, 1, 150.00m),
            new Room(1, 1, "102", RoomStatus.Available, 1, 150.00m),
            new Room(1, 2, "201", RoomStatus.Available, 2, 250.00m),
            // Note: ManualPrice is now required in constructor
        };
        
        context.Rooms.AddRange(rooms);
        context.SaveChanges();
    }
}
```

### Step 4: Update API Layer

#### 4.1 Update Controllers
```csharp
// RoomsController.cs
[HttpPost]
public async Task<ActionResult<RoomRecord>> CreateRoom(CreateRoomRequest request)
{
    var command = new CreateRoomCommand(
        request.BranchId,
        request.RoomTypeId,
        request.RoomNumber,
        request.Status,
        request.Floor,
        request.ManualPrice); // Updated parameter

    var result = await _mediator.Send(command);
    return result.IsSuccess ? Created($"/api/rooms/{result.Value.Id}", result.Value) : BadRequest(result.Errors);
}
```

#### 4.2 Update Response Models
```csharp
// RoomRecord.cs
public record RoomRecord(
    int Id,
    int BranchId,
    string BranchName,
    int RoomTypeId,
    string RoomTypeName,
    int Capacity,
    string RoomNumber,
    RoomStatus Status,
    int Floor,
    decimal ManualPrice); // Updated field name
```

## Production Migration

### Step 1: Pre-Migration Verification
```sql
-- Verify data integrity before migration
SELECT 
    COUNT(*) as total_rooms,
    COUNT(CASE WHEN price_override IS NOT NULL THEN 1 END) as rooms_with_override,
    COUNT(CASE WHEN price_override IS NULL THEN 1 END) as rooms_using_base_price,
    MIN(COALESCE(price_override, rt.base_price)) as min_effective_price,
    MAX(COALESCE(price_override, rt.base_price)) as max_effective_price
FROM rooms r
JOIN room_types rt ON r.room_type_id = rt.id;

-- Check for any potential issues
SELECT r.id, r.room_number, rt.base_price, r.price_override
FROM rooms r
JOIN room_types rt ON r.room_type_id = rt.id
WHERE rt.base_price <= 0 OR (r.price_override IS NOT NULL AND r.price_override <= 0);
```

### Step 2: Deploy Application
```bash
# Build and deploy new version
dotnet publish -c Release -o ./publish
# Deploy to production environment
```

### Step 3: Run Migration
```bash
# Apply database migration
dotnet ef database update --connection "your-production-connection-string"
```

### Step 4: Post-Migration Verification
```sql
-- Verify migration success
SELECT 
    COUNT(*) as total_rooms,
    COUNT(manual_price) as rooms_with_manual_price,
    COUNT(CASE WHEN manual_price > 0 THEN 1 END) as valid_prices,
    MIN(manual_price) as min_price,
    MAX(manual_price) as max_price,
    AVG(manual_price) as avg_price
FROM rooms;

-- Check for any data loss
SELECT COUNT(*) FROM rooms WHERE manual_price IS NULL OR manual_price <= 0;
```

## Rollback Plan

### Emergency Rollback Script
```sql
-- EMERGENCY ROLLBACK - Only use if migration fails
BEGIN;

-- Re-add old columns
ALTER TABLE room_types ADD COLUMN base_price numeric(10,2);
ALTER TABLE rooms ADD COLUMN price_override numeric(10,2);

-- Restore data from backup if needed
-- RESTORE DATA FROM BACKUP HERE

-- Remove new column
ALTER TABLE rooms DROP COLUMN manual_price;

COMMIT;
```

### Application Rollback
```bash
# Deploy previous version of application
# Restore database from backup if necessary
psql -d innhotel_production < backup_before_migration.sql
```

## Testing Checklist

### Pre-Migration Tests
- [ ] Backup database successfully created
- [ ] Test environment setup and verified
- [ ] All existing functionality works in test environment
- [ ] Migration script tested on test data

### Post-Migration Tests
- [ ] All rooms have valid manual_price > 0
- [ ] No data loss during migration
- [ ] API endpoints work with new pricing model
- [ ] Room creation/update works with manual pricing
- [ ] Room type operations work without base price
- [ ] Search and filtering work correctly

### Integration Tests
- [ ] Frontend can create/update rooms with manual prices
- [ ] Reservation system works with new pricing
- [ ] Reporting systems updated for new pricing model
- [ ] All dependent systems updated

## Common Issues and Solutions

### Issue 1: Rooms with Zero or Negative Prices
```sql
-- Find problematic rooms
SELECT r.id, r.room_number, rt.base_price, r.price_override
FROM rooms r
JOIN room_types rt ON r.room_type_id = rt.id
WHERE COALESCE(r.price_override, rt.base_price) <= 0;

-- Fix: Set default price for problematic rooms
UPDATE rooms 
SET manual_price = 100.00 
WHERE manual_price <= 0;
```

### Issue 2: Missing Room Type References
```sql
-- Find rooms with missing room types
SELECT r.id, r.room_number, r.room_type_id
FROM rooms r
LEFT JOIN room_types rt ON r.room_type_id = rt.id
WHERE rt.id IS NULL;

-- Fix: Either fix references or remove orphaned rooms
```

### Issue 3: Application Errors After Migration
```csharp
// Check for remaining references to old properties
// Search codebase for: BasePrice, PriceOverride
// Update any missed references
```

## Performance Considerations

### Database Indexes
```sql
-- Add index for price-based searches
CREATE INDEX IX_rooms_manual_price ON rooms(manual_price);

-- Add composite index for common queries
CREATE INDEX IX_rooms_branch_status_price ON rooms(branch_id, status, manual_price);
```

### Query Optimization
```sql
-- OLD: Complex price calculation query
SELECT r.*, COALESCE(r.price_override, rt.base_price) as effective_price
FROM rooms r
JOIN room_types rt ON r.room_type_id = rt.id;

-- NEW: Simple direct price query
SELECT r.*, r.manual_price
FROM rooms r;
```

## Monitoring and Alerts

### Set up monitoring for:
- Database migration completion
- Application startup after deployment
- API response times
- Error rates in pricing-related endpoints
- Data integrity checks

### Alert conditions:
- Any rooms with manual_price <= 0
- High error rates in room creation/update
- Unusual pricing patterns
- Performance degradation

## Support and Troubleshooting

### Log Analysis
```bash
# Check application logs for pricing-related errors
grep -i "manualprice\|pricing\|price" /var/log/innhotel/app.log

# Check database logs for constraint violations
grep -i "CK_rooms_manual_price" /var/log/postgresql/postgresql.log
```

### Health Checks
```csharp
// Add health check for pricing data integrity
public class PricingHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var invalidPriceCount = await _context.Rooms.CountAsync(r => r.ManualPrice <= 0, cancellationToken);
        
        return invalidPriceCount == 0 
            ? HealthCheckResult.Healthy("All rooms have valid manual prices")
            : HealthCheckResult.Unhealthy($"Found {invalidPriceCount} rooms with invalid prices");
    }
}
```

## Conclusion

هذه الهجرة تبسط نموذج التسعير وتوفر مرونة أكبر في إدارة أسعار الغرف. تأكد من اتباع جميع الخطوات بعناية واختبار كل شيء في بيئة التطوير قبل التطبيق في الإنتاج.

للحصول على المساعدة أو الإبلاغ عن مشاكل، يرجى التواصل مع فريق التطوير.
