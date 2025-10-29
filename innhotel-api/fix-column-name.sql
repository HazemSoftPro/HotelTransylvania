-- Rename PriceOverride to manual_price
ALTER TABLE rooms RENAME COLUMN "PriceOverride" TO manual_price;

-- Update NULL values to default
UPDATE rooms SET manual_price = 100.00 WHERE manual_price IS NULL;

-- Alter column to NOT NULL with proper type
ALTER TABLE rooms ALTER COLUMN manual_price SET NOT NULL;
ALTER TABLE rooms ALTER COLUMN manual_price TYPE numeric(10,2);
ALTER TABLE rooms ALTER COLUMN manual_price SET DEFAULT 100.00;

-- Add check constraint
ALTER TABLE rooms DROP CONSTRAINT IF EXISTS "CK_rooms_manual_price";
ALTER TABLE rooms ADD CONSTRAINT "CK_rooms_manual_price" CHECK (manual_price > 0);

-- Verify the change
\d rooms
