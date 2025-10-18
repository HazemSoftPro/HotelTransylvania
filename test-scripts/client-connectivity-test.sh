#!/bin/bash

# InnHotel Client Connectivity Test Script
# This script verifies that the client is running and can connect to the API

echo "=========================================="
echo "InnHotel Client Connectivity Test"
echo "=========================================="
echo ""

CLIENT_URL="http://localhost:5173"
API_URL="https://localhost:57679"
RESULTS_FILE="client-connectivity-results.txt"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Initialize results file
echo "Client Connectivity Test Results - $(date)" > "$RESULTS_FILE"
echo "========================================" >> "$RESULTS_FILE"
echo "" >> "$RESULTS_FILE"

PASSED=0
FAILED=0

# Test 1: Client Server Running
echo "1. Testing Client Server"
echo "------------------------"
CLIENT_RESPONSE=$(curl -s -o /dev/null -w "%{http_code}" "$CLIENT_URL")

if [ "$CLIENT_RESPONSE" = "200" ]; then
    echo -e "${GREEN}✓ PASSED${NC} - Client is running (HTTP $CLIENT_RESPONSE)"
    echo "✓ PASSED - Client is running (HTTP $CLIENT_RESPONSE)" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗ FAILED${NC} - Client is not responding (HTTP $CLIENT_RESPONSE)"
    echo "✗ FAILED - Client is not responding (HTTP $CLIENT_RESPONSE)" >> "$RESULTS_FILE"
    ((FAILED++))
fi

echo ""

# Test 2: Client HTML Content
echo "2. Testing Client HTML Content"
echo "------------------------------"
CLIENT_CONTENT=$(curl -s "$CLIENT_URL")

if echo "$CLIENT_CONTENT" | grep -q "<!DOCTYPE html>"; then
    echo -e "${GREEN}✓ PASSED${NC} - Client returns valid HTML"
    echo "✓ PASSED - Client returns valid HTML" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗ FAILED${NC} - Client does not return valid HTML"
    echo "✗ FAILED - Client does not return valid HTML" >> "$RESULTS_FILE"
    ((FAILED++))
fi

# Check for React root element
if echo "$CLIENT_CONTENT" | grep -q "root"; then
    echo -e "${GREEN}✓ PASSED${NC} - React root element found"
    echo "✓ PASSED - React root element found" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗ FAILED${NC} - React root element not found"
    echo "✗ FAILED - React root element not found" >> "$RESULTS_FILE"
    ((FAILED++))
fi

echo ""

# Test 3: API Connectivity from Client Perspective
echo "3. Testing API Connectivity"
echo "---------------------------"
API_HEALTH=$(curl -k -s -o /dev/null -w "%{http_code}" "$API_URL/health")

if [ "$API_HEALTH" = "200" ]; then
    echo -e "${GREEN}✓ PASSED${NC} - API is accessible (HTTP $API_HEALTH)"
    echo "✓ PASSED - API is accessible (HTTP $API_HEALTH)" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗ FAILED${NC} - API is not accessible (HTTP $API_HEALTH)"
    echo "✗ FAILED - API is not accessible (HTTP $API_HEALTH)" >> "$RESULTS_FILE"
    ((FAILED++))
fi

echo ""

# Test 4: CORS Configuration
echo "4. Testing CORS Configuration"
echo "-----------------------------"
CORS_TEST=$(curl -k -s -o /dev/null -w "%{http_code}" \
    -H "Origin: $CLIENT_URL" \
    -H "Access-Control-Request-Method: POST" \
    -H "Access-Control-Request-Headers: Content-Type" \
    -X OPTIONS "$API_URL/api/auth/login")

if [ "$CORS_TEST" = "204" ] || [ "$CORS_TEST" = "200" ]; then
    echo -e "${GREEN}✓ PASSED${NC} - CORS is configured (HTTP $CORS_TEST)"
    echo "✓ PASSED - CORS is configured (HTTP $CORS_TEST)" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${YELLOW}⚠ WARNING${NC} - CORS preflight returned HTTP $CORS_TEST"
    echo "⚠ WARNING - CORS preflight returned HTTP $CORS_TEST" >> "$RESULTS_FILE"
fi

echo ""

# Test 5: Client Assets Loading
echo "5. Testing Client Assets"
echo "------------------------"

# Check for JavaScript files
JS_FILES=$(curl -s "$CLIENT_URL" | grep -o 'src="[^"]*\.js"' | wc -l)
if [ "$JS_FILES" -gt 0 ]; then
    echo -e "${GREEN}✓ PASSED${NC} - JavaScript files referenced ($JS_FILES files)"
    echo "✓ PASSED - JavaScript files referenced ($JS_FILES files)" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗ FAILED${NC} - No JavaScript files found"
    echo "✗ FAILED - No JavaScript files found" >> "$RESULTS_FILE"
    ((FAILED++))
fi

# Check for CSS files
CSS_FILES=$(curl -s "$CLIENT_URL" | grep -o 'href="[^"]*\.css"' | wc -l)
if [ "$CSS_FILES" -gt 0 ]; then
    echo -e "${GREEN}✓ PASSED${NC} - CSS files referenced ($CSS_FILES files)"
    echo "✓ PASSED - CSS files referenced ($CSS_FILES files)" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${YELLOW}⚠ WARNING${NC} - No CSS files found"
    echo "⚠ WARNING - No CSS files found" >> "$RESULTS_FILE"
fi

echo ""

# Test 6: Response Time
echo "6. Testing Response Time"
echo "------------------------"
START_TIME=$(date +%s%N)
curl -s "$CLIENT_URL" > /dev/null
END_TIME=$(date +%s%N)
RESPONSE_TIME=$(( (END_TIME - START_TIME) / 1000000 ))

echo "Client response time: ${RESPONSE_TIME}ms"
echo "Client response time: ${RESPONSE_TIME}ms" >> "$RESULTS_FILE"

if [ "$RESPONSE_TIME" -lt 3000 ]; then
    echo -e "${GREEN}✓ PASSED${NC} - Response time is acceptable (< 3000ms)"
    echo "✓ PASSED - Response time is acceptable" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${YELLOW}⚠ WARNING${NC} - Response time is slow (> 3000ms)"
    echo "⚠ WARNING - Response time is slow" >> "$RESULTS_FILE"
fi

echo ""

# Test 7: Port Availability
echo "7. Testing Port Availability"
echo "----------------------------"

# Check if port 5173 is listening
if netstat -tuln 2>/dev/null | grep -q ":5173"; then
    echo -e "${GREEN}✓ PASSED${NC} - Port 5173 is listening"
    echo "✓ PASSED - Port 5173 is listening" >> "$RESULTS_FILE"
    ((PASSED++))
elif ss -tuln 2>/dev/null | grep -q ":5173"; then
    echo -e "${GREEN}✓ PASSED${NC} - Port 5173 is listening"
    echo "✓ PASSED - Port 5173 is listening" >> "$RESULTS_FILE"
    ((PASSED++))
else
    echo -e "${RED}✗ FAILED${NC} - Port 5173 is not listening"
    echo "✗ FAILED - Port 5173 is not listening" >> "$RESULTS_FILE"
    ((FAILED++))
fi

echo ""

# Summary
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