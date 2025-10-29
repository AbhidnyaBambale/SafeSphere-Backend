# SafeSphere Mobile - Quick Start Guide

Get the SafeSphere mobile app running in 10 minutes!

## 📋 Prerequisites Checklist

- [ ] Node.js installed (v18+)
- [ ] npm or yarn installed
- [ ] Expo CLI installed globally
- [ ] Backend API running
- [ ] Physical device or emulator ready

---

## 🚀 Setup Steps

### Step 1: Install Expo CLI

```bash
npm install -g expo-cli
```

### Step 2: Navigate to Project

```bash
cd SafeSphere-Mobile
```

### Step 3: Install Dependencies

```bash
npm install
```

**Wait for installation to complete** (~2-3 minutes)

### Step 4: Find Your Computer's IP Address

**Windows:**
```cmd
ipconfig
```
Look for **IPv4 Address** (e.g., `192.168.1.100`)

**Mac/Linux:**
```bash
ifconfig | grep "inet "
```
Look for your local IP (e.g., `192.168.1.100`)

### Step 5: Configure API URL

Edit `src/config/api.config.ts`:

```typescript
export const API_CONFIG = {
  BASE_URL: 'http://192.168.1.100:5297', // ← CHANGE THIS to YOUR IP
  // ...
};
```

**Replace `192.168.1.100` with your actual IP address**

### Step 6: Start Backend API

In a separate terminal:

```bash
cd ../SafeSphere.API
dotnet run
```

**Verify it's running:**
```bash
curl http://localhost:5297/health
```

Should return: `{"status":"healthy","timestamp":"..."}`

### Step 7: Start Mobile App

```bash
npm start
```

or

```bash
expo start
```

### Step 8: Run on Device

#### Option A: Physical Device (Recommended)

1. **Install Expo Go** on your phone:
   - iOS: App Store
   - Android: Google Play Store

2. **Scan QR Code:**
   - iOS: Use Camera app
   - Android: Use Expo Go app

3. App loads on your device!

#### Option B: Emulator/Simulator

**iOS (Mac only):**
```bash
npm run ios
```

**Android:**
```bash
npm run android
```

---

## ✅ Verify Installation

### 1. Check Backend Connection

The app should display location information on the Panic Alert screen.

### 2. Test Panic Alert

1. Tap **Panic Alert** tab
2. Wait for location to load
3. Tap **"SEND PANIC ALERT"**
4. Confirm
5. Should see: ✅ "Alert sent successfully!"

### 3. Test SOS Alert

1. Tap **SOS** tab
2. Enter a message: "Test SOS"
3. Tap **"SEND SOS ALERT"**
4. Confirm
5. Should see: ✅ "SOS sent successfully!"

---

## 🐛 Troubleshooting

### "Network request failed"

**Solution:**
1. Verify backend is running: `curl http://192.168.1.100:5297/health`
2. Check IP address in `api.config.ts` matches your computer's IP
3. Ensure phone and computer are on **same WiFi network**
4. Disable VPN if active

### "Location not loading"

**Solution:**
1. Grant location permissions when prompted
2. Enable Location Services in device settings
3. For iOS Simulator: Features → Location → Custom Location

### "Cannot connect to Metro"

**Solution:**
```bash
# Clear cache
expo start -c

# Or restart Metro bundler
npx react-native start --reset-cache
```

### Build errors

**Solution:**
```bash
# Clean install
rm -rf node_modules
npm install

# Clear Expo cache
rm -rf .expo
```

---

## 📱 Device-Specific Notes

### iOS

- Requires Xcode (Mac only) for simulator
- Real device requires Apple Developer account for custom builds
- Expo Go works without developer account

### Android

- Enable Developer Mode:
  1. Settings → About Phone
  2. Tap "Build Number" 7 times
  3. Enable USB Debugging
- Allow "Install from Unknown Sources" for Expo Go

---

## 🎯 What's Next?

### Test All Features

- [x] Panic Alert sending
- [x] SOS Alert with custom message
- [x] Location tracking
- [x] Error handling
- [x] Success confirmations

### Backend Integration Checklist

- [x] Backend API running
- [x] Database connected
- [x] CORS enabled
- [x] Endpoints responding
- [x] Sample data loaded

---

## 📊 Expected Behavior

### Panic Alert Flow

1. **Location loads** automatically
2. **Add info** (optional)
3. **Tap button** → Confirmation dialog
4. **Confirm** → Loading spinner
5. **Success** → Haptic feedback + Sound + Success message
6. **Backend** receives alert with ID

### SOS Flow

1. **Location loads** automatically
2. **Select template** or type message
3. **Tap button** → Confirmation dialog
4. **Confirm** → Loading spinner
5. **Success** → Haptic feedback + Success message
6. **Backend** receives SOS with ID

---

## 🔍 Debugging

### View Console Logs

**Expo:**
- Press `j` in terminal to open debugger
- Or shake device → "Debug Remote JS"

**React Native Debugger:**
```bash
# Install
brew install --cask react-native-debugger

# Open
open "rndebugger://set-debugger-loc?host=localhost&port=19000"
```

### Check API Calls

Look for logs like:
```
[API] POST /api/alert/panic?userId=1
[PanicAlert] Creating alert for user: 1
[API] Response from /api/alert/panic?userId=1: 201
[PanicAlert] Alert created successfully: 15
```

### Common Issues

| Error | Solution |
|-------|----------|
| "Timeout" | Increase timeout in `api.config.ts` |
| "401 Unauthorized" | Check userId exists in backend |
| "CORS error" | Verify backend CORS is enabled |
| "Location denied" | Grant permissions in settings |

---

## 🎉 Success!

You should now have:
- ✅ Mobile app running on device
- ✅ Connected to backend API
- ✅ Able to send panic alerts
- ✅ Able to send SOS alerts
- ✅ Location tracking working
- ✅ Haptic feedback and sounds

---

## 📚 Next Steps

1. **Customize UI** - Edit `src/screens/` files
2. **Add Features** - Implement user authentication
3. **Test Thoroughly** - Try different scenarios
4. **Build for Production** - Use `expo build`

---

## 📞 Need Help?

1. Check `README.md` for detailed documentation
2. Review `API_EXAMPLES.md` for API reference
3. Check backend logs: `SafeSphere.API/logs/`
4. Review mobile console logs

---

**Happy Testing! 🚀**

