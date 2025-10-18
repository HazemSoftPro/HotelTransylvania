#!/bin/bash

# InnHotel Database Verification Script
# This script verifies database connectivity and schema

echo "=========================================="
echo "InnHotel Database Verification"
echo "=========================================="
echo ""

# Database connection parameters
DB_HOST="localhost"
DB_PORT="5432"
DB_NAME="innhotel_db"
DB_USER="innhotel_user"
DB_PASSWORD="innhotel_secure_password_2024"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

RESULTS_FILE="database-verification-results.txt"

# Initialize results file
echo "Database Verification Results - $(date)" > "$RESULTS_FILE"
echo "========================================" >> "$RESULTS_FILE"
echo "" >> "$RESULTS_FILE"

PASSED=0
FAILED=0

# Function to run SQL query
run_query() {
    local query=$1
    PGPASSWORD="$DB_PASSWORD" psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -t -c "$query" 2>&1
}

# Test 1: Database Connection
echo "1. Testing Database Connection"
echo "------------------------------"
CONNECTION_TEST=$(PGPASSWORD="$DB_PASSWORD" psql -h "$DB_HOST" -p "$DB_PORT" -U "$DB_USER" -d "$DB_NAME" -c "SELECT 1" 2>&1)

if echo "$CONNECTION_TEST" | grep -q "1 row"; then
    echo -e "${GREEN}✓ PASSED${NC} - Database connection successful"
    echo "✓ PASSED - Database connection successful" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗ FAILED${NC} - Database connection failed"
    echo "✗ FAILED - Database connection failed" >> "$RESULTS_FILE"
    echo "Error: $CONNECTION_TEST" >> "$RESULTS_FILE"
    ((FAILED++))
    echo ""
    echo "Cannot proceed without database connection. Exiting..."
    exit 1
fi

echo ""

# Test 2: Check Required Tables
echo "2. Checking Required Tables"
echo "---------------------------"

REQUIRED_TABLES=("branches" "rooms" "roomtypes" "guests" "reservations" "employees" "services" "applicationusers" "refreshtokens")

for table in "${REQUIRED_TABLES[@]}"; do
    TABLE_EXISTS=$(run_query "SELECT EXISTS (SELECT FROM information_schema.tables WHERE table_schema = 'public' AND table_name = '$table');")
    
    if echo "$TABLE_EXISTS" | grep -q "t"; then
        echo -e "${GREEN}✓${NC} Table '$table' exists"
        echo "✓ Table '$table' exists" >> "$RESULTS_FILE"
        ((PASSED++))
    else
        echo -e "${RED}✗${NC} Table '$table' missing"
        echo "✗ Table '$table' missing" >> "$RESULTS_FILE"
        ((FAILED++))
    fi
done

echo ""

# Test 3: Check Table Structures
echo "3. Checking Table Structures"
echo "----------------------------"

# Check branches table columns
BRANCHES_COLUMNS=$(run_query "SELECT column_name FROM information_schema.columns WHERE table_name = 'branches' ORDER BY ordinal_position;")
if echo "$BRANCHES_COLUMNS" | grep -q "name"; then
    echo -e "${GREEN}✓${NC} Branches table structure is valid"
    echo "✓ Branches table structure is valid" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗${NC} Branches table structure is invalid"
    echo "✗ Branches table structure is invalid" >> "$RESULTS_FILE"
    ((FAILED++))
fi

# Check rooms table columns
ROOMS_COLUMNS=$(run_query "SELECT column_name FROM information_schema.columns WHERE table_name = 'rooms' ORDER BY ordinal_position;")
if echo "$ROOMS_COLUMNS" | grep -q "roomnumber"; then
    echo -e "${GREEN}✓${NC} Rooms table structure is valid"
    echo "✓ Rooms table structure is valid" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗${NC} Rooms table structure is invalid"
    echo "✗ Rooms table structure is invalid" >> "$RESULTS_FILE"
    ((FAILED++))
fi

echo ""

# Test 4: Check Seed Data
echo "4. Checking Seed Data"
echo "---------------------"

