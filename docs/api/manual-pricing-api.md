# Manual Pricing API Documentation

## Overview

تم تحديث نظام التسعير في InnHotel API ليستخدم نموذج التسعير اليدوي (`ManualPrice`) بدلاً من النموذج القديم المعتمد على `BasePrice` و `PriceOverride`. هذا التحديث يوفر مرونة أكبر في إدارة أسعار الغرف.

## Changes Summary

### ما تم إزالته:
- `BasePrice` من `RoomType` entity
- `PriceOverride` من `Room` entity
- جميع الحسابات التلقائية للأسعار

### ما تم إضافته:
- `ManualPrice` في `Room` entity (مطلوب، يجب أن يكون > 0)
- Validation rules جديدة للتسعير اليدوي
- Database migration للتحويل الآمن للبيانات

## API Endpoints

### Rooms API

#### 1. Create Room
**POST** `/api/rooms`

**Request Body:**
```json
{
  "branchId": 1,
  "roomTypeId": 1,
  "roomNumber": "101",
  "status": "Available",
  "floor": 1,
  "manualPrice": 150.75
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "branchId": 1,
  "branchName": "Main Branch",
  "roomTypeId": 1,
  "roomTypeName": "Standard Room",
  "capacity": 2,
  "roomNumber": "101",
  "status": "Available",
  "floor": 1,
  "manualPrice": 150.75
}
```

**Validation Rules:**
- `manualPrice` مطلوب ويجب أن يكون > 0
- `roomNumber` مطلوب وفريد لكل فرع
- `branchId` و `roomTypeId` يجب أن يكونا موجودين

#### 2. Update Room
**PUT** `/api/rooms/{id}`

**Request Body:**
```json
{
  "roomTypeId": 1,
  "roomNumber": "101A",
  "status": "Occupied",
  "floor": 1,
  "manualPrice": 175.50
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "branchId": 1,
  "branchName": "Main Branch",
  "roomTypeId": 1,
  "roomTypeName": "Standard Room",
  "capacity": 2,
  "roomNumber": "101A",
  "status": "Occupied",
  "floor": 1,
  "manualPrice": 175.50
}
```

#### 3. Get Room by ID
**GET** `/api/rooms/{id}`

**Response (200 OK):**
```json
{
  "id": 1,
  "branchId": 1,
  "branchName": "Main Branch",
  "roomTypeId": 1,
  "roomTypeName": "Standard Room",
  "capacity": 2,
  "roomNumber": "101",
  "status": "Available",
  "floor": 1,
  "manualPrice": 150.75
}
```

#### 4. List Rooms
**GET** `/api/rooms`

**Query Parameters:**
- `branchId` (optional): Filter by branch
- `status` (optional): Filter by room status
- `page` (optional): Page number (default: 1)
- `pageSize` (optional): Items per page (default: 10)

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "branchId": 1,
    "branchName": "Main Branch",
    "roomTypeId": 1,
    "roomTypeName": "Standard Room",
    "capacity": 2,
    "roomNumber": "101",
    "status": "Available",
    "floor": 1,
    "manualPrice": 150.75
  },
  {
    "id": 2,
    "branchId": 1,
    "branchName": "Main Branch",
    "roomTypeId": 2,
    "roomTypeName": "Deluxe Room",
    "capacity": 4,
    "roomNumber": "201",
    "status": "Occupied",
    "floor": 2,
    "manualPrice": 250.00
  }
]
```

#### 5. Search Rooms
**GET** `/api/rooms/search`

**Query Parameters:**
- `roomNumber` (optional): Search by room number
- `status` (optional): Filter by status
- `minPrice` (optional): Minimum manual price
- `maxPrice` (optional): Maximum manual price
- `floor` (optional): Filter by floor

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "branchId": 1,
    "branchName": "Main Branch",
    "roomTypeId": 1,
    "roomTypeName": "Standard Room",
    "capacity": 2,
    "roomNumber": "101",
    "status": "Available",
    "floor": 1,
    "manualPrice": 150.75
  }
]
```

#### 6. Update Room Status
**PATCH** `/api/rooms/{id}/status`

**Request Body:**
```json
{
  "status": "UnderMaintenance"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "branchId": 1,
  "branchName": "Main Branch",
  "roomTypeId": 1,
  "roomTypeName": "Standard Room",
  "capacity": 2,
  "roomNumber": "101",
  "status": "UnderMaintenance",
  "floor": 1,
  "manualPrice": 150.75
}
```

### Room Types API

#### 1. Create Room Type
**POST** `/api/room-types`

**Request Body:**
```json
{
  "branchId": 1,
  "name": "Presidential Suite",
  "capacity": 6,
  "description": "Luxury presidential suite with premium amenities"
}
```

