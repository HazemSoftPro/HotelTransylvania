#!/usr/bin/env node

const { spawn } = require('child_process')
const { createServer } = require('http')

const PORT = 3000
const HOST = 'localhost'

// Function to check if server is running
function checkServer() {
  return new Promise((resolve) => {
    const req = createServer().listen(PORT, () => {
      req.close()
      resolve(false) // Port is available
    }).on('error', () => {
      resolve(true) // Port is in use
    })
  })
}

// Function to wait for server to be ready
async function waitForServer(maxAttempts = 30) {
  for (let i = 0; i < maxAttempts; i++) {
    try {
      const response = await fetch(`http://${HOST}:${PORT}`)
      if (response.ok) {
        console.log('✅ Nuxt server is ready!')
        return true
      }
    } catch (error) {
      // Server not ready yet
    }
    
    console.log(`⏳ Waiting for Nuxt server... (${i + 1}/${maxAttempts})`)
    await new Promise(resolve => setTimeout(resolve, 1000))
  }
  
  console.error('❌ Nuxt server failed to start within timeout')
  return false
}

async function main() {
  console.log('🚀 Starting HMSuite in Electron development mode...\n')
  
  // Check if server is already running
  const serverRunning = await checkServer()
  
  let nuxtProcess
  
  if (!serverRunning) {
    console.log('📦 Starting Nuxt development server...')
    nuxtProcess = spawn('pnpm', ['dev'], {
      stdio: 'inherit',
      shell: true
    })
    
    // Wait for server to be ready
    const ready = await waitForServer()
    if (!ready) {
      if (nuxtProcess) nuxtProcess.kill()
      process.exit(1)
    }
  } else {
    console.log('✅ Nuxt server is already running!')
  }
  
  console.log('🖥️  Launching Electron app...\n')
  
  // Start Electron
  const electronProcess = spawn('electron', ['.'], {
    stdio: 'inherit',
    shell: true,
    env: { ...process.env, NODE_ENV: 'development' }
  })
  
  // Handle process cleanup
  process.on('SIGINT', () => {
    console.log('\n🛑 Shutting down...')
    if (electronProcess) electronProcess.kill()
    if (nuxtProcess) nuxtProcess.kill()
    process.exit(0)
  })
  
  electronProcess.on('close', (code) => {
    console.log(`\n📱 Electron process exited with code ${code}`)
    if (nuxtProcess) nuxtProcess.kill()
    process.exit(code)
  })
}

main().catch(console.error)
