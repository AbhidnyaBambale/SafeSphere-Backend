# SafeSphere - Emergency Safety Application

SafeSphere is a comprehensive React Native mobile application designed to provide emergency safety features and real-time assistance to users. Built with Expo, TypeScript, and modern React Native technologies.

## 🚀 Features

### Core Safety Modules
- **Panic Alert**: Send immediate emergency alerts with location sharing
- **Threat Detection**: AI-powered threat detection and analysis
- **Disaster Alert**: Real-time natural disaster warnings and updates
- **Weather Alert**: Weather condition monitoring and alerts
- **Emergency SOS**: Quick access to emergency services (911)
- **Safe Route Mapping**: Find the safest path to your destination
- **Danger Zone Alerts**: Avoid dangerous areas with real-time warnings
- **Live Location Sharing**: Share your location with trusted contacts
- **Health Emergency Support**: Medical emergency assistance and first aid guidance

### Technical Features
- **Real-time Location Tracking**: GPS-based location services
- **Redux State Management**: Centralized state management with Redux Toolkit
- **API Integration**: Axios-based API service for backend communication
- **Navigation**: React Navigation with bottom tabs and stack navigation
- **TypeScript**: Full TypeScript support for type safety
- **Modern UI**: Clean and intuitive user interface with React Native Paper

## 🛠️ Tech Stack

- **React Native** (latest version)
- **Expo** (for fast development & deployment)
- **TypeScript**
- **React Navigation** (for screen navigation)
- **Redux Toolkit** (for state management)
- **Axios** (for API calls)
- **React Native Maps** (for Safe Route Mapping & Danger Zone Alerts)
- **Expo Location** (for GPS tracking)
- **React Native Paper** (for UI components)

## 📱 Installation & Setup

### Prerequisites
- Node.js (v20.19.4 or higher recommended)
- npm or yarn
- Expo CLI
- iOS Simulator (for iOS development) or Android Studio (for Android development)

### Installation Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd SafeSphere
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Start the development server**
   ```bash
   npm start
   # or
   npx expo start
   ```

4. **Run on specific platforms**
   ```bash
   # Android
   npm run android
   
   # iOS
   npm run ios
   
   # Web
   npm run web
   ```

## 🏗️ Project Structure

```
SafeSphere/
├── src/
│   ├── components/          # Reusable UI components
│   ├── navigation/          # Navigation configuration
│   │   └── AppNavigator.tsx
│   ├── redux/              # Redux store and slices
│   │   ├── store.ts
│   │   └── slices/
│   │       ├── authSlice.ts
│   │       ├── alertSlice.ts
│   │       ├── locationSlice.ts
│   │       └── userSlice.ts
│   ├── screens/            # Application screens
│   │   ├── SplashScreen.tsx
│   │   ├── LoginScreen.tsx
│   │   ├── RegisterScreen.tsx
│   │   ├── HomeScreen.tsx
│   │   ├── PanicAlertScreen.tsx
│   │   ├── ThreatDetectionScreen.tsx
│   │   ├── DisasterAlertScreen.tsx
│   │   ├── WeatherAlertScreen.tsx
│   │   ├── EmergencySOSScreen.tsx
│   │   ├── SafeRouteMappingScreen.tsx
│   │   ├── DangerZoneAlertsScreen.tsx
│   │   ├── LiveLocationSharingScreen.tsx
│   │   └── HealthEmergencySupportScreen.tsx
│   ├── services/           # API services
│   │   ├── api.ts
│   │   └── locationService.ts
│   ├── types/              # TypeScript type definitions
│   │   └── index.ts
│   ├── utils/              # Utility functions
│   └── config/             # Configuration files
│       └── environment.ts
├── App.tsx                 # Main application component
├── package.json
└── README.md
```

## 🔧 Configuration

### Environment Variables
Create a `.env` file in the root directory with the following variables:

```env
API_BASE_URL=http://localhost:5000/api
GOOGLE_MAPS_API_KEY=your_google_maps_api_key_here
EXPO_PUBLIC_API_URL=http://localhost:5000/api
```

### API Configuration
The app is configured to connect to a .NET backend API. Update the `API_BASE_URL` in the environment configuration to point to your backend server.

## 📱 Usage

### Getting Started
1. Launch the app
2. Create an account or sign in
3. Grant location permissions when prompted
4. Explore the safety modules from the home dashboard

### Key Features Usage
- **Panic Alert**: Press and hold the panic button to send an emergency alert
- **Safe Routes**: Get real-time route suggestions with safety scores
- **Location Sharing**: Share your location with emergency contacts
- **Emergency SOS**: Quick access to emergency services

## 🚨 Emergency Features

### Panic Alert
- One-touch emergency alert system
- Automatic location sharing
- Emergency contact notification
- Integration with local authorities

### Health Emergency Support
- Medical emergency guidance
- First aid instructions
- Emergency contact access
- Symptom-based assistance

### Safety Features
- Real-time threat detection
- Danger zone avoidance
- Weather and disaster alerts
- Safe route recommendations

## 🔒 Privacy & Security

- Location data is encrypted and secure
- User data is only shared with selected contacts
- No permanent storage of sensitive information
- Privacy-protected location sharing

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the documentation

## 🔮 Future Enhancements

- Integration with wearable devices
- Machine learning-based threat prediction
- Community safety features
- Multi-language support
- Offline mode capabilities

# SafeSphere - Quick Start Guide

## 🚀 5-Minute Setup for Backend Integration

### Step 1: Get Your IP Address

**Windows:**
```cmd
ipconfig
```
Look for "IPv4 Address" (e.g., `192.168.1.100`)

**Mac/Linux:**
```bash
ifconfig | grep "inet " | grep -v 127.0.0.1
```

### Step 2: Create `.env` File

```bash
cd SafeSphere
echo "EXPO_PUBLIC_BACKEND_URL=http://YOUR_IP_HERE:5000/api" > .env
```

**Replace `YOUR_IP_HERE` with your actual IP address!**

Example:
```env
EXPO_PUBLIC_BACKEND_URL=http://192.168.1.100:5000/api
```

### Step 3: Install Dependencies

```bash
npm install
```

### Step 4: Start Backend

Make sure your .NET backend is running on port 5000.

### Step 5: Start the App

```bash
npx expo start
```

Then press:
- `a` for Android
- `i` for iOS (Mac only)
- `w` for Web
- Scan QR code for physical device

---

## ✅ Quick Test

1. **Register**: Create account with email/password
2. **Login**: Sign in with credentials
3. **Panic Alert**: Grant location permission → Press panic button
4. **SOS Alert**: Select emergency type → Send SOS

Check console for API logs:
```
[API Request] POST /user/register
[API Response] /user/register {token: "...", user: {...}}
```

---

## 🐛 Quick Fixes

**Can't connect?**
```bash
# 1. Check backend is running
curl http://YOUR_IP:5000/api

# 2. Update .env with correct IP

# 3. Restart Expo
npx expo start --clear
```

**Location not working?**
- Grant location permissions in device settings
- iOS: Settings > Privacy > Location Services
- Android: Settings > Apps > SafeSphere > Permissions

---

## 📚 Full Documentation

- [BACKEND_INTEGRATION.md](./BACKEND_INTEGRATION.md) - Complete integration guide
- [README.md](./README.md) - Full project documentation
- [setup.md](./setup.md) - Detailed setup instructions

---

**Happy Testing! 🎉**

