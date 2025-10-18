#!/bin/bash

# InnHotel Comprehensive Test Suite Runner
# This script runs all test scripts and generates a comprehensive report

echo "=========================================="
echo "InnHotel Comprehensive Test Suite"
echo "=========================================="
echo ""
echo "Starting test execution at $(date)"
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Create results directory
RESULTS_DIR="test-results-$(date +%Y%m%d_%H%M%S)"
mkdir -p "$RESULTS_DIR"

echo "Test results will be saved to: $RESULTS_DIR"
echo ""

# Initialize summary
TOTAL_PASSED=0
TOTAL_FAILED=0
TESTS_RUN=0

# Function to run a test script
run_test() {
    local script=$1
    local name=$2
    
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}Running: $name${NC}"
    echo -e "${BLUE}========================================${NC}"
    echo ""
    
    if [ -f "$script" ]; then
        chmod +x "$script"
        ./"$script"
        EXIT_CODE=$?
        
        # Move results file to results directory
        if [ -f "$(basename ${script%.sh})-results.txt" ]; then
            mv "$(basename ${script%.sh})-results.txt" "$RESULTS_DIR/"
        fi
        
        if [ $EXIT_CODE -eq 0 ]; then
            echo -e "\n${GREEN}✓ $name completed successfully${NC}\n"
            ((TESTS_RUN++))
        else
            echo -e "\n${RED}✗ $name completed with failures${NC}\n"
            ((TESTS_RUN++))
        fi
        
        return $EXIT_CODE
    else
        echo -e "${RED}✗ Test script not found: $script${NC}\n"
        return 1
    fi
}

# Run all test scripts
echo "Starting test execution..."
echo ""

# Test 1: Database Verification
run_test "test-scripts/database-verification.sh" "Database Verification"
DB_EXIT=$?

sleep 2

# Test 2: API Health Check
run_test "test-scripts/api-health-check.sh" "API Health Check"
API_EXIT=$?

sleep 2

# Test 3: Client Connectivity Test
run_test "test-scripts/client-connectivity-test.sh" "Client Connectivity Test"
CLIENT_EXIT=$?

echo ""
echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}Generating Comprehensive Report${NC}"
echo -e "${BLUE}========================================${NC}"
echo ""

# Create comprehensive report
REPORT_FILE="$RESULTS_DIR/comprehensive-test-report.txt"

cat > "$REPORT_FILE" << EOF
========================================
InnHotel Comprehensive Test Report
========================================
Generated: $(date)

========================================
Test Execution Summary
========================================

Tests Run: $TESTS_RUN

Individual Test Results:
------------------------
1. Database Verification: $([ $DB_EXIT -eq 0 ] && echo "PASSED ✓" || echo "FAILED ✗")
2. API Health Check: $([ $API_EXIT -eq 0 ] && echo "PASSED ✓" || echo "FAILED ✗")
3. Client Connectivity: $([ $CLIENT_EXIT -eq 0 ] && echo "PASSED ✓" || echo "FAILED ✗")

========================================
Overall Status
========================================

EOF

# Calculate overall status
TOTAL_FAILURES=$((DB_EXIT + API_EXIT + CLIENT_EXIT))

if [ $TOTAL_FAILURES -eq 0 ]; then
    echo "Status: ALL TESTS PASSED ✓" >> "$REPORT_FILE"
    echo ""
    echo -e "${GREEN}========================================${NC}"
    echo -e "${GREEN}ALL TESTS PASSED! ✓${NC}"
    echo -e "${GREEN}========================================${NC}"
    OVERALL_EXIT=0
else
    echo "Status: SOME TESTS FAILED ✗" >> "$REPORT_FILE"
    echo "Total Failures: $TOTAL_FAILURES" >> "$REPORT_FILE"
    echo ""
    echo -e "${RED}========================================${NC}"
    echo -e "${RED}SOME TESTS FAILED! ✗${NC}"
    echo -e "${RED}========================================${NC}"
    OVERALL_EXIT=1
fi

cat >> "$REPORT_FILE" << EOF

========================================
Detailed Results
========================================

Detailed results for each test can be found in:
- database-verification-results.txt
- api-health-check-results.txt
- client-connectivity-results.txt

========================================
Recommendations
========================================

EOF

# Add recommendations based on failures
if [ $DB_EXIT -ne 0 ]; then
    cat >> "$REPORT_FILE" << EOF
Database Issues Detected:
- Check PostgreSQL service is running
- Verify database credentials
- Ensure migrations are applied
- Review database-verification-results.txt for details

EOF
fi

if [ $API_EXIT -ne 0 ]; then
    cat >> "$REPORT_FILE" << EOF
API Issues Detected:
- Check API service is running on port 57679
- Verify database connection
- Check API logs for errors
- Review api-health-check-results.txt for details

EOF
fi

if [ $CLIENT_EXIT -ne 0 ]; then
    cat >> "$REPORT_FILE" << EOF
Client Issues Detected:
- Check client development server is running on port 5173
- Verify API connectivity
- Check browser console for errors
- Review client-connectivity-results.txt for details

EOF
fi

cat >> "$REPORT_FILE" << EOF
========================================
Next Steps
========================================

1. Review individual test result files
2. Address any failed tests
3. Re-run tests after fixes
4. Proceed with manual testing if all automated tests pass

========================================
End of Report
========================================
EOF

echo ""
echo "Comprehensive report saved to: $REPORT_FILE"
echo ""

# Display report summary
echo "=========================================="
echo "Report Summary"
echo "=========================================="
cat "$REPORT_FILE" | grep -A 20 "Test Execution Summary"

echo ""
echo "All test results saved to: $RESULTS_DIR"
echo ""

# Create a quick summary file
SUMMARY_FILE="$RESULTS_DIR/SUMMARY.txt"
cat > "$SUMMARY_FILE" << EOF
InnHotel Test Suite - Quick Summary
====================================
Date: $(date)

Tests Run: $TESTS_RUN
Overall Status: $([ $OVERALL_EXIT -eq 0 ] && echo "PASSED ✓" || echo "FAILED ✗")

Individual Results:
- Database Verification: $([ $DB_EXIT -eq 0 ] && echo "PASSED" || echo "FAILED")
- API Health Check: $([ $API_EXIT -eq 0 ] && echo "PASSED" || echo "FAILED")
- Client Connectivity: $([ $CLIENT_EXIT -eq 0 ] && echo "PASSED" || echo "FAILED")

For detailed results, see comprehensive-test-report.txt
EOF

echo "Quick summary saved to: $SUMMARY_FILE"
echo ""

exit $OVERALL_EXIT