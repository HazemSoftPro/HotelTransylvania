# Hotel Transylvania - Integrated System Deployment Guide

## 🚀 Overview

This guide covers the deployment of the fully integrated Hotel Transylvania system with real-time communication, event-driven architecture, and cross-module integration.

## 📋 Prerequisites

### Infrastructure Requirements

**Minimum Hardware Specifications:**
- **Application Server**: 4 CPU cores, 8GB RAM, 50GB SSD
- **Database Server**: 2 CPU cores, 4GB RAM, 100GB SSD
- **Load Balancer**: 2 CPU cores, 2GB RAM (if using HA)
- **Redis Server**: 1 CPU core, 2GB RAM (for caching and SignalR scaleout)

**Software Requirements:**
- **Operating System**: Ubuntu 20.04+ / CentOS 8+ / Windows Server 2019+
- **.NET 9 Runtime**: Latest version
- **PostgreSQL**: Version 15.x
- **Redis**: Version 7.x (for SignalR scaleout and caching)
- **Nginx** (or IIS): Version 1.20+
- **Docker**: Version 24.x (optional)
- **Docker Compose**: Version 2.x (optional)

### Network Requirements

- **HTTPS**: SSL/TLS certificate required
- **Ports**: 80 (HTTP), 443 (HTTPS), 5001 (SignalR)
- **Bandwidth**: Minimum 100 Mbps
- **Firewall**: Allow incoming traffic on required ports

## 🏗️ Architecture Overview

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Load Balancer │    │   Web Server    │    │  Database Server│
│    (Nginx/IIS)  │────│   (.NET API)    │────│   (PostgreSQL)  │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         │              ┌─────────────────┐              │
         │              │  SignalR Hub    │              │
         └──────────────│  (Real-time)    │──────────────┘
                        └─────────────────┘
                                │
                       ┌─────────────────┐
                       │  Redis Server   │
                       │ (SignalR Scale) │
                       └─────────────────┘
```

## 📦 Installation Steps

### Step 1: Database Setup

#### PostgreSQL Installation

```bash
# Ubuntu/Debian
sudo apt update
sudo apt install postgresql postgresql-contrib

# RHEL/CentOS
sudo yum install postgresql-server postgresql-contrib
sudo postgresql-setup initdb
sudo systemctl enable postgresql
sudo systemctl start postgresql

# Create database and user
sudo -u postgres psql
CREATE DATABASE innhotel_prod;
CREATE USER innhotel_prod WITH PASSWORD 'your_secure_password';
GRANT ALL PRIVILEGES ON DATABASE innhotel_prod TO innhotel_prod;
\q
```

#### Database Migration

```bash
cd /path/to/HotelTransylvania/innhotel-api/src/InnHotel.Web
export ConnectionStrings__PostgreSQLConnection="Host=localhost;Port=5432;Database=innhotel_prod;Username=innhotel_prod;Password=your_secure_password"

# Apply migrations
dotnet ef database update --project ../InnHotel.Infrastructure
```

### Step 2: Redis Setup (for SignalR Scaleout)

```bash
# Ubuntu/Debian
sudo apt install redis-server

# Configure Redis
sudo nano /etc/redis/redis.conf
# Ensure these settings:
# bind 127.0.0.1
# port 6379
# requirepass your_redis_password

sudo systemctl enable redis
sudo systemctl start redis
```

### Step 3: Application Configuration

#### Environment Configuration

Create `/etc/innhotel/appsettings.Production.json`:

```json
{
  "ConnectionStrings": {
    "PostgreSQLConnection": "Host=localhost;Port=5432;Database=innhotel_prod;Username=innhotel_prod;Password=your_secure_password;Pooling=true;MinimumPoolSize=10;MaximumPoolSize=100"
  },
  "JwtSettings": {
    "Secret": "your_production_jwt_secret_key_at_least_32_characters",
    "ExpirationHours": 24,
    "Issuer": "HotelTransylvania",
    "Audience": "HotelTransylvania-Users"
  },
  "Redis": {
    "ConnectionString": "localhost:6379,password=your_redis_password"
  },
  "SignalR": {
    "EnableDetailedErrors": false,
    "EnableRedisScaleout": true
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error",
      "InnHotel": "Information"
    },
    "File": {
      "Path": "/var/log/innhotel/app.log",
      "RollingInterval": "Day",
      "RetainedFileCountLimit": 30
    }
  },
  "AllowedOrigins": [
    "https://yourdomain.com",
    "https://www.yourdomain.com"
  ]
}
```

### Step 4: Application Deployment

#### Build and Publish

```bash
cd /path/to/HotelTransylvania/innhotel-api/src/InnHotel.Web

