-- =====================================================
-- InnHotel Database Mock Data
-- Generated comprehensive test data for all tables
-- =====================================================
-- This file contains realistic mock data for testing the InnHotel database
-- All foreign key relationships are maintained and data constraints are respected
-- =====================================================

-- Reset sequences for proper ID generation (for PostgreSQL)
-- These should be adjusted based on your actual sequence names
-- ALTER SEQUENCE branches_id_seq RESTART WITH 1;
-- ALTER SEQUENCE guests_id_seq RESTART WITH 1;
-- ALTER SEQUENCE employees_id_seq RESTART WITH 1;
-- ALTER SEQUENCE room_types_id_seq RESTART WITH 1;
-- ALTER SEQUENCE rooms_id_seq RESTART WITH 1;
-- ALTER SEQUENCE services_id_seq RESTART WITH 1;
-- ALTER SEQUENCE reservations_id_seq RESTART WITH 1;
-- ALTER SEQUENCE reservation_rooms_id_seq RESTART WITH 1;
-- ALTER SEQUENCE reservation_services_id_seq RESTART WITH 1;

-- =====================================================
-- 1. BRANCHES TABLE
-- Hotel locations/branches
-- =====================================================

INSERT INTO branches (id, name, location) VALUES 
(1, 'Grand Plaza Hotel', '123 Main Street, Downtown, New York, NY 10001'),
(2, 'Airport Inn Express', '456 Airport Boulevard, Terminal 2, Newark, NJ 07114'),
(3, 'Seaside Resort & Spa', '789 Ocean Drive, Miami Beach, FL 33139'),
(4, 'Mountain View Lodge', '321 Alpine Road, Aspen, CO 81611'),
(5, 'Business Central Hotel', '555 Financial District, Chicago, IL 60601');

-- =====================================================
-- 2. ROOM_TYPES TABLE
-- Room categories for each branch
-- =====================================================

INSERT INTO room_types (id, branch_id, name, capacity, description) VALUES 
-- Grand Plaza Hotel Room Types
(1, 1, 'Standard Room', 2, 'Comfortable room with queen bed, work desk, and city view'),
(2, 1, 'Deluxe Room', 2, 'Spacious room with king bed, sitting area, and premium amenities'),
(3, 1, 'Executive Suite', 4, 'Luxury suite with separate living area, kitchenette, and panoramic city views'),
(4, 1, 'Presidential Suite', 6, 'Ultimate luxury suite with full kitchen, dining area, and butler service'),

-- Airport Inn Express Room Types
(5, 2, 'Standard Room', 2, 'Efficient room with double bed perfect for transit passengers'),
(6, 2, 'Family Room', 4, 'Connecting rooms ideal for families with children'),
(7, 2, 'Business Suite', 2, 'Room with workspace and high-speed internet for business travelers'),

-- Seaside Resort & Spa Room Types
(8, 3, 'Garden View Room', 2, 'Room overlooking tropical gardens with private balcony'),
(9, 3, 'Ocean View Room', 2, 'Room with stunning ocean views and beach access'),
(10, 3, 'Beach Villa', 6, 'Private villa with direct beach access and personal pool'),

-- Mountain View Lodge Room Types
(11, 4, 'Cozy Room', 2, 'Warm room with fireplace and mountain views'),
(12, 4, 'Luxury Cabin', 4, 'Spacious cabin with full kitchen and ski-in/ski-out access'),

-- Business Central Hotel Room Types
(13, 5, 'Corporate Room', 1, 'Single room optimized for business travelers'),
(14, 5, 'Executive Floor Room', 2, 'Premium room with lounge access and concierge service');

-- =====================================================
-- 3. SERVICES TABLE
-- Additional services offered at each branch
-- =====================================================

