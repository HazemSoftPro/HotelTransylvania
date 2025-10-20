-- Migration: Add Payment and Notification tables for Phase 4
-- Date: 2025-10-19

-- Create Payment table
CREATE TABLE IF NOT EXISTS "Payments" (
    "Id" SERIAL PRIMARY KEY,
    "ReservationId" INTEGER NOT NULL,
    "Amount" DECIMAL(18,2) NOT NULL,
    "Method" INTEGER NOT NULL,
    "Status" INTEGER NOT NULL,
    "PaymentDate" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "ProcessedDate" TIMESTAMP NULL,
    "TransactionId" VARCHAR(255) NULL,
    "PaymentProvider" VARCHAR(100) NULL,
    "PaymentProviderResponse" TEXT NULL,
    "IsRefunded" BOOLEAN NOT NULL DEFAULT FALSE,
    "RefundedAmount" DECIMAL(18,2) NOT NULL DEFAULT 0,
    "RefundDate" TIMESTAMP NULL,
    "RefundReason" TEXT NULL,
    "Description" TEXT NULL,
    "Notes" TEXT NULL,
    "ProcessedByUserId" INTEGER NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT "FK_Payments_Reservations" FOREIGN KEY ("ReservationId") 
        REFERENCES "Reservations"("Id") ON DELETE CASCADE
);

-- Create indexes for Payment table
CREATE INDEX "IX_Payments_ReservationId" ON "Payments"("ReservationId");
CREATE INDEX "IX_Payments_Status" ON "Payments"("Status");
CREATE INDEX "IX_Payments_PaymentDate" ON "Payments"("PaymentDate");
CREATE INDEX "IX_Payments_TransactionId" ON "Payments"("TransactionId");

-- Create Notification table
CREATE TABLE IF NOT EXISTS "Notifications" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "Title" VARCHAR(255) NOT NULL,
    "Message" TEXT NOT NULL,
    "Type" INTEGER NOT NULL,
    "Channel" INTEGER NOT NULL,
    "Status" INTEGER NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "SentAt" TIMESTAMP NULL,
    "ReadAt" TIMESTAMP NULL,
    "RelatedEntityType" VARCHAR(100) NULL,
    "RelatedEntityId" INTEGER NULL,
    "DeliveryId" VARCHAR(255) NULL,
    "ErrorMessage" TEXT NULL,
    "RetryCount" INTEGER NOT NULL DEFAULT 0
);

-- Create indexes for Notification table
CREATE INDEX "IX_Notifications_UserId" ON "Notifications"("UserId");
CREATE INDEX "IX_Notifications_Status" ON "Notifications"("Status");
CREATE INDEX "IX_Notifications_CreatedAt" ON "Notifications"("CreatedAt");
CREATE INDEX "IX_Notifications_Type" ON "Notifications"("Type");

-- Create NotificationPreferences table
CREATE TABLE IF NOT EXISTS "NotificationPreferences" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "NotificationType" INTEGER NOT NULL,
    "EmailEnabled" BOOLEAN NOT NULL DEFAULT TRUE,
    "SMSEnabled" BOOLEAN NOT NULL DEFAULT FALSE,
    "InAppEnabled" BOOLEAN NOT NULL DEFAULT TRUE,
    "PushEnabled" BOOLEAN NOT NULL DEFAULT TRUE,
    CONSTRAINT "UQ_NotificationPreferences_User_Type" UNIQUE ("UserId", "NotificationType")
);

-- Create index for NotificationPreferences table
CREATE INDEX "IX_NotificationPreferences_UserId" ON "NotificationPreferences"("UserId");

-- Add performance indexes to existing tables
CREATE INDEX IF NOT EXISTS "IX_Reservations_CheckInDate" ON "Reservations"("CheckInDate");
CREATE INDEX IF NOT EXISTS "IX_Reservations_CheckOutDate" ON "Reservations"("CheckOutDate");
CREATE INDEX IF NOT EXISTS "IX_Reservations_Status" ON "Reservations"("Status");
CREATE INDEX IF NOT EXISTS "IX_Reservations_GuestId" ON "Reservations"("GuestId");

CREATE INDEX IF NOT EXISTS "IX_Rooms_BranchId" ON "Rooms"("BranchId");
CREATE INDEX IF NOT EXISTS "IX_Rooms_RoomTypeId" ON "Rooms"("RoomTypeId");
CREATE INDEX IF NOT EXISTS "IX_Rooms_Status" ON "Rooms"("Status");

CREATE INDEX IF NOT EXISTS "IX_Guests_Email" ON "Guests"("Email");
CREATE INDEX IF NOT EXISTS "IX_Guests_PhoneNumber" ON "Guests"("PhoneNumber");

CREATE INDEX IF NOT EXISTS "IX_Employees_BranchId" ON "Employees"("BranchId");
CREATE INDEX IF NOT EXISTS "IX_Employees_UserId" ON "Employees"("UserId");

-- Add comments for documentation
COMMENT ON TABLE "Payments" IS 'Stores payment transactions for reservations';
COMMENT ON TABLE "Notifications" IS 'Stores notifications sent to users';
COMMENT ON TABLE "NotificationPreferences" IS 'Stores user preferences for notification channels';