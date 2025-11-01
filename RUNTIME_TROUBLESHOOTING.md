# 🏨 HotelTransylvania - Runtime Troubleshooting Guide

## 🚀 Quick Start

### Prerequisites
- **Node.js 18+** (for client)
- **.NET SDK 8.0** (for API)
- **pnpm** (package manager)

### One-Command Startup
```bash
./start-project.sh
```

## 🔍 Common Runtime Issues & Solutions

### 1. **API Connection Issues**

#### Problem: "API server is not available"
**Symptoms:**
- Client shows network errors
- Login fails immediately
- No data loads in any page

**Solutions:**
```bash
# Check if API is running
curl http://localhost:57679/api/health

# If not running, start API manually:
cd innhotel-api/src/InnHotel.Web
dotnet run --urls="http://localhost:57679"
```

#### Problem: CORS Errors
**Symptoms:**
- Browser console shows CORS policy errors
- API calls blocked by browser

**Solution:**
Ensure `innhotel-api/.env` contains:
```
ALLOWED_ORIGINS=http://localhost:5173,http://localhost:3000,https://localhost:5173
```

### 2. **Database Issues**

#### Problem: PostgreSQL Connection Failed
**Symptoms:**
- API fails to start
- Database connection errors in logs

**Solutions:**

**Option A: Use SQLite (Recommended for Development)**
- The project is already configured to use SQLite in development mode
- No additional setup required

**Option B: Use PostgreSQL**
```bash
# Using Docker
docker run --name innhotel-postgres \
  -e POSTGRES_DB=innhotel-db \
  -e POSTGRES_USER=innhotel_user \
  -e POSTGRES_PASSWORD=innhotel \
  -p 5432:5432 -d postgres:15-alpine

# Or using docker-compose
docker-compose up postgres -d
```

### 3. **Client Issues**

#### Problem: Pages Not Loading/Blank Screen
**Symptoms:**
- White screen after login
- Routes not working
- Components not rendering

**Solutions:**
```bash
# Clear cache and reinstall dependencies
cd innhotel-desktop-client
rm -rf node_modules pnpm-lock.yaml
pnpm install

# Check for JavaScript errors in browser console
# Press F12 → Console tab
```

#### Problem: Authentication Loop
**Symptoms:**
- Redirected to login repeatedly
- Token refresh fails
- "Unauthorized" errors

**Solutions:**
1. Clear browser storage:
   ```javascript
   // In browser console:
   localStorage.clear();
   sessionStorage.clear();
   ```

2. Check API is running and accessible

3. Verify default credentials:
   - Super Admin: `super@innhotel.com` / `Sup3rP@ssword!`
   - Admin: `admin@innhotel.com` / `Adm1nP@ssword!`

### 4. **Real-time Connection Issues**

#### Problem: SignalR Connection Failed
**Symptoms:**
- Real-time updates not working
- WebSocket connection errors
- Room status not updating

**Solutions:**
```bash
# Check if SignalR hub is accessible
curl http://localhost:57679/api/hubs/roomstatus

# Verify WebSocket support in browser
# Check browser console for SignalR connection logs
```

## 🛠️ Development Setup

### Environment Files

**innhotel-api/.env:**
```
ALLOWED_ORIGINS=http://localhost:5173,http://localhost:3000,https://localhost:5173
ASPNETCORE_ENVIRONMENT=Development
```

**innhotel-desktop-client/.env:**
```
NODE_ENV=development
VITE_API_BASE_URL="http://localhost:57679/api"
VITE_ENABLE_LOGGING=false
```

### Port Configuration
- **API**: http://localhost:57679
- **Client**: http://localhost:5173
- **Database**: localhost:5432 (PostgreSQL) or SQLite file

## 🧪 Testing the Setup

### 1. Test API Health
```bash
curl http://localhost:57679/api/health
# Expected: 200 OK response
```

### 2. Test Authentication
```bash
curl -X POST http://localhost:57679/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@innhotel.com","password":"Adm1nP@ssword!"}'
# Expected: JWT token response
```

### 3. Test Client Access
- Open http://localhost:5173
- Should show login page
- Login with default credentials
- Should redirect to dashboard

## 🐛 Debugging Tips

### Enable Detailed Logging

**API Logging:**
- Logs are written to console and `log.txt`
- Check `innhotel-api/src/InnHotel.Web/log*.txt` files

**Client Logging:**
```bash
# Set in .env file:
VITE_ENABLE_LOGGING=true
```

### Browser Developer Tools
1. **Network Tab**: Check API calls and responses
2. **Console Tab**: Look for JavaScript errors
3. **Application Tab**: Check localStorage/sessionStorage
4. **Sources Tab**: Set breakpoints for debugging

### Common Error Codes
- **ECONNREFUSED**: API server not running
- **401 Unauthorized**: Authentication issues
- **403 Forbidden**: Permission issues
- **404 Not Found**: Route or endpoint issues
- **500 Internal Server Error**: API server errors

## 📋 System Requirements

### Minimum Requirements
- **OS**: Windows 10, macOS 10.15, Ubuntu 18.04+
- **RAM**: 4GB minimum, 8GB recommended
- **Storage**: 2GB free space
- **Network**: Internet connection for package downloads

### Recommended Development Environment
- **IDE**: Visual Studio Code with extensions:
  - C# Dev Kit
  - React/TypeScript extensions
  - REST Client
- **Browser**: Chrome/Edge with React Developer Tools

## 🆘 Getting Help

### Log Locations
- **API Logs**: `innhotel-api/src/InnHotel.Web/log*.txt`
- **Client Logs**: Browser console (F12)
- **System Logs**: Check terminal output

### Useful Commands
```bash
# Check running processes
ps aux | grep -E "(dotnet|node|vite)"

# Check port usage
lsof -i :57679  # API port
lsof -i :5173   # Client port

# Kill processes if needed
pkill -f "dotnet run"
pkill -f "vite"
```

### Reset Everything
```bash
# Stop all processes
pkill -f "dotnet run"
pkill -f "vite"

# Clean client
cd innhotel-desktop-client
rm -rf node_modules .vite
pnpm install

# Clean API (if needed)
cd ../innhotel-api/src/InnHotel.Web
dotnet clean
dotnet restore

# Restart
cd ../../..
./start-project.sh
```

## 📞 Support Information

If you encounter issues not covered in this guide:

1. **Check the logs** first (API and browser console)
2. **Verify prerequisites** are installed correctly
3. **Test individual components** (API, Client, Database)
4. **Clear caches** and restart services
5. **Check network connectivity** and firewall settings

---

*Last updated: October 2024*