INSERT INTO services (id, branch_id, name, price, description) VALUES 
-- Grand Plaza Hotel Services
(1, 1, 'Room Service Breakfast', 25.00, 'Continental breakfast delivered to your room'),
(2, 1, 'Spa Treatment - 60min', 150.00, 'Full body massage with aromatherapy'),
(3, 1, 'Airport Shuttle', 35.00, 'Round trip airport transfer service'),
(4, 1, 'Business Center Access', 50.00, '24-hour access to business facilities'),
(5, 1, 'Laundry Service - Express', 45.00, 'Same-day laundry and pressing service'),

-- Airport Inn Express Services
(6, 2, 'Airport Shuttle - One Way', 15.00, 'Transfer to/from airport terminals'),
(7, 2, 'Early Check-in', 30.00, 'Guaranteed room access before 3 PM'),
(8, 2, 'Late Check-out', 40.00, 'Extended stay until 2 PM'),

-- Seaside Resort & Spa Services
(9, 3, 'Beach Cabana Rental', 75.00, 'Private cabana with lounge chairs and service'),
(10, 3, 'Scuba Diving Lesson', 125.00, 'Beginner scuba diving session with equipment'),
(11, 3, 'Sunset Cruise', 85.00, 'Evening boat tour with refreshments'),
(12, 3, 'Spa Facial Treatment', 95.00, 'Rejuvenating facial with premium products'),

-- Mountain View Lodge Services
(13, 4, 'Ski Equipment Rental', 55.00, 'Full day ski equipment rental'),
(14, 4, 'Ski Lesson - Private', 180.00, 'One-on-one ski instruction'),
(15, 4, 'Firewood Delivery', 25.00, 'Firewood for your room fireplace'),

-- Business Central Hotel Services
(16, 5, 'Meeting Room Rental - Hourly', 100.00, 'Fully equipped meeting room'),
(17, 5, 'Secretarial Services', 60.00, 'Professional administrative support'),
(18, 5, 'Printing Services', 0.50, 'Per page black and white printing');

-- =====================================================
-- 4. GUESTS TABLE
-- Customer information
-- =====================================================

INSERT INTO guests (id, first_name, last_name, gender, id_proof_type, id_proof_number, email, phone, address) VALUES 
(1, 'John', 'Smith', 'Male', 'Passport', 'US123456789', 'john.smith@email.com', '+1-555-0101', '456 Oak Avenue, Boston, MA 02108'),
(2, 'Emily', 'Johnson', 'Female', 'DriverLicense', 'DL987654321', 'emily.j@company.com', '+1-555-0102', '789 Maple Street, Philadelphia, PA 19103'),
(3, 'Michael', 'Chen', 'Male', 'Passport', 'CN456789123', 'm.chen@globalcorp.com', '+1-555-0103', '321 Pine Road, San Francisco, CA 94102'),
(4, 'Sarah', 'Williams', 'Female', 'NationalId', 'NI789456123', 'sarah.williams@email.com', '+1-555-0104', '654 Elm Drive, Los Angeles, CA 90001'),
(5, 'David', 'Brown', 'Male', 'DriverLicense', 'DL321654987', 'david.brown@tech.com', '+1-555-0105', '987 Cedar Lane, Seattle, WA 98101'),
(6, 'Jessica', 'Davis', 'Female', 'Passport', 'UK987123456', 'j.davis@consulting.com', '+1-555-0106', '147 Birch Street, Denver, CO 80201'),
(7, 'Robert', 'Miller', 'Male', 'NationalId', 'NI654321789', 'robert.miller@email.com', '+1-555-0107', '258 Spruce Way, Portland, OR 97201'),
(8, 'Maria', 'Garcia', 'Female', 'Passport', 'ES123789456', 'maria.garcia@travel.com', '+1-555-0108', '369 Redwood Boulevard, San Diego, CA 92101'),
(9, 'James', 'Wilson', 'Male', 'DriverLicense', 'DL147258369', 'j.wilson@finance.com', '+1-555-0109', '741 Hemlock Court, Phoenix, AZ 85001'),
(10, 'Lisa', 'Anderson', 'Female', 'NationalId', 'NI369258147', 'lisa.anderson@email.com', '+1-555-0110', '852 Sequoia Place, Dallas, TX 75201');

