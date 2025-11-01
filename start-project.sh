#!/bin/bash

# HotelTransylvania Project Startup Script
# This script starts both the API and Client components

set -e

echo "🏨 Starting HotelTransylvania Project..."

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to check if command exists
command_exists() {
    command -v "$1" >/dev/null 2>&1
}

# Function to check if port is in use
port_in_use() {
    lsof -i :$1 >/dev/null 2>&1
}

echo -e "${BLUE}📋 Checking prerequisites...${NC}"

# Check Node.js
if ! command_exists node; then
    echo -e "${RED}❌ Node.js is not installed. Please install Node.js 18+ first.${NC}"
    exit 1
fi

# Check .NET
if ! command_exists dotnet; then
    echo -e "${YELLOW}⚠️  .NET SDK is not installed.${NC}"
    echo -e "${YELLOW}   Installing .NET SDK...${NC}"
    
    # Try to install .NET SDK (Ubuntu/Debian)
    if command_exists apt-get; then
        wget -q https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
        sudo dpkg -i packages-microsoft-prod.deb
        sudo apt-get update
        sudo apt-get install -y dotnet-sdk-8.0
        rm packages-microsoft-prod.deb
    else
        echo -e "${RED}❌ Please install .NET SDK 8.0 manually from: https://dotnet.microsoft.com/download${NC}"
        exit 1
    fi
fi

# Check pnpm
if ! command_exists pnpm; then
    echo -e "${YELLOW}⚠️  pnpm is not installed. Installing...${NC}"
    corepack enable
fi

echo -e "${GREEN}✅ Prerequisites check completed${NC}"

# Check ports
if port_in_use 57679; then
    echo -e "${YELLOW}⚠️  Port 57679 is already in use. Please stop the service using this port.${NC}"
fi

if port_in_use 5173; then
    echo -e "${YELLOW}⚠️  Port 5173 is already in use. Please stop the service using this port.${NC}"
fi

# Install client dependencies
echo -e "${BLUE}📦 Installing client dependencies...${NC}"
cd innhotel-desktop-client
if [ ! -d "node_modules" ]; then
    pnpm install
else
    echo -e "${GREEN}✅ Dependencies already installed${NC}"
fi
cd ..

# Create .env file if it doesn't exist
if [ ! -f "innhotel-desktop-client/.env" ]; then
    echo -e "${BLUE}📝 Creating client .env file...${NC}"
    cp innhotel-desktop-client/.env.example innhotel-desktop-client/.env
fi

# Create API .env file if it doesn't exist
if [ ! -f "innhotel-api/.env" ]; then
    echo -e "${BLUE}📝 Creating API .env file...${NC}"
    echo "ALLOWED_ORIGINS=http://localhost:5173,http://localhost:3000,https://localhost:5173" > innhotel-api/.env
    echo "ASPNETCORE_ENVIRONMENT=Development" >> innhotel-api/.env
fi

# Start API in background
echo -e "${BLUE}🚀 Starting API server...${NC}"
cd innhotel-api/src/InnHotel.Web
dotnet run --urls="http://localhost:57679" &
API_PID=$!
cd ../../..

# Wait a moment for API to start
sleep 5

# Start Client
echo -e "${BLUE}🚀 Starting Client...${NC}"
cd innhotel-desktop-client
pnpm run dev:react &
CLIENT_PID=$!
cd ..

echo -e "${GREEN}✅ Project started successfully!${NC}"
echo -e "${BLUE}📱 Client: http://localhost:5173${NC}"
echo -e "${BLUE}🔧 API: http://localhost:57679${NC}"
echo ""
echo -e "${YELLOW}📋 Default Login Credentials:${NC}"
echo -e "   Super Admin: super@innhotel.com / Sup3rP@ssword!"
echo -e "   Admin: admin@innhotel.com / Adm1nP@ssword!"
echo ""
echo -e "${YELLOW}⚠️  Press Ctrl+C to stop all services${NC}"

# Function to cleanup on exit
cleanup() {
    echo -e "\n${YELLOW}🛑 Stopping services...${NC}"
    kill $API_PID 2>/dev/null || true
    kill $CLIENT_PID 2>/dev/null || true
    echo -e "${GREEN}✅ All services stopped${NC}"
    exit 0
}

# Set trap to cleanup on script exit
trap cleanup SIGINT SIGTERM

# Wait for processes
wait

