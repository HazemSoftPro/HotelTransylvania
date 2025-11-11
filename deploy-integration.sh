#!/bin/bash

# Hotel Transylvania Integration Deployment Script
# This script helps push the integration branch and create a pull request

set -e

echo "🏨 Hotel Transylvania Integration Deployment Script"
echo "=================================================="

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Check if we're in the right directory
if [ ! -f "INTEGRATION_ARCHITECTURE.md" ]; then
    echo -e "${RED}❌ Error: Please run this script from the HotelTransylvania root directory${NC}"
    exit 1
fi

echo -e "${BLUE}📋 Current Branch Status:${NC}"
git branch --show-current
echo ""

echo -e "${BLUE}🔍 Checking for uncommitted changes...${NC}"
if [ -n "$(git status --porcelain)" ]; then
    echo -e "${YELLOW}⚠️  Warning: There are uncommitted changes${NC}"
    git status --short
    echo ""
    read -p "Do you want to commit these changes? (y/n): " -n 1 -r
    echo
    if [[ $REPLY =~ ^[Yy]$ ]]; then
        echo -e "${BLUE}📝 Committing changes...${NC}"
        git add .
        git commit -m "feat: Additional integration improvements and fixes"
    else
        echo -e "${RED}❌ Please commit or stash changes before proceeding${NC}"
        exit 1
    fi
fi

echo -e "${BLUE}🚀 Pushing integration branch to remote...${NC}"
echo ""

# Check if authentication is configured
if ! git config --get credential.helper >/dev/null 2>&1; then
    echo -e "${YELLOW}⚠️  No credential helper configured${NC}"
    echo ""
    echo -e "${BLUE}To configure Git authentication, choose one of the following:${NC}"
    echo ""
    echo "1. Personal Access Token (Recommended):"
    echo "   - Go to GitHub > Settings > Developer settings > Personal access tokens"
    echo "   - Generate a new token with 'repo' scope"
    echo "   - Use: git config credential.helper store"
    echo "   - Then: git push (enter token as password)"
    echo ""
    echo "2. SSH Key Setup:"
    echo "   - Generate SSH key: ssh-keygen -t ed25519 -C 'your-email@example.com'"
    echo "   - Add to GitHub: Settings > SSH and GPG keys"
    echo "   - Update remote URL: git remote set-url origin git@github.com:HazemSoftPro/HotelTransylvania.git"
    echo ""
    echo "3. GitHub CLI:"
    echo "   - Install: gh auth login"
    echo "   - Then: gh repo clone HazemSoftPro/HotelTransylvania"
    echo ""
    
    read -p "Press Enter to attempt push with current configuration..."
fi

# Attempt to push
if git push -u origin feature/integrated-hotel-management-system; then
    echo -e "${GREEN}✅ Branch pushed successfully!${NC}"
else
    echo -e "${RED}❌ Push failed. Please configure authentication as shown above.${NC}"
    echo ""
    echo -e "${BLUE}After configuring authentication, run:${NC}"
    echo "git push -u origin feature/integrated-hotel-management-system"
    exit 1
fi

echo ""
echo -e "${GREEN}🎉 Integration branch deployed successfully!${NC}"
echo ""

# Instructions for creating pull request
echo -e "${BLUE}📋 Next Steps - Create Pull Request:${NC}"
echo ""
echo "Option 1 - Using GitHub CLI:"
echo "  gh pr create --title 'feat: Complete integration of hotel management system' \&quot;
echo "              --body-file .github/PULL_REQUEST_TEMPLATE.md \&quot;
echo "              --base main --head feature/integrated-hotel-management-system"
echo ""
echo "Option 2 - Using GitHub Web Interface:"
echo "  1. Visit: https://github.com/HazemSoftPro/HotelTransylvania"
echo "  2. Click 'Compare & pull request'"
echo "  3. Select: feature/integrated-hotel-management-system -> main"
echo "  4. Use the template in .github/PULL_REQUEST_TEMPLATE.md"
echo "  5. Click 'Create pull request'"
echo ""
echo "Option 3 - Using this script:"
read -p "Do you want to create a pull request now? (y/n): " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    if command -v gh >/dev/null 2>&1; then
        echo -e "${BLUE}📝 Creating pull request with GitHub CLI...${NC}"
        gh pr create \
            --title "feat: Complete integration of hotel management system" \
            --body "$(cat .github/PULL_REQUEST_TEMPLATE.md)" \
            --base main \
            --head feature/integrated-hotel-management-system
        echo -e "${GREEN}✅ Pull request created successfully!${NC}"
    else
        echo -e "${YELLOW}⚠️  GitHub CLI not found. Please create PR manually using the web interface.${NC}"
        echo "Visit: https://github.com/HazemSoftPro/HotelTransylvania/compare/main...feature/integrated-hotel-management-system"
    fi
fi

echo ""
echo -e "${BLUE}📊 Integration Summary:${NC}"
echo "- 35 files changed, 5,766 insertions(+)"
echo "- Event-driven architecture with 15+ event types"
echo "- Real-time SignalR integration"
echo "- 50+ integration tests"
echo "- Complete documentation suite"
echo ""
echo -e "${GREEN}🚀 Hotel Transylvania is ready for integrated hospitality excellence!${NC}"