-- =====================================================
-- 5. EMPLOYEES TABLE
-- Hotel staff members
-- =====================================================

INSERT INTO employees (id, branch_id, first_name, last_name, email, phone, hire_date, position, user_id) VALUES 
-- Grand Plaza Hotel Staff
(1, 1, 'Thomas', 'Anderson', 't.anderson@grandplaza.com', '+1-555-1001', '2022-03-15', 'Front Desk Manager', 't.anderson'),
(2, 1, 'Jennifer', 'White', 'j.white@grandplaza.com', '+1-555-1002', '2021-07-20', 'Housekeeping Supervisor', 'j.white'),
(3, 1, 'Richard', 'Taylor', 'r.taylor@grandplaza.com', '+1-555-1003', '2023-01-10', 'Concierge', 'r.taylor'),
(4, 1, 'Patricia', 'Martin', 'p.martin@grandplaza.com', '+1-555-1004', '2022-09-05', 'Spa Manager', 'p.martin'),

-- Airport Inn Express Staff
(5, 2, 'Daniel', 'Thompson', 'd.thompson@airportinn.com', '+1-555-2001', '2023-02-01', 'Night Auditor', 'd.thompson'),
(6, 2, 'Nancy', 'Garcia', 'n.garcia@airportinn.com', '+1-555-2002', '2022-11-15', 'Reservation Agent', 'n.garcia'),

-- Seaside Resort & Spa Staff
(7, 3, 'Mark', 'Martinez', 'm.martinez@seasideresort.com', '+1-555-3001', '2021-05-20', 'Activities Director', 'm.martinez'),
(8, 3, 'Linda', 'Robinson', 'l.robinson@seasideresort.com', '+1-555-3002', '2022-08-10', 'Restaurant Manager', 'l.robinson'),

-- Mountain View Lodge Staff
(9, 4, 'Kevin', 'Clark', 'k.clark@mountainlodge.com', '+1-555-4001', '2022-12-01', 'Ski School Director', 'k.clark'),
(10, 4, 'Barbara', 'Rodriguez', 'b.rodriguez@mountainlodge.com', '+1-555-4002', '2023-03-15', 'Front Desk Agent', 'b.rodriguez'),

-- Business Central Hotel Staff
(11, 5, 'Steven', 'Lewis', 's.lewis@businesscentral.com', '+1-555-5001', '2021-10-10', 'General Manager', 's.lewis'),
(12, 5, 'Mary', 'Walker', 'm.walker@businesscentral.com', '+1-555-5002', '2022-06-25', 'Sales Director', 'm.walker');

-- =====================================================
-- 6. ROOMS TABLE
-- Individual hotel rooms
-- =====================================================

INSERT INTO rooms (id, branch_id, room_type_id, capacity, room_number, status, floor, manual_price) VALUES 
-- Grand Plaza Hotel Rooms
(1, 1, 1, 2, '101', 'Available', 1, 150.00),
(2, 1, 1, 2, '102', 'Occupied', 1, 150.00),
(3, 1, 2, 2, '201', 'Available', 2, 225.00),
(4, 1, 2, 2, '202', 'UnderMaintenance', 2, 225.00),
(5, 1, 3, 4, '301', 'Occupied', 3, 450.00),
(6, 1, 4, 6, '401', 'Available', 4, 800.00),

-- Airport Inn Express Rooms
(7, 2, 5, 2, 'A101', 'Available', 1, 95.00),
(8, 2, 5, 2, 'A102', 'Occupied', 1, 95.00),
(9, 2, 6, 4, 'B201', 'Available', 2, 180.00),
(10, 2, 7, 2, 'C301', 'UnderMaintenance', 3, 140.00),

-- Seaside Resort & Spa Rooms
(11, 3, 8, 2, 'G101', 'Available', 1, 275.00),
(12, 3, 9, 2, 'O201', 'Occupied', 2, 350.00),
(13, 3, 10, 6, 'V301', 'Available', 3, 650.00),

