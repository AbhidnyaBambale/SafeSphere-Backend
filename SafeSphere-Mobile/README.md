# SafeSphere Mobile App

A React Native mobile application for emergency alerts and safety features, fully integrated with the SafeSphere .NET 8 backend API.

## 🚀 Features

### ✅ Implemented Features

- **Panic Alert System**
  - One-tap emergency alert sending
  - Automatic GPS location capture
  - Real-time location tracking
  - Additional info input
  - Success confirmation with haptic feedback
  - Sound alerts
  
- **SOS Alert System**
  - Custom emergency message input
  - Quick message templates
  - Location-based alerts
  - Message character counter
  - Delivery confirmation

- **Backend Integration**
  - Full REST API integration with .NET 8 backend
  - Axios HTTP client
  - Redux Toolkit state management
  - Error handling and retry logic
  
- **User Experience**
  - Haptic feedback on all actions
  - Alert sound effects
  - Loading states
  - Error handling with user-friendly messages
  - Responsive design

## 📋 Prerequisites

- Node.js (v18 or higher)
- npm or yarn
- Expo CLI: `npm install -g expo-cli`
- iOS Simulator (Mac) or Android Emulator
- Physical device for testing (recommended)

## 🛠️ Installation

### 1. Navigate to Mobile App Directory

```bash
cd SafeSphere-Mobile
```

### 2. Install Dependencies

```bash
npm install
# or
yarn install
```

### 3. Configure Backend API URL

Edit `src/config/api.config.ts` and update the `BASE_URL`:

```typescript
export const API_CONFIG = {
  // Change to your computer's local IP address
  BASE_URL: 'http://192.168.1.100:5297',  // Replace with YOUR IP
  
  // For Android Emulator:
  // BASE_URL: 'http://10.0.2.2:5297',
  
  // For iOS Simulator:
  // BASE_URL: 'http://localhost:5297',
  
  TIMEOUT: 30000,
  // ...
};
```

**How to find your IP address:**

**Windows:**
```cmd
ipconfig
# Look for IPv4 Address under your active network adapter
```

**Mac/Linux:**
```bash
ifconfig
# Look for inet address under en0 or your active adapter
```

### 4. Add Sound Files (Optional)

Create placeholder sound files or use your own:

```bash
mkdir -p assets
touch assets/alert-sound.mp3
touch assets/success-sound.mp3
```

Or download free sound effects from:
- https://freesound.org/
- https://mixkit.co/free-sound-effects/

## 🚀 Running the App

### Start Expo Development Server

```bash
npm start
# or
expo start
```

### Run on Physical Device (Recommended)

1. Install **Expo Go** app on your phone:
   - iOS: App Store
   - Android: Google Play Store

2. Scan the QR code from the terminal with:
   - iOS: Camera app
   - Android: Expo Go app

3. The app will load on your device

### Run on Emulator/Simulator

**iOS Simulator (Mac only):**
```bash
npm run ios
```

**Android Emulator:**
```bash
npm run android
```

## 📡 Backend API Integration

### Ensure Backend is Running

Before testing the mobile app, make sure your .NET 8 backend is running:

```bash
cd ../SafeSphere.API
dotnet run
```

The API should be accessible at `http://localhost:5297` (or your configured port).

### API Endpoints Used

| Feature | Method | Endpoint | Description |
|---------|--------|----------|-------------|
| Panic Alert | POST | `/api/alert/panic?userId={id}` | Create panic alert |
| SOS Alert | POST | `/api/alert/sos?userId={id}` | Create SOS alert |
| Get User Alerts | GET | `/api/alert/panic/user/{userId}` | Fetch user's panic alerts |
| Get SOS Alerts | GET | `/api/alert/sos/user/{userId}` | Fetch user's SOS alerts |
| Health Check | GET | `/health` | API health status |

## 📦 Example Payloads

### Panic Alert Request

```json
{
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "additionalInfo": "Emergency panic alert triggered"
}
```

**Full Request:**
```http
POST http://192.168.1.100:5297/api/alert/panic?userId=1
Content-Type: application/json

{
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "additionalInfo": "Need immediate help"
}
```

**Response (201 Created):**
```json
{
  "id": 3,
  "userId": 1,
  "userName": "John Doe",
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "timestamp": "2025-10-27T12:30:00Z",
  "status": "Active",
  "additionalInfo": "Need immediate help"
}
```

### SOS Alert Request

```json
{
  "message": "Car breakdown, need assistance",
  "location": "Highway 101, San Francisco",
  "locationLat": 37.7749,
  "locationLng": -122.4194
}
```

**Full Request:**
```http
POST http://192.168.1.100:5297/api/alert/sos?userId=1
Content-Type: application/json

{
  "message": "Car breakdown, need assistance",
  "location": "Highway 101, San Francisco",
  "locationLat": 37.7749,
  "locationLng": -122.4194
}
```