# Check for default users
USER_COUNT=$(run_query "SELECT COUNT(*) FROM applicationusers;")
if [ "$USER_COUNT" -ge 1 ]; then
    echo -e "${GREEN}✓${NC} Default users exist (Count: $USER_COUNT)"
    echo "✓ Default users exist (Count: $USER_COUNT)" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${YELLOW}⚠${NC} No users found in database"
    echo "⚠ No users found in database" >> "$RESULTS_FILE"
fi

echo ""

# Test 5: Check Foreign Key Constraints
echo "5. Checking Foreign Key Constraints"
echo "-----------------------------------"

FK_COUNT=$(run_query "SELECT COUNT(*) FROM information_schema.table_constraints WHERE constraint_type = 'FOREIGN KEY';")
if [ "$FK_COUNT" -ge 5 ]; then
    echo -e "${GREEN}✓${NC} Foreign key constraints exist (Count: $FK_COUNT)"
    echo "✓ Foreign key constraints exist (Count: $FK_COUNT)" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗${NC} Insufficient foreign key constraints (Count: $FK_COUNT)"
    echo "✗ Insufficient foreign key constraints (Count: $FK_COUNT)" >> "$RESULTS_FILE"
    ((FAILED++))
fi

echo ""

# Test 6: Check Indexes
echo "6. Checking Indexes"
echo "-------------------"

INDEX_COUNT=$(run_query "SELECT COUNT(*) FROM pg_indexes WHERE schemaname = 'public';")
if [ "$INDEX_COUNT" -ge 5 ]; then
    echo -e "${GREEN}✓${NC} Indexes exist (Count: $INDEX_COUNT)"
    echo "✓ Indexes exist (Count: $INDEX_COUNT)" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${YELLOW}⚠${NC} Few indexes found (Count: $INDEX_COUNT)"
    echo "⚠ Few indexes found (Count: $INDEX_COUNT)" >> "$RESULTS_FILE"
fi

echo ""

# Test 7: Database Size
echo "7. Checking Database Size"
echo "-------------------------"

DB_SIZE=$(run_query "SELECT pg_size_pretty(pg_database_size('$DB_NAME'));")
echo "Database size: $DB_SIZE"
echo "Database size: $DB_SIZE" >> "$RESULTS_FILE"

echo ""

# Test 8: Active Connections
echo "8. Checking Active Connections"
echo "------------------------------"

ACTIVE_CONNECTIONS=$(run_query "SELECT COUNT(*) FROM pg_stat_activity WHERE datname = '$DB_NAME';")
echo "Active connections: $ACTIVE_CONNECTIONS"
echo "Active connections: $ACTIVE_CONNECTIONS" >> "$RESULTS_FILE"

echo ""

# Summary
echo "=========================================="
echo "Verification Summary"
echo "=========================================="
echo -e "Total Tests: $((PASSED + FAILED))"
echo -e "${GREEN}Passed: $PASSED${NC}"
echo -e "${RED}Failed: $FAILED${NC}"

if [ $FAILED -eq 0 ]; then
    echo -e "\n${GREEN}All verifications passed! ✓${NC}"
    OVERALL_STATUS="ALL VERIFICATIONS PASSED"
else
    echo -e "\n${RED}Some verifications failed! ✗${NC}"
    OVERALL_STATUS="SOME VERIFICATIONS FAILED"
fi

echo ""
echo "Results saved to: $RESULTS_FILE"

# Write summary to results file
echo "" >> "$RESULTS_FILE"
echo "========================================" >> "$RESULTS_FILE"
echo "Summary" >> "$RESULTS_FILE"
echo "========================================" >> "$RESULTS_FILE"
echo "Total Tests: $((PASSED + FAILED))" >> "$RESULTS_FILE"
echo "Passed: $PASSED" >> "$RESULTS_FILE"
echo "Failed: $FAILED" >> "$RESULTS_FILE"
echo "Status: $OVERALL_STATUS" >> "$RESULTS_FILE"

exit $FAILED