-- Mountain View Lodge Rooms
(14, 4, 11, 2, 'C101', 'Available', 1, 200.00),
(15, 4, 11, 2, 'C102', 'Occupied', 1, 200.00),
(16, 4, 12, 4, 'L201', 'UnderMaintenance', 2, 380.00),

-- Business Central Hotel Rooms
(17, 5, 13, 1, 'E101', 'Available', 10, 180.00),
(18, 5, 14, 2, 'E201', 'Occupied', 20, 320.00);

-- =====================================================
-- 7. RESERVATIONS TABLE
-- Booking records
-- =====================================================

INSERT INTO reservations (id, guest_id, branch_id, check_in_date, check_out_date, reservation_date, status, total_cost) VALUES 
-- Current and upcoming reservations
(1, 1, 1, '2024-11-15', '2024-11-18', '2024-10-20 14:30:00', 'Confirmed', 525.00),
(2, 2, 3, '2024-11-20', '2024-11-25', '2024-10-25 10:15:00', 'Confirmed', 1875.00),
(3, 3, 5, '2024-11-22', '2024-11-23', '2024-10-28 16:45:00', 'Pending', 180.00),
(4, 4, 1, '2024-11-25', '2024-11-27', '2024-11-01 09:20:00', 'CheckedIn', 900.00),
(5, 5, 4, '2024-12-01', '2024-12-07', '2024-11-02 11:30:00', 'Confirmed', 1200.00),
(6, 6, 2, '2024-12-05', '2024-12-06', '2024-11-03 13:00:00', 'Confirmed', 95.00),
(7, 7, 3, '2024-12-10', '2024-12-15', '2024-11-04 15:45:00', 'Confirmed', 1375.00),
(8, 8, 1, '2024-12-12', '2024-12-14', '2024-11-05 08:30:00', 'Pending', 450.00),
(9, 9, 5, '2024-12-15', '2024-12-20', '2024-11-06 12:15:00', 'Confirmed', 1600.00),
(10, 10, 4, '2024-12-18', '2024-12-21', '2024-11-07 14:20:00', 'Confirmed', 600.00);

-- =====================================================
-- 8. RESERVATION_ROOMS TABLE
-- Room assignments for reservations
-- =====================================================

INSERT INTO reservation_rooms (id, reservation_id, room_id, price_per_night) VALUES 
-- Reservation 1: John Smith at Grand Plaza (Standard Room, 3 nights)
(1, 1, 1, 175.00),

-- Reservation 2: Emily Johnson at Seaside Resort (Ocean View Room, 5 nights)
(2, 2, 12, 375.00),

-- Reservation 3: Michael Chen at Business Central (Corporate Room, 1 night)
(3, 3, 17, 180.00),

-- Reservation 4: Sarah Williams at Grand Plaza (Executive Suite, 2 nights)
(4, 4, 5, 450.00),

-- Reservation 5: David Brown at Mountain View (Cozy Room, 6 nights)
(5, 5, 14, 200.00),

-- Reservation 6: Jessica Davis at Airport Inn (Standard Room, 1 night)
(6, 6, 7, 95.00),

-- Reservation 7: Robert Miller at Seaside Resort (Garden View Room, 5 nights)
(7, 7, 11, 275.00),

-- Reservation 8: Maria Garcia at Grand Plaza (Deluxe Room, 2 nights)
(8, 8, 3, 225.00),

-- Reservation 9: James Wilson at Business Central (Executive Floor Room, 5 nights)
(9, 9, 18, 320.00),

-- Reservation 10: Lisa Anderson at Mountain View (Cozy Room, 3 nights)
(10, 10, 15, 200.00);

-- =====================================================
-- 9. RESERVATION_SERVICES TABLE
-- Services included in reservations
-- =====================================================

