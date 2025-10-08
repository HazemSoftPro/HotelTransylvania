# InnHotel Troubleshooting Guide

## Quick Diagnostic Commands

### Check All Services
```bash
# Check PostgreSQL
sudo systemctl status postgresql

# Check .NET API
curl -f https://localhost:57679/health || echo "API down"

# Check client
netstat -plntu | grep :5173
```

### Database Issues

#### Connection Refused
```bash
# Check PostgreSQL is listening
sudo netstat -plntu | grep 5432

# Check firewall
sudo ufw status | grep 5432

# Test connection
psql -h localhost -U innhotel_user -d innhotel_db -c "SELECT 1;"
```

#### Permission Denied
```bash
# Fix user permissions
sudo -u postgres psql -c "GRANT ALL PRIVILEGES ON DATABASE innhotel_db TO innhotel_user;"
sudo -u postgres psql -c "GRANT USAGE ON SCHEMA public TO innhotel_user;"
```

### API Issues

#### Port Already in Use
```bash
# Find process using port 57679
sudo lsof -i :57679

# Kill process if needed
sudo kill -9 <PID>

# Or use different port
dotnet run --urls "https://localhost:57680"
```

#### SSL Certificate Issues
```bash
# Clean and regenerate certificates
dotnet dev-certs https --clean
dotnet dev-certs https --trust

# Verify certificate
dotnet dev-certs https --check
```

#### Entity Framework Issues
```bash
# Reset migrations
dotnet ef database drop --force
dotnet ef database update

# Check migration status
dotnet ef migrations list

# Remove and recreate migrations
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
```

### Client Issues

#### Node Modules Issues
```bash
# Clear all caches
npm cache clean --force
rm -rf node_modules package-lock.json
npm install

# Check for vulnerabilities
npm audit
npm audit fix
```

#### Electron Issues
```bash
# Clear Electron cache
rm -rf ~/.cache/electron
rm -rf ~/.config/Electron

# Rebuild native modules
npm rebuild

# Check Electron version
npx electron --version
```

#### Build Issues
```bash
# Clear build cache
rm -rf dist dist-electron

# Force rebuild
npm run build
npm run dist:win
```

### CORS Issues

#### Browser/Client can't connect to API
```bash
# Check API is running
curl -I https://localhost:57679

# Check CORS configuration
# Update .env file:
ALLOWED_ORIGINS=http://localhost:3000,http://localhost:5173,https://localhost:5173
```

### Authentication Issues

#### JWT Token Problems
```bash
# Check token expiration
# Test with curl
curl -X POST https://localhost:57679/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"password"}'
```

#### Database Connection String Issues
```bash
# Test connection string
psql "Host=localhost;Port=5432;Username=innhotel_user;Password=innhotel_secure_password_2024;Database=innhotel_db" -c "SELECT 1;"
```

## Log Files Location

### API Logs
- **Development**: Console output
- **File logging**: Check `logs/` directory in API project
- **Windows**: `%APPDATA%\InnHotel\logs\`
- **Linux/macOS**: `~/.local/share/InnHotel/logs/`

### Client Logs
- **Development**: Electron console (Ctrl+Shift+I)
- **Production**: Check `~/.config/InnHotel/logs/`

## Performance Issues

### Database Performance
```sql
-- Check slow queries
SELECT query, mean_time, calls 
FROM pg_stat_statements 
ORDER BY mean_time DESC 
LIMIT 10;

-- Check table sizes
SELECT schemaname, tablename, 
       pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) as size
FROM pg_tables 
WHERE schemaname NOT IN ('information_schema', 'pg_catalog')
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC;
```

### Memory Usage
```bash
# Check .NET memory usage
dotnet-counters monitor --process-id <PID>

# Check Node.js memory
node --max-old-space-size=4096 your-script.js
```

## Environment-Specific Issues

### Windows-Specific
```powershell
# Fix Windows path issues
$env:PATH += ";C:\Program Files\PostgreSQL\15\bin"
$env:PATH += ";C:\Program Files\dotnet&quot;
```

### macOS-Specific
```bash
# Fix macOS permissions
sudo chown -R $(whoami) /usr/local/lib/node_modules
sudo chown -R $(whoami) ~/.npm
```

### Linux-Specific
```bash
# Fix Linux permissions
sudo usermod -a -G postgres $USER
sudo systemctl restart postgresql
```

## Diagnostic Scripts

### Full Health Check
```bash
#!/bin/bash
echo "=== InnHotel Health Check ==="

echo "1. Checking PostgreSQL..."
pg_isready -h localhost -p 5432 && echo "✅ PostgreSQL OK" || echo "❌ PostgreSQL DOWN"

echo "2. Checking .NET API..."
curl -f https://localhost:57679/health > /dev/null 2>&1 && echo "✅ API OK" || echo "❌ API DOWN"

echo "3. Checking ports..."
netstat -plntu | grep -E "5432|57679|5173" && echo "✅ Ports OK" || echo "❌ Ports not listening"

echo "4. Checking disk space..."
df -h | grep -E "(/$|/home)" && echo "✅ Disk space OK"

echo "5. Checking memory..."
free -h && echo "✅ Memory check complete"

echo "=== Health check complete ==="
```

## Getting Help

### Create Debug Report
```bash
#!/bin/bash
echo "Creating debug report..."
{
    echo "=== System Info ==="
    uname -a
    echo ""
    echo "=== Versions ==="
    dotnet --version
    node --version
    npm --version
    psql --version
    echo ""
    echo "=== Services Status ==="
    sudo systemctl status postgresql
    echo ""
    echo "=== Network ==="
    netstat -plntu | grep -E "5432|57679|5173"
} > debug_report.txt 2>&1
echo "Debug report saved to debug_report.txt"
```

### GitHub Issues Template
When reporting issues, include:
1. Operating system and version
2. .NET version (`dotnet --version`)
3. Node.js version (`node --version`)
4. PostgreSQL version (`psql --version`)
5. Complete error messages
6. Steps to reproduce
7. Output of health check script