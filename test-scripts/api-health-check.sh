#!/bin/bash

# InnHotel API Health Check Script
# This script verifies that the API is running and all endpoints are accessible

echo "=========================================="
echo "InnHotel API Health Check"
echo "=========================================="
echo ""

API_BASE_URL="https://localhost:57679"
RESULTS_FILE="api-health-check-results.txt"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Initialize results file
echo "API Health Check Results - $(date)" > "$RESULTS_FILE"
echo "========================================" >> "$RESULTS_FILE"
echo "" >> "$RESULTS_FILE"

# Counter for passed/failed tests
PASSED=0
FAILED=0

# Function to test endpoint
test_endpoint() {
    local method=$1
    local endpoint=$2
    local expected_status=$3
    local description=$4
    
    echo -n "Testing: $description... "
    
    response=$(curl -k -s -o /dev/null -w "%{http_code}" -X "$method" "$API_BASE_URL$endpoint")
    
    if [ "$response" = "$expected_status" ]; then
        echo -e "${GREEN}✓ PASSED${NC} (HTTP $response)"
        echo "✓ PASSED - $description (HTTP $response)" >> "$RESULTS_FILE"
        ((PASSED++))
    else
        echo -e "${RED}✗ FAILED${NC} (Expected: $expected_status, Got: $response)"
        echo "✗ FAILED - $description (Expected: $expected_status, Got: $response)" >> "$RESULTS_FILE"
        ((FAILED++))
    fi
}

# Test 1: Health Endpoint
echo "1. Testing Health Endpoint"
echo "----------------------------"
test_endpoint "GET" "/health" "200" "Health check endpoint"
echo ""

# Test 2: Swagger Documentation
echo "2. Testing Swagger Documentation"
echo "--------------------------------"
test_endpoint "GET" "/swagger/index.html" "200" "Swagger UI"
echo ""

# Test 3: Authentication Endpoints (without auth - should return 401 or 400)
echo "3. Testing Authentication Endpoints"
echo "-----------------------------------"
test_endpoint "POST" "/api/auth/login" "400" "Login endpoint (no body)"
test_endpoint "POST" "/api/auth/refresh" "401" "Refresh token endpoint (no token)"
echo ""

# Test 4: Protected Endpoints (should return 401 without auth)
echo "4. Testing Protected Endpoints (No Auth)"
echo "----------------------------------------"
test_endpoint "GET" "/api/branches" "401" "Branches list (unauthorized)"
test_endpoint "GET" "/api/rooms" "401" "Rooms list (unauthorized)"
test_endpoint "GET" "/api/guests" "401" "Guests list (unauthorized)"
test_endpoint "GET" "/api/reservations" "401" "Reservations list (unauthorized)"
test_endpoint "GET" "/api/employees" "401" "Employees list (unauthorized)"
test_endpoint "GET" "/api/services" "401" "Services list (unauthorized)"
test_endpoint "GET" "/api/dashboard/statistics" "401" "Dashboard statistics (unauthorized)"
echo ""

# Test 5: Login with valid credentials
echo "5. Testing Login with Valid Credentials"
echo "---------------------------------------"
LOGIN_RESPONSE=$(curl -k -s -X POST "$API_BASE_URL/api/auth/login" \
    -H "Content-Type: application/json" \
    -d '{"email":"super@innhotel.com","password":"Sup3rP@ssword!"}')

if echo "$LOGIN_RESPONSE" | grep -q "accessToken"; then
    echo -e "${GREEN}✓ PASSED${NC} - Login successful"
    echo "✓ PASSED - Login with valid credentials" >> "$RESULTS_FILE"
    ((PASSED++))
    
    # Extract access token
    ACCESS_TOKEN=$(echo "$LOGIN_RESPONSE" | grep -o '"accessToken":"[^"]*' | cut -d'"' -f4)
    echo "Access token obtained: ${ACCESS_TOKEN:0:20}..."
    
    # Test 6: Protected Endpoints with Auth
    echo ""
    echo "6. Testing Protected Endpoints (With Auth)"
    echo "------------------------------------------"
    
    # Test branches endpoint
    BRANCHES_RESPONSE=$(curl -k -s -o /dev/null -w "%{http_code}" \
        -H "Authorization: Bearer $ACCESS_TOKEN" \
        "$API_BASE_URL/api/branches")
    
    if [ "$BRANCHES_RESPONSE" = "200" ]; then
        echo -e "${GREEN}✓ PASSED${NC} - Branches endpoint (authenticated)"
        echo "✓ PASSED - Branches endpoint (authenticated)" >> "$RESULTS_FILE"
        ((PASSED++))
    else
        echo -e "${RED}✗ FAILED${NC} - Branches endpoint (Expected: 200, Got: $BRANCHES_RESPONSE)"
        echo "✗ FAILED - Branches endpoint (Expected: 200, Got: $BRANCHES_RESPONSE)" >> "$RESULTS_FILE"
        ((FAILED++))
    fi
    
    # Test rooms endpoint
    ROOMS_RESPONSE=$(curl -k -s -o /dev/null -w "%{http_code}" \
        -H "Authorization: Bearer $ACCESS_TOKEN" \
        "$API_BASE_URL/api/rooms")
    
    if [ "$ROOMS_RESPONSE" = "200" ]; then
        echo -e "${GREEN}✓ PASSED${NC} - Rooms endpoint (authenticated)"
        echo "✓ PASSED - Rooms endpoint (authenticated)" >> "$RESULTS_FILE"
        ((PASSED++))
    else
        echo -e "${RED}✗ FAILED${NC} - Rooms endpoint (Expected: 200, Got: $ROOMS_RESPONSE)"
        echo "✗ FAILED - Rooms endpoint (Expected: 200, Got: $ROOMS_RESPONSE)" >> "$RESULTS_FILE"
        ((FAILED++))
    fi
    
    # Test dashboard endpoint
    DASHBOARD_RESPONSE=$(curl -k -s -o /dev/null -w "%{http_code}" \
        -H "Authorization: Bearer $ACCESS_TOKEN" \
        "$API_BASE_URL/api/dashboard/statistics")
    
    if [ "$DASHBOARD_RESPONSE" = "200" ]; then
        echo -e "${GREEN}✓ PASSED${NC} - Dashboard endpoint (authenticated)"
        echo "✓ PASSED - Dashboard endpoint (authenticated)" >> "$RESULTS_FILE"
        ((PASSED++))
    else
        echo -e "${RED}✗ FAILED${NC} - Dashboard endpoint (Expected: 200, Got: $DASHBOARD_RESPONSE)"
        echo "✗ FAILED - Dashboard endpoint (Expected: 200, Got: $DASHBOARD_RESPONSE)" >> "$RESULTS_FILE"
        ((FAILED++))
    fi
    
else
    echo -e "${RED}✗ FAILED${NC} - Login failed"
    echo "✗ FAILED - Login with valid credentials" >> "$RESULTS_FILE"
    ((FAILED++))
fi

echo ""
echo "=========================================="
echo "Test Summary"
echo "=========================================="
echo -e "Total Tests: $((PASSED + FAILED))"
echo -e "${GREEN}Passed: $PASSED${NC}"
echo -e "${RED}Failed: $FAILED${NC}"

if [ $FAILED -eq 0 ]; then
    echo -e "\n${GREEN}All tests passed! ✓${NC}"
    OVERALL_STATUS="ALL TESTS PASSED"
else
    echo -e "\n${RED}Some tests failed! ✗${NC}"
    OVERALL_STATUS="SOME TESTS FAILED"
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