INSERT INTO reservation_services (id, reservation_id, service_id, quantity, unit_price, total_price) VALUES 
-- Reservation 1: Room Service and Spa Treatment
(1, 1, 1, 2, 25.00, 50.00),
(2, 1, 2, 1, 150.00, 150.00),

-- Reservation 2: Beach Cabana and Scuba Diving
(3, 2, 9, 3, 75.00, 225.00),
(4, 2, 10, 2, 125.00, 250.00),

-- Reservation 3: Meeting Room Rental
(5, 3, 16, 2, 100.00, 200.00),

-- Reservation 4: Airport Shuttle and Laundry
(6, 4, 3, 1, 35.00, 35.00),
(7, 4, 5, 3, 45.00, 135.00),

-- Reservation 5: Ski Equipment and Lessons
(8, 5, 13, 4, 55.00, 220.00),
(9, 5, 14, 2, 180.00, 360.00),

-- Reservation 6: Early Check-in
(10, 6, 7, 1, 30.00, 30.00),

-- Reservation 7: Sunset Cruise and Spa Facial
(11, 7, 11, 2, 85.00, 170.00),
(12, 7, 12, 2, 95.00, 190.00),

-- Reservation 8: Room Service and Business Center
(13, 8, 1, 2, 25.00, 50.00),
(14, 8, 4, 1, 50.00, 50.00),

-- Reservation 9: Meeting Room and Secretarial Services
(15, 9, 16, 8, 100.00, 800.00),
(16, 9, 17, 5, 60.00, 300.00),

-- Reservation 10: Ski Equipment and Firewood
(17, 10, 13, 2, 55.00, 110.00),
(18, 10, 15, 3, 25.00, 75.00);

-- =====================================================
-- DATA VALIDATION QUERIES
-- Uncomment these queries to verify data integrity
-- =====================================================

-- Verify foreign key relationships
-- SELECT COUNT(*) as orphaned_records FROM reservations WHERE guest_id NOT IN (SELECT id FROM guests);
-- SELECT COUNT(*) as orphaned_records FROM reservation_rooms WHERE reservation_id NOT IN (SELECT id FROM reservations);
-- SELECT COUNT(*) as orphaned_records FROM reservation_rooms WHERE room_id NOT IN (SELECT id FROM rooms);
-- SELECT COUNT(*) as orphaned_records FROM reservation_services WHERE reservation_id NOT IN (SELECT id FROM reservations);
-- SELECT COUNT(*) as orphaned_records FROM reservation_services WHERE service_id NOT IN (SELECT id FROM services);

-- Verify data constraints
-- SELECT COUNT(*) as invalid_records FROM rooms WHERE manual_price <= 0;
-- SELECT COUNT(*) as invalid_records FROM reservations WHERE check_out_date <= check_in_date;
-- SELECT COUNT(*) as invalid_records FROM reservation_services WHERE quantity <= 0;

-- Check room availability overlaps (simplified check)
-- SELECT r1.room_id, r1.reservation_id, r2.reservation_id 
-- FROM reservation_rooms r1 
-- JOIN reservation_rooms r2 ON r1.room_id = r2.room_id AND r1.reservation_id != r2.reservation_id
-- JOIN reservations res1 ON r1.reservation_id = res1.id
-- JOIN reservations res2 ON r2.reservation_id = res2.id
-- WHERE res1.check_in_date < res2.check_out_date 
-- AND res2.check_in_date < res1.check_out_date
-- AND res1.status IN ('Confirmed', 'CheckedIn')
-- AND res2.status IN ('Confirmed', 'CheckedIn');

-- =====================================================
-- SUMMARY STATISTICS
-- =====================================================

-- Total records inserted:
-- Branches: 5
-- Room Types: 14
-- Services: 18
-- Guests: 10
-- Employees: 12
-- Rooms: 18
-- Reservations: 10
-- Reservation Rooms: 10
-- Reservation Services: 18

-- Total Records: 105

-- This mock data provides a comprehensive testing environment for the InnHotel database system
-- with realistic relationships, proper constraints, and diverse scenarios for testing various features.