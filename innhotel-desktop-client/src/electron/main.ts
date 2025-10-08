import { app, BrowserWindow } from 'electron';
import path from 'path';
import { isDevEnv } from './utils/isDevEnv.js';

// تجاوز شهادات HTTPS dev
app.on('certificate-error', (event, webContents, url, error, certificate, callback) => {
  if (url.startsWith('https://localhost')) {
    event.preventDefault();
    callback(true);
  } else {
    callback(false);
  }
});

app.whenReady().then(() => {
  const mainWindow = new BrowserWindow({
    width: 1280,
    height: 800,
    minWidth: 1024,
    minHeight: 768,
    center: true,
    webPreferences: { nodeIntegration: true }
  });

  if (isDevEnv()) {
    mainWindow.loadURL('http://localhost:5173'); // Vite dev server
    // mainWindow.webContents.openDevTools(); // اختياري للتطوير
  } else {
    mainWindow.loadFile(path.join(app.getAppPath(), 'dist-react', 'index.html'));
  }
});