**Response (201 Created):**
```json
{
  "id": 3,
  "branchId": 1,
  "branchName": "Main Branch",
  "name": "Presidential Suite",
  "capacity": 6,
  "description": "Luxury presidential suite with premium amenities"
}
```

**Note:** لم يعد `basePrice` مطلوباً أو موجوداً في Room Types

#### 2. Update Room Type
**PUT** `/api/room-types/{id}`

**Request Body:**
```json
{
  "branchId": 1,
  "name": "Executive Suite",
  "capacity": 4,
  "description": "Updated description"
}
```

#### 3. Get Room Type by ID
**GET** `/api/room-types/{id}`

**Response (200 OK):**
```json
{
  "id": 1,
  "branchId": 1,
  "branchName": "Main Branch",
  "name": "Standard Room",
  "capacity": 2,
  "description": "Comfortable standard room"
}
```

#### 4. List Room Types
**GET** `/api/room-types`

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "branchId": 1,
    "branchName": "Main Branch",
    "name": "Standard Room",
    "capacity": 2,
    "description": "Comfortable standard room"
  },
  {
    "id": 2,
    "branchId": 1,
    "branchName": "Main Branch",
    "name": "Deluxe Room",
    "capacity": 4,
    "description": "Spacious deluxe room with city view"
  }
]
```

## Error Responses

### Validation Errors (400 Bad Request)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "ManualPrice": [
      "Manual price must be greater than 0"
    ]
  }
}
```

### Not Found (404 Not Found)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Room with ID 999 was not found."
}
```

### Server Error (500 Internal Server Error)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "An error occurred while processing your request.",
  "status": 500
}
```

## Data Models

### Room Model
```typescript
interface Room {
  id: number;
  branchId: number;
  branchName: string;
  roomTypeId: number;
  roomTypeName: string;
  capacity: number;
  roomNumber: string;
  status: 'Available' | 'Occupied' | 'UnderMaintenance';
  floor: number;
  manualPrice: number; // Always required, must be > 0
}
```

### RoomType Model
```typescript
interface RoomType {
  id: number;
  branchId: number;
  branchName: string;
  name: string;
  capacity: number;
  description?: string;
  // Note: basePrice is no longer present
}
```

### Create/Update Request Models
```typescript
interface CreateRoomRequest {
  branchId: number;
  roomTypeId: number;
  roomNumber: string;
  status: 'Available' | 'Occupied' | 'UnderMaintenance';
  floor: number;
  manualPrice: number; // Required, must be > 0
}

interface UpdateRoomRequest {
  roomTypeId: number;
  roomNumber: string;
  status: 'Available' | 'Occupied' | 'UnderMaintenance';
  floor: number;
  manualPrice: number; // Required, must be > 0
}

interface CreateRoomTypeRequest {
  branchId: number;
  name: string;
  capacity: number;
  description?: string;
  // Note: basePrice is no longer required
}
```

## Migration Guide

### For Frontend Developers

1. **Update TypeScript Interfaces:**
   - Remove `basePrice` from `RoomType` interface
   - Remove `priceOverride` from `Room` interface
   - Add `manualPrice` to `Room` interface

2. **Update API Calls:**
   - Include `manualPrice` in room creation/update requests
   - Remove `basePrice` from room type requests
   - Update validation to ensure `manualPrice > 0`

3. **Update UI Components:**
   - Replace price calculation logic with direct `manualPrice` display
   - Update forms to include manual price input
   - Add validation for manual price field

### For Backend Developers

1. **Database Migration:**
   - Run the provided migration script
   - Verify data integrity after migration
   - Update seed data if necessary

2. **Code Updates:**
   - All domain models, DTOs, and handlers have been updated
   - Validation rules have been updated
   - Remove any remaining price calculation logic

## Testing

### Unit Tests
- Test domain model validation
- Test price validation rules
- Test entity creation and updates

### Integration Tests
- Test API endpoints with valid/invalid prices
- Test database operations
- Test migration scripts

### Manual Testing Checklist
- [ ] Create room with valid manual price
- [ ] Try to create room with invalid price (should fail)
- [ ] Update room price
- [ ] Verify price is maintained during status updates
- [ ] Test room type operations (no basePrice)
- [ ] Verify migration preserves existing data

## Performance Considerations

- Manual pricing eliminates complex price calculations
- Database queries are simplified (no joins for price calculation)
- Indexing on `manual_price` column may be beneficial for price-based searches
- Consider caching frequently accessed room data

## Security Considerations

- Validate manual price on both client and server side
- Ensure only authorized users can modify room prices
- Log price changes for audit purposes
- Consider implementing price change approval workflow for high-value rooms
