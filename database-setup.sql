-- InnHotel Database Setup Script
-- PostgreSQL Database Creation and Configuration

-- Create database and user
CREATE DATABASE innhotel_db;
CREATE USER innhotel_user WITH PASSWORD 'innhotel_secure_password_2024';
GRANT ALL PRIVILEGES ON DATABASE innhotel_db TO innhotel_user;

-- Connect to the database
\c innhotel_db;

-- Create schema if needed
CREATE SCHEMA IF NOT EXISTS public;

-- Grant schema permissions
GRANT USAGE ON SCHEMA public TO innhotel_user;
GRANT CREATE ON SCHEMA public TO innhotel_user;

-- Set default privileges
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO innhotel_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT USAGE, SELECT ON SEQUENCES TO innhotel_user;
ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT EXECUTE ON FUNCTIONS TO innhotel_user;

-- Create extensions if needed
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "citext";

-- Verify setup
SELECT 
    current_database() as database,
    current_user as user,
    version() as postgresql_version;