**Response (201 Created):**
```json
{
  "id": 5,
  "userId": 1,
  "userName": "John Doe",
  "message": "Car breakdown, need assistance",
  "location": "Highway 101, San Francisco",
  "locationLat": 37.7749,
  "locationLng": -122.4194,
  "timestamp": "2025-10-27T12:35:00Z",
  "acknowledged": false,
  "acknowledgedAt": null
}
```

## 🧪 Testing the App

### 1. Test Panic Alert Feature

1. Open the app
2. Navigate to "Panic Alert" tab
3. Wait for location to load
4. (Optional) Add additional information
5. Tap "SEND PANIC ALERT"
6. Confirm the alert
7. Verify:
   - Haptic feedback triggers
   - Success message appears
   - Alert sound plays
   - Backend receives the request

### 2. Test SOS Feature

1. Navigate to "SOS" tab
2. Wait for location to load
3. Either:
   - Select a quick message template, OR
   - Type a custom message
4. Tap "SEND SOS ALERT"
5. Confirm the alert
6. Verify:
   - Success confirmation shows
   - Haptic feedback triggers
   - Backend receives the request

### 3. Test Error Handling

**Network Error:**
1. Stop the backend API
2. Try to send an alert
3. Verify error message displays

**No Location:**
1. Deny location permissions
2. Try to send an alert
3. Verify appropriate message shows

## 🔧 Project Structure

```
SafeSphere-Mobile/
├── src/
│   ├── config/
│   │   └── api.config.ts          # API configuration
│   ├── services/
│   │   ├── api.service.ts         # Base Axios service
│   │   ├── panicAlert.service.ts  # Panic alert API calls
│   │   └── sos.service.ts         # SOS alert API calls
│   ├── store/
│   │   ├── index.ts               # Redux store
│   │   ├── hooks.ts               # Typed Redux hooks
│   │   └── slices/
│   │       ├── panicAlertSlice.ts # Panic alert state
│   │       └── sosAlertSlice.ts   # SOS alert state
│   └── screens/
│       ├── PanicAlertScreen.tsx   # Panic alert UI
│       └── SOSScreen.tsx          # SOS alert UI
├── assets/                         # Images and sounds
├── App.tsx                        # Main app component
├── package.json                   # Dependencies
├── tsconfig.json                  # TypeScript config
└── app.json                       # Expo configuration
```

## 🎨 Technologies Used

- **React Native** - Mobile framework
- **Expo** - Development platform
- **TypeScript** - Type safety
- **Redux Toolkit** - State management
- **Axios** - HTTP client
- **React Navigation** - Navigation
- **Expo Location** - GPS location
- **Expo Haptics** - Haptic feedback
- **Expo AV** - Audio playback

## 🐛 Troubleshooting

### "Network request failed" Error

1. **Check backend is running:**
   ```bash
   curl http://192.168.1.100:5297/health
   ```

2. **Verify IP address in `api.config.ts` is correct**

3. **Ensure phone and computer are on same network**

4. **Check firewall settings** (Windows may block incoming connections)

5. **For Android Emulator,** use `10.0.2.2` instead of `localhost`

### Location Not Loading

1. **Grant location permissions** in phone settings
2. **Enable Location Services** on device
3. **For iOS Simulator:** Go to Features → Location → Custom Location

### Build Errors

```bash
# Clear cache and reinstall
rm -rf node_modules
npm install

# Clear Expo cache
expo start -c
```

## 📱 Device Testing Tips

### iOS Testing
- Use real device for location testing
- Simulator location can be set manually
- Accept location permissions when prompted

### Android Testing
- Enable Developer Options
- Install Expo Go from Play Store
- May need to enable "Install from Unknown Sources"

## 🚀 Production Deployment

### Build for iOS

```bash
expo build:ios
```

### Build for Android

```bash
expo build:android
```

### Or use EAS Build (recommended)

```bash
npm install -g eas-cli
eas build --platform ios
eas build --platform android
```

## 📝 Development Notes

### Current Limitations

1. **Authentication:** Currently uses hardcoded `userId = 1`
   - TODO: Implement full authentication flow
   - TODO: Store user session in AsyncStorage

2. **Sound Files:** Placeholder sounds need to be added
   - Add `assets/alert-sound.mp3`
   - Add `assets/success-sound.mp3`

3. **Offline Support:** No offline queue for alerts
   - TODO: Implement offline alert queue
   - TODO: Retry failed requests when online

### Future Enhancements

- [ ] User authentication (login/register)
- [ ] Alert history screen
- [ ] Push notifications
- [ ] Real-time alert updates
- [ ] Emergency contact management
- [ ] Map view for alerts
- [ ] Dark mode support
- [ ] Multi-language support

## 📄 License

This project is part of the SafeSphere safety platform.

## 👥 Support

For issues or questions:
1. Check backend API is running and accessible
2. Verify network connectivity
3. Check console logs for detailed errors
4. Review backend logs in `SafeSphere.API/logs/`

---

**Built with ❤️ using React Native, Expo, and .NET 8**

