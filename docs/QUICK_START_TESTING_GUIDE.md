# InnHotel QA Testing - Quick Start Guide

## 🚀 Get Started in 5 Minutes

This quick start guide will help you begin testing the InnHotel system immediately.

---

## Prerequisites Check

Before starting, verify you have:

```bash
# Check .NET
dotnet --version  # Should be 9.0+

# Check Node.js
node --version    # Should be 20.x+

# Check PostgreSQL
psql --version    # Should be 15.x+

# Check Git
git --version     # Should be 2.x+
```

✅ All commands should return version numbers. If not, install missing software.

---

## Step 1: Clone and Setup (5 minutes)

```bash
# Clone repository
git clone https://github.com/HazemSoftPro/HotelTransylvania.git
cd HotelTransylvania

# Setup database
sudo -u postgres psql << EOF
CREATE DATABASE innhotel_db;
CREATE USER innhotel_user WITH PASSWORD 'innhotel_secure_password_2024';
GRANT ALL PRIVILEGES ON DATABASE innhotel_db TO innhotel_user;
\c innhotel_db
GRANT ALL ON SCHEMA public TO innhotel_user;
EOF
```

---

## Step 2: Start API (2 minutes)

```bash
# Terminal 1: Start API
cd innhotel-api/src/InnHotel.Web
dotnet restore
dotnet ef database update
dotnet run --urls "https://localhost:57679"

# Wait for: "Application started. Press Ctrl+C to shut down."
```

✅ API is ready when you see the startup message.

---

## Step 3: Start Client (2 minutes)

```bash
# Terminal 2: Start Client
cd innhotel-desktop-client
npm install
npm run dev:react

# Wait for: "Local: http://localhost:5173/"
```

✅ Client is ready when you see the local URL.

---

## Step 4: Run Automated Tests (1 minute)

```bash
# Terminal 3: Run tests
cd HotelTransylvania
chmod +x test-scripts/*.sh
./test-scripts/run-all-tests.sh
```

✅ All tests should pass (green checkmarks).

---

## Step 5: Manual Testing (Start Here)

### Test 1: Login (30 seconds)

1. Open browser: `http://localhost:5173`
2. Enter credentials:
   - Email: `super@innhotel.com`
   - Password: `Sup3rP@ssword!`
3. Click "Login"

✅ **Expected:** You should see the dashboard.

### Test 2: Create Branch (1 minute)

1. Click "Branches" in menu
2. Click "Add Branch"
3. Fill form:
   - Name: "Test Branch"
   - Address: "123 Test St"
   - Phone: "+1-555-0100"
   - Email: "test@innhotel.com"
4. Click "Save"

✅ **Expected:** Branch appears in list with success message.

### Test 3: API Test (30 seconds)

```bash
# Test API directly
curl -k -X POST https://localhost:57679/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"super@innhotel.com","password":"Sup3rP@ssword!"}'
```

✅ **Expected:** JSON response with accessToken.

---

## Common Issues & Quick Fixes

### Issue: API won't start
```bash
# Fix: Kill process on port
sudo lsof -i :57679
sudo kill -9 <PID>
```

### Issue: Database connection failed
```bash
# Fix: Restart PostgreSQL
sudo systemctl restart postgresql
```

### Issue: Client won't start
```bash
# Fix: Clear and reinstall
rm -rf node_modules package-lock.json
npm install
```

---

## Next Steps

Now that everything is working:

1. **Read the full guide:** `QA_TESTING_GUIDE.md`
2. **Use test templates:** Check `test-templates/` folder
3. **Run specific tests:** Use individual test scripts
4. **Report bugs:** Use `test-templates/bug-report-template.md`

---

## Test Scripts Reference

```bash
# Run all tests
./test-scripts/run-all-tests.sh

# Test database only
./test-scripts/database-verification.sh

# Test API only
./test-scripts/api-health-check.sh

# Test client only
./test-scripts/client-connectivity-test.sh
```

---

## Default Test Credentials

**SuperAdmin:**
- Email: `super@innhotel.com`
- Password: `Sup3rP@ssword!`

**Admin:**
- Email: `admin@innhotel.com`
- Password: `Adm1nP@ssword!`

---

## Service URLs

| Service | URL | Purpose |
|---------|-----|---------|
| Client | http://localhost:5173 | Desktop application |
| API | https://localhost:57679 | Backend API |
| Swagger | https://localhost:57679/swagger | API documentation |
| Health | https://localhost:57679/health | API health check |

---

## Getting Help

1. **Check troubleshooting:** `troubleshooting.md` in project root
2. **Review full guide:** `QA_TESTING_GUIDE.md`
3. **Check test scripts:** `test-scripts/README.md`
4. **Create issue:** GitHub repository issues

---

## Testing Checklist

Use this checklist for your first test session:

- [ ] All prerequisites installed
- [ ] Repository cloned
- [ ] Database created and configured
- [ ] API running on port 57679
- [ ] Client running on port 5173
- [ ] Automated tests pass
- [ ] Can login successfully
- [ ] Can create a branch
- [ ] Can view dashboard
- [ ] API responds to curl requests
- [ ] No console errors in browser

---

## Time Estimate

- **Setup:** 10 minutes
- **First test session:** 30 minutes
- **Full testing cycle:** 4-8 hours

---

**Ready to start? Open the full guide: `QA_TESTING_GUIDE.md`**

**Questions? Check: `test-scripts/README.md` or `troubleshooting.md`**