#!/bin/bash

# InnHotel Full-Stack Quick Start Script
# Run this script to start both API and Desktop Client

set -e

echo "🚀 InnHotel Full-Stack Quick Start"
echo "=================================="

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Check prerequisites
echo "📋 Checking prerequisites..."

# Check .NET
if ! command -v dotnet &> /dev/null; then
    echo -e "${RED}❌ .NET SDK is not installed${NC}"
    exit 1
fi

# Check Node.js
if ! command -v node &> /dev/null; then
    echo -e "${RED}❌ Node.js is not installed${NC}"
    exit 1
fi

# Check PostgreSQL
if ! command -v psql &> /dev/null; then
    echo -e "${RED}❌ PostgreSQL is not installed${NC}"
    exit 1
fi

echo -e "${GREEN}✅ All prerequisites met${NC}"

# Start PostgreSQL if not running
echo "🐘 Checking PostgreSQL..."
if ! pg_isready -q; then
    echo "Starting PostgreSQL..."
    sudo systemctl start postgresql
fi

# Setup database if not exists
echo "🗄️ Setting up database..."
sudo -u postgres psql -c "CREATE DATABASE innhotel_db;" 2>/dev/null || true
sudo -u postgres psql -c "CREATE USER innhotel_user WITH PASSWORD 'innhotel_secure_password';" 2>/dev/null || true
sudo -u postgres psql -c "GRANT ALL PRIVILEGES ON DATABASE innhotel_db TO innhotel_user;" 2>/dev/null || true

# Start API
echo "🔧 Starting API..."
cd innhotel-api/src/InnHotel.Web
dotnet ef database update --project ../InnHotel.Infrastructure/InnHotel.Infrastructure.csproj 2>/dev/null || true
dotnet run --urls "https://localhost:57679" &
API_PID=$!

# Wait for API to start
echo "⏳ Waiting for API to start..."
sleep 10

# Start Desktop Client
echo "🖥️ Starting Desktop Client..."
cd ../../../innhotel-desktop-client
npm install --silent
npm run dev &
CLIENT_PID=$!

echo ""
echo -e "${GREEN}✅ InnHotel Full-Stack is starting!${NC}"
echo ""
echo "📊 Services Status:"
echo "   API: https://localhost:57679"
echo "   Swagger: https://localhost:57679/swagger"
echo "   Desktop Client: Running in Electron"
echo ""
echo "🛑 To stop all services, run: kill $API_PID $CLIENT_PID"
echo ""

# Keep script running
wait