# Build for production
dotnet publish -c Release -o /var/www/innhotel-api

# Set permissions
sudo chown -R www-data:www-data /var/www/innhotel-api
sudo chmod -R 755 /var/www/innhotel-api
```

#### Systemd Service Configuration

Create `/etc/systemd/system/innhotel-api.service`:

```ini
[Unit]
Description=Hotel Transylvania API
After=network.target

[Service]
Type=notify
WorkingDirectory=/var/www/innhotel-api
ExecStart=/usr/bin/dotnet /var/www/innhotel-api/InnHotel.Web.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=innhotel-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

Enable and start the service:

```bash
sudo systemctl daemon-reload
sudo systemctl enable innhotel-api
sudo systemctl start innhotel-api
```

### Step 5: Reverse Proxy Configuration

#### Nginx Configuration

Create `/etc/nginx/sites-available/innhotel`:

```nginx
server {
    listen 80;
    server_name yourdomain.com www.yourdomain.com;
    return 301 https://$server_name$request_uri;
}

server {
    listen 443 ssl http2;
    server_name yourdomain.com www.yourdomain.com;

    ssl_certificate /path/to/your/certificate.crt;
    ssl_certificate_key /path/to/your/private.key;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers ECDHE-RSA-AES256-GCM-SHA512:DHE-RSA-AES256-GCM-SHA512:ECDHE-RSA-AES256-GCM-SHA384:DHE-RSA-AES256-GCM-SHA384;
    ssl_prefer_server_ciphers off;

    # API endpoints
    location /api/ {
        proxy_pass http://127.0.0.1:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
        proxy_read_timeout 300s;
        proxy_connect_timeout 75s;
    }

    # SignalR hub
    location /hotelHub {
        proxy_pass http://127.0.0.1:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "upgrade";
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }

    # Static files and frontend
    location / {
        proxy_pass http://127.0.0.1:5000;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }

    # WebSocket optimization for SignalR
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
        proxy_pass http://127.0.0.1:5000;
    }
}
```

Enable the site:

```bash
sudo ln -s /etc/nginx/sites-available/innhotel /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx
```

### Step 6: SSL Certificate Setup

#### Let's Encrypt (Recommended)

```bash
# Install Certbot
sudo apt install certbot python3-certbot-nginx

# Get certificate
sudo certbot --nginx -d yourdomain.com -d www.yourdomain.com

# Set up auto-renewal
sudo crontab -e
# Add: 0 12 * * * /usr/bin/certbot renew --quiet
```

## 🐳 Docker Deployment (Alternative)

### Docker Compose Configuration

Create `docker-compose.prod.yml`:

```yaml
version: '3.8'

services:
  db:
    image: postgres:15
    environment:
      POSTGRES_DB: innhotel_prod
      POSTGRES_USER: innhotel_prod
      POSTGRES_PASSWORD: your_secure_password
    volumes:
      - postgres_data:/var/lib/postgresql/data
    ports:
      - "5432:5432"
    restart: unless-stopped

  redis:
    image: redis:7-alpine
    command: redis-server --requirepass your_redis_password
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    restart: unless-stopped

  api:
    build:
      context: ./innhotel-api
      dockerfile: Dockerfile.prod
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__PostgreSQLConnection=Host=db;Port=5432;Database=innhotel_prod;Username=innhotel_prod;Password=your_secure_password
      - Redis__ConnectionString=redis:6379,password=your_redis_password
    depends_on:
      - db
      - redis
    ports:
      - "5000:80"
    restart: unless-stopped

  nginx:
    image: nginx:alpine
    ports:
      - "80:80"
      - "443:443"
    volumes:
      - ./nginx.conf:/etc/nginx/nginx.conf
      - ./ssl:/etc/nginx/ssl
    depends_on:
      - api
    restart: unless-stopped

volumes:
  postgres_data:
  redis_data:
```

### Dockerfile for API

Create `innhotel-api/Dockerfile.prod`:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/InnHotel.Web/InnHotel.Web.csproj", "src/InnHotel.Web/"]
COPY ["src/InnHotel.Core/InnHotel.Core.csproj", "src/InnHotel.Core/"]
COPY ["src/InnHotel.UseCases/InnHotel.UseCases.csproj", "src/InnHotel.UseCases/"]
COPY ["src/InnHotel.Infrastructure/InnHotel.Infrastructure.csproj", "src/InnHotel.Infrastructure/"]
RUN dotnet restore "src/InnHotel.Web/InnHotel.Web.csproj"
COPY . .
WORKDIR "/src/src/InnHotel.Web"
RUN dotnet build "InnHotel.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InnHotel.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnHotel.Web.dll"]
```

Deploy with Docker:

```bash
docker-compose -f docker-compose.prod.yml up -d
```

## 🔧 Configuration Tuning

### PostgreSQL Optimization

```sql
-- Connect to your database
\c innhotel_prod

-- Performance settings
ALTER SYSTEM SET shared_buffers = '256MB';
ALTER SYSTEM SET effective_cache_size = '1GB';
ALTER SYSTEM SET maintenance_work_mem = '64MB';
ALTER SYSTEM SET checkpoint_completion_target = 0.9;
ALTER SYSTEM SET wal_buffers = '16MB';
ALTER SYSTEM SET default_statistics_target = 100;
ALTER SYSTEM SET random_page_cost = 1.1;
ALTER SYSTEM SET effective_io_concurrency = 200;

-- Apply changes
SELECT pg_reload_conf();
```

### Nginx Optimization

```nginx
# Add to nginx.conf
worker_processes auto;
worker_connections 1024;

# Gzip compression
gzip on;
gzip_vary on;
gzip_min_length 1024;
gzip_types text/plain text/css application/json application/javascript text/xml application/xml application/xml+rss text/javascript;

# Rate limiting
limit_req_zone $binary_remote_addr zone=api:10m rate=10r/s;
limit_req_zone $binary_remote_addr zone=login:10m rate=1r/s;

# Apply in server block
location /api/login {
    limit_req zone=login burst=5 nodelay;
    proxy_pass http://127.0.0.1:5000;
}
```

## 🔍 Monitoring & Logging

### Application Monitoring

Install monitoring tools:

```bash
# Application logs
sudo mkdir -p /var/log/innhotel
sudo chown www-data:www-data /var/log/innhotel

# Log rotation
sudo nano /etc/logrotate.d/innhotel
```

Content for log rotation:

```
/var/log/innhotel/*.log {
    daily
    missingok
    rotate 30
    compress
    delaycompress
    notifempty
    create 644 www-data www-data
    postrotate
        systemctl reload innhotel-api
    endscript
}
```

### Health Checks

Add health check endpoint monitoring:

```bash
# Create health check script
cat > /usr/local/bin/innhotel-health-check.sh << 'EOF'
#!/bin/bash
response=$(curl -s -o /dev/null -w "%{http_code}" http://localhost:5000/health)
if [ $response != "200" ]; then
    echo "API health check failed with status $response"
    systemctl restart innhotel-api
fi
EOF

chmod +x /usr/local/bin/innhotel-health-check.sh

# Add to crontab (every 5 minutes)
echo "*/5 * * * * /usr/local/bin/innhotel-health-check.sh" | crontab -
```

## 🔒 Security Considerations

### Application Security

1. **Environment Variables**: Store secrets in environment variables or secure vault
2. **Database Security**: Use strong passwords, limit connections, enable SSL
3. **Network Security**: Configure firewall, use VPN for admin access
4. **Regular Updates**: Keep OS, .NET, PostgreSQL, and Nginx updated

### SSL/TLS Configuration

```nginx
# Strong SSL configuration
ssl_protocols TLSv1.2 TLSv1.3;
ssl_ciphers ECDHE-ECDSA-AES128-GCM-SHA256:ECDHE-RSA-AES128-GCM-SHA256:ECDHE-ECDSA-AES256-GCM-SHA384:ECDHE-RSA-AES256-GCM-SHA384;
ssl_prefer_server_ciphers off;
ssl_session_cache shared:SSL:10m;
ssl_session_timeout 10m;

# HSTS
add_header Strict-Transport-Security "max-age=31536000; includeSubDomains" always;

# Security headers
add_header X-Frame-Options DENY;
add_header X-Content-Type-Options nosniff;
add_header X-XSS-Protection "1; mode=block";
add_header Referrer-Policy strict-origin-when-cross-origin;
```

## 📊 Performance Optimization

### Caching Strategy

```json
// appsettings.Production.json
{
  "Caching": {
    "EnableRedisCache": true,
    "DefaultExpiration": "00:15:00",
    "CacheKeys": {
      "RoomAvailability": "rooms:availability:",
      "GuestProfile": "guests:profile:",
      "DashboardMetrics": "dashboard:metrics:",
      "ReservationData": "reservations:data:"
    }
  }
}
```

### Database Connection Pooling

```json
{
  "ConnectionStrings": {
    "PostgreSQLConnection": "Host=localhost;Port=5432;Database=innhotel_prod;Username=innhotel_prod;Password=your_secure_password;Pooling=true;MinimumPoolSize=10;MaximumPoolSize=100;ConnectionIdleLifetime=300;"
  }
}
```

## 🚨 Troubleshooting

### Common Issues

1. **Application Won't Start**
   ```bash
   # Check logs
   sudo journalctl -u innhotel-api -f
   
   # Check configuration
   dotnet /var/www/innhotel-api/InnHotel.Web.dll --dry-run
   ```

2. **Database Connection Issues**
   ```bash
   # Test connection
   psql -h localhost -U innhotel_prod -d innhotel_prod -c "SELECT version();"
   ```

3. **SignalR Connection Issues**
   ```bash
   # Check Redis
   redis-cli ping
   
   # Check Redis logs
   sudo journalctl -u redis -f
   ```

4. **Performance Issues**
   ```bash
   # Check system resources
   top
   htop
   iotop
   
   # Check database performance
   sudo -u postgres psql -d innhotel_prod -c "SELECT * FROM pg_stat_activity;"
   ```

## 🔄 Backup & Recovery

### Database Backup

```bash
# Create backup script
cat > /usr/local/bin/innhotel-backup.sh << 'EOF'
#!/bin/bash
BACKUP_DIR="/var/backups/innhotel"
DATE=$(date +%Y%m%d_%H%M%S)
BACKUP_FILE="$BACKUP_DIR/innhotel_backup_$DATE.sql"

mkdir -p $BACKUP_DIR
pg_dump -h localhost -U innhotel_prod -d innhotel_prod > $BACKUP_FILE
gzip $BACKUP_FILE

# Keep only last 7 days
find $BACKUP_DIR -name "*.sql.gz" -mtime +7 -delete
EOF

chmod +x /usr/local/bin/innhotel-backup.sh

# Schedule daily backups
echo "0 2 * * * /usr/local/bin/innhotel-backup.sh" | crontab -
```

### Application Backup

```bash
# Backup application files
tar -czf /var/backups/innhotel/app_backup_$(date +%Y%m%d).tar.gz /var/www/innhotel-api
```

## 📈 Scaling Considerations

### Horizontal Scaling

1. **Load Balancer**: Use Nginx or dedicated load balancer
2. **Multiple API Instances**: Run multiple instances behind load balancer
3. **Database Read Replicas**: Use PostgreSQL streaming replication
4. **Redis Cluster**: For high availability and scaling

### Monitoring Scaling

- Use Prometheus + Grafana for metrics
- Implement distributed tracing
- Set up alerting for critical issues

---

## ✅ Verification Checklist

After deployment, verify:

- [ ] API is accessible via HTTPS
- [ ] Database connectivity is working
- [ ] SignalR hub is functional
- [ ] Real-time updates are working
- [ ] Authentication is working
- [ ] All modules are integrated
- [ ] Dashboard shows real data
- [ ] Notifications are being sent
- [ ] Performance is acceptable
- [ ] Security headers are present
- [ ] Logs are being generated
- [ ] Backup system is working

## 🆘 Support

For deployment issues:

1. Check application logs: `sudo journalctl -u innhotel-api -f`
2. Check database logs: `sudo tail -f /var/log/postgresql/postgresql-15-main.log`
3. Check Nginx logs: `sudo tail -f /var/log/nginx/error.log`
4. Review this documentation
5. Contact support at support@hoteltransylvania.com

---

**Deployment completed successfully! 🎉**

The Hotel Transylvania integrated system is now running with full real-time capabilities, event-driven architecture, and cross-module integration.