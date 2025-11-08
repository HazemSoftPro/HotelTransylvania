-- Fix the reservations sequence to prevent duplicate key errors
-- This sets the sequence to the maximum ID currently in the table

SELECT setval('public."reservations_Id_seq"', COALESCE((SELECT MAX("Id") FROM public.reservations), 0) + 1, false);
