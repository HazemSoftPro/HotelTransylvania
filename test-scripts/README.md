# InnHotel Test Scripts

This directory contains automated test scripts for the InnHotel system.

## Available Scripts

### 1. run-all-tests.sh
**Master script that runs all tests and generates a comprehensive report.**

**Usage:**
```bash
chmod +x test-scripts/run-all-tests.sh
./test-scripts/run-all-tests.sh
```

**What it does:**
- Runs all test scripts in sequence
- Generates a comprehensive test report
- Creates a results directory with all test outputs
- Provides overall pass/fail status

**Output:**
- Creates a timestamped directory: `test-results-YYYYMMDD_HHMMSS/`
- Generates `comprehensive-test-report.txt`
- Generates `SUMMARY.txt` for quick overview

---

### 2. database-verification.sh
**Verifies database connectivity and schema.**

**Usage:**
```bash
chmod +x test-scripts/database-verification.sh
./test-scripts/database-verification.sh
```

**Tests performed:**
- Database connection test
- Required tables existence check
- Table structure validation
- Seed data verification
- Foreign key constraints check
- Index verification
- Database size check
- Active connections count

**Output:**
- Console output with color-coded results
- `database-verification-results.txt` file

**Prerequisites:**
- PostgreSQL must be running
- Database `innhotel_db` must exist
- User `innhotel_user` must have access

---

### 3. api-health-check.sh
**Tests API endpoints and functionality.**

**Usage:**
```bash
chmod +x test-scripts/api-health-check.sh
./test-scripts/api-health-check.sh
```

**Tests performed:**
- Health endpoint check
- Swagger documentation accessibility
- Authentication endpoints (without auth)
- Protected endpoints (unauthorized access)
- Login with valid credentials
- Protected endpoints (with authentication)
- Token-based access verification

**Output:**
- Console output with color-coded results
- `api-health-check-results.txt` file

**Prerequisites:**
- API must be running on https://localhost:57679
- Database must be accessible
- Default users must be seeded

---

### 4. client-connectivity-test.sh
**Tests client application and API connectivity.**

**Usage:**
```bash
chmod +x test-scripts/client-connectivity-test.sh
./test-scripts/client-connectivity-test.sh
```

**Tests performed:**
- Client server availability
- HTML content validation
- React root element check
- API connectivity from client
- CORS configuration
- Client assets loading
- Response time measurement
- Port availability check

**Output:**
- Console output with color-coded results
- `client-connectivity-results.txt` file

**Prerequisites:**
- Client must be running on http://localhost:5173
- API must be running on https://localhost:57679

---

## Quick Start

### Run All Tests
```bash
# Make all scripts executable
chmod +x test-scripts/*.sh

# Run comprehensive test suite
./test-scripts/run-all-tests.sh
```

### Run Individual Tests
```bash
# Test database only
./test-scripts/database-verification.sh

# Test API only
./test-scripts/api-health-check.sh

# Test client only
./test-scripts/client-connectivity-test.sh
```

---

## Understanding Results

### Exit Codes
- `0` = All tests passed
- `Non-zero` = Some tests failed (number indicates failure count)

### Color Coding
- 🟢 **GREEN (✓)** = Test passed
- 🔴 **RED (✗)** = Test failed
- 🟡 **YELLOW (⚠)** = Warning or informational

### Result Files
Each script generates a detailed result file:
- Plain text format
- Timestamped
- Contains all test results
- Includes summary statistics

---

## Troubleshooting

### Database Tests Failing
```bash
# Check PostgreSQL is running
sudo systemctl status postgresql

# Test connection manually
psql -h localhost -U innhotel_user -d innhotel_db

# Check credentials in script
```

### API Tests Failing
```bash
# Check API is running
curl -k https://localhost:57679/health

# Check API logs
cd innhotel-api/src/InnHotel.Web
cat log.txt

# Restart API
dotnet run
```

### Client Tests Failing
```bash
# Check client is running
curl http://localhost:5173

# Check for errors
# Open browser console at http://localhost:5173

# Restart client
cd innhotel-desktop-client
npm run dev:react
```

---

## Customization

### Modify Connection Parameters

**Database (database-verification.sh):**
```bash
DB_HOST="localhost"
DB_PORT="5432"
DB_NAME="innhotel_db"
DB_USER="innhotel_user"
DB_PASSWORD="innhotel_secure_password_2024"
```

**API (api-health-check.sh):**
```bash
API_BASE_URL="https://localhost:57679"
```

**Client (client-connectivity-test.sh):**
```bash
CLIENT_URL="http://localhost:5173"
API_URL="https://localhost:57679"
```

---

## Integration with CI/CD

These scripts can be integrated into CI/CD pipelines:

```yaml
# Example GitHub Actions workflow
- name: Run Tests
  run: |
    chmod +x test-scripts/run-all-tests.sh
    ./test-scripts/run-all-tests.sh
    
- name: Upload Test Results
  uses: actions/upload-artifact@v2
  with:
    name: test-results
    path: test-results-*/
```

---

## Best Practices

1. **Run tests before committing code**
2. **Run full test suite after major changes**
3. **Review result files for detailed information**
4. **Keep test scripts updated with system changes**
5. **Document any test failures**

---

## Support

For issues or questions:
1. Check the main QA Testing Guide
2. Review troubleshooting.md in project root
3. Check individual result files for details
4. Create an issue on GitHub repository

---

**Last Updated:** 2025-10-18