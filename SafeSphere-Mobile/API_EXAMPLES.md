# SafeSphere Mobile - API Integration Examples

Complete reference for API integration with example payloads and responses.

## 🌐 API Configuration

### Base URL Setup

```typescript
// src/config/api.config.ts
export const API_CONFIG = {
  BASE_URL: 'http://192.168.1.100:5297', // Your computer's IP
  
  // Alternative URLs:
  // Android Emulator: 'http://10.0.2.2:5297'
  // iOS Simulator: 'http://localhost:5297'
  // Production: 'https://api.safesphere.com'
};
```

---

## 📡 API Endpoints Reference

### 1. Panic Alert Endpoints

#### Create Panic Alert

**Endpoint:** `POST /api/alert/panic?userId={userId}`

**Request Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "additionalInfo": "Emergency situation - need immediate help"
}
```

**Success Response (201 Created):**
```json
{
  "id": 15,
  "userId": 1,
  "userName": "John Doe",
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "timestamp": "2025-10-27T14:30:25.123Z",
  "status": "Active",
  "additionalInfo": "Emergency situation - need immediate help"
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "User not found or failed to create alert"
}
```

**Error Response (401 Unauthorized):**
```json
{
  "message": "Invalid email or password"
}
```

**Code Example:**
```typescript
import PanicAlertService from './services/panicAlert.service';

const sendPanicAlert = async () => {
  try {
    const response = await PanicAlertService.createPanicAlert(1, {
      locationLat: 40.7128,
      locationLng: -74.0060,
      additionalInfo: "Emergency help needed"
    });
    
    console.log('Alert created:', response.id);
    console.log('Status:', response.status);
  } catch (error) {
    console.error('Failed to create alert:', error);
  }
};
```

---

### 2. SOS Alert Endpoints

#### Create SOS Alert

**Endpoint:** `POST /api/alert/sos?userId={userId}`

**Request Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "message": "Car accident on Highway 101. Need immediate assistance!",
  "location": "Highway 101 North, San Francisco, CA",
  "locationLat": 37.7749,
  "locationLng": -122.4194
}
```

**Success Response (201 Created):**
```json
{
  "id": 28,
  "userId": 1,
  "userName": "John Doe",
  "message": "Car accident on Highway 101. Need immediate assistance!",
  "location": "Highway 101 North, San Francisco, CA",
  "locationLat": 37.7749,
  "locationLng": -122.4194,
  "timestamp": "2025-10-27T14:35:10.456Z",
  "acknowledged": false,
  "acknowledgedAt": null
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "User not found or failed to create alert",
  "errors": {
    "message": ["The message field is required."]
  }
}
```

**Code Example:**
```typescript
import SOSService from './services/sos.service';

const sendSOSAlert = async () => {
  try {
    const response = await SOSService.createSOSAlert(1, {
      message: "Need medical assistance",
      location: "Downtown Los Angeles",
      locationLat: 34.0522,
      locationLng: -118.2437
    });
    
    console.log('SOS Alert ID:', response.id);
    console.log('Message:', response.message);
    console.log('Acknowledged:', response.acknowledged);
  } catch (error) {
    console.error('Failed to send SOS:', error);
  }
};
```

---

### 3. Get User Alerts

#### Get User's Panic Alerts

**Endpoint:** `GET /api/alert/panic/user/{userId}`

**Success Response (200 OK):**
```json
[
  {
    "id": 15,
    "userId": 1,
    "userName": "John Doe",
    "locationLat": 40.7128,
    "locationLng": -74.0060,
    "timestamp": "2025-10-27T14:30:25.123Z",
    "status": "Active",
    "additionalInfo": "Emergency situation"
  },
  {
    "id": 12,
    "userId": 1,
    "userName": "John Doe",
    "locationLat": 34.0522,
    "locationLng": -118.2437,
    "timestamp": "2025-10-27T10:15:00.000Z",
    "status": "Resolved",
    "additionalInfo": "False alarm"
  }
]
```

**Code Example:**
```typescript
const fetchUserAlerts = async (userId: number) => {
  try {
    const alerts = await PanicAlertService.getUserPanicAlerts(userId);
    console.log(`Found ${alerts.length} panic alerts`);
    alerts.forEach(alert => {
      console.log(`Alert ${alert.id}: ${alert.status}`);
    });
  } catch (error) {
    console.error('Failed to fetch alerts:', error);
  }
};
```

#### Get User's SOS Alerts

**Endpoint:** `GET /api/alert/sos/user/{userId}`

**Success Response (200 OK):**
```json
[
  {
    "id": 28,
    "userId": 1,
    "userName": "John Doe",
    "message": "Car accident on Highway 101",
    "location": "Highway 101, San Francisco",
    "locationLat": 37.7749,
    "locationLng": -122.4194,
    "timestamp": "2025-10-27T14:35:10.456Z",
    "acknowledged": false,
    "acknowledgedAt": null
  }
]
```

---

## 🧪 Testing Examples

### Test with cURL

#### Panic Alert
```bash
curl -X POST "http://192.168.1.100:5297/api/alert/panic?userId=1" \
  -H "Content-Type: application/json" \
  -d '{
    "locationLat": 40.7128,
    "locationLng": -74.0060,
    "additionalInfo": "Test panic alert"
  }'
```

#### SOS Alert
```bash
curl -X POST "http://192.168.1.100:5297/api/alert/sos?userId=1" \
  -H "Content-Type: application/json" \
  -d '{
    "message": "Test SOS message",
    "location": "Test Location",
    "locationLat": 40.7128,
    "locationLng": -74.0060
  }'
```

### Test with Postman

1. **Create New Request**
2. **Set Method:** POST
3. **URL:** `http://192.168.1.100:5297/api/alert/panic?userId=1`
4. **Headers:**
   - `Content-Type: application/json`
5. **Body (raw JSON):**
```json
{
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "additionalInfo": "Postman test"
}
```

---

## 🔄 Redux Integration

### Panic Alert with Redux

```typescript
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { createPanicAlert } from '../store/slices/panicAlertSlice';

const MyComponent = () => {
  const dispatch = useAppDispatch();
  const { loading, error, success, currentAlert } = useAppSelector(
    (state) => state.panicAlert
  );

  const handleSendAlert = async () => {
    try {
      const result = await dispatch(
        createPanicAlert({
          userId: 1,
          alertData: {
            locationLat: 40.7128,
            locationLng: -74.0060,
            additionalInfo: "Emergency"
          }
        })
      ).unwrap();
      
      console.log('Alert created:', result);
    } catch (err) {
      console.error('Error:', err);
    }
  };

  return (
    // Your component JSX
  );
};
```

### SOS Alert with Redux

```typescript
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { createSOSAlert } from '../store/slices/sosAlertSlice';

const SOSComponent = () => {
  const dispatch = useAppDispatch();
  const { loading, error, success } = useAppSelector(
    (state) => state.sosAlert
  );

  const handleSendSOS = async () => {
    try {
      const result = await dispatch(
        createSOSAlert({
          userId: 1,
          alertData: {
            message: "Need help!",
            location: "Downtown",
            locationLat: 40.7128,
            locationLng: -74.0060
          }
        })
      ).unwrap();
      
      console.log('SOS sent:', result);
    } catch (err) {
      console.error('Error:', err);
    }
  };

  return (
    // Your component JSX
  );
};
```

---

## 🎯 Complete Flow Examples

### Panic Alert Flow

```typescript
// 1. Get user's current location
const location = await Location.getCurrentPositionAsync({
  accuracy: Location.Accuracy.High
});

// 2. Prepare alert data
const alertData = {
  locationLat: location.coords.latitude,
  locationLng: location.coords.longitude,
  additionalInfo: "Emergency situation"
};

// 3. Send alert via Redux
const result = await dispatch(
  createPanicAlert({ userId: 1, alertData })
).unwrap();

// 4. Trigger feedback
await Haptics.notificationAsync(
  Haptics.NotificationFeedbackType.Success
);

// 5. Show confirmation
Alert.alert('Success', `Alert ID: ${result.id} sent!`);
```

### SOS Alert Flow

```typescript
// 1. Get location
const location = await Location.getCurrentPositionAsync();

// 2. Get address
const address = await Location.reverseGeocodeAsync({
  latitude: location.coords.latitude,
  longitude: location.coords.longitude
});

// 3. Format location string
const locationStr = address[0] 
  ? `${address[0].street}, ${address[0].city}`
  : 'Unknown location';

// 4. Send SOS
const result = await dispatch(
  createSOSAlert({
    userId: 1,
    alertData: {
      message: userMessage,
      location: locationStr,
      locationLat: location.coords.latitude,
      locationLng: location.coords.longitude
    }
  })
).unwrap();

// 5. Confirm
Alert.alert('SOS Sent', result.message);
```

---

## 🔐 Error Handling

### Network Errors

```typescript
try {
  const response = await PanicAlertService.createPanicAlert(userId, data);
} catch (error: any) {
  if (error.status === 0) {
    // Network error - no response from server
    Alert.alert(
      'Network Error',
      'Unable to connect to server. Please check your internet connection.'
    );
  } else if (error.status === 400) {
    // Bad request
    Alert.alert('Invalid Data', error.message);
  } else if (error.status === 404) {
    // Not found
    Alert.alert('Not Found', 'User or resource not found');
  } else if (error.status >= 500) {
    // Server error
    Alert.alert('Server Error', 'Please try again later');
  }
}
```

### Timeout Handling

```typescript
// Configure timeout in api.config.ts
export const API_CONFIG = {
  TIMEOUT: 30000, // 30 seconds
  // ...
};

// Handle timeout in service
try {
  const response = await axios.post(url, data, {
    timeout: 30000
  });
} catch (error) {
  if (error.code === 'ECONNABORTED') {
    Alert.alert('Timeout', 'Request took too long. Please try again.');
  }
}
```

---

## 📊 Response Status Codes

| Code | Status | Meaning |
|------|--------|---------|
| 200 | OK | Request successful |
| 201 | Created | Alert created successfully |
| 400 | Bad Request | Invalid request data |
| 401 | Unauthorized | Authentication required |
| 404 | Not Found | Resource not found |
| 500 | Internal Server Error | Server error |

---

## 🧩 TypeScript Types

```typescript
// Panic Alert Types
interface CreatePanicAlertRequest {
  locationLat: number;
  locationLng: number;
  additionalInfo?: string;
}

interface PanicAlertResponse {
  id: number;
  userId: number;
  userName: string;
  locationLat: number;
  locationLng: number;
  timestamp: string;
  status: 'Active' | 'Resolved' | 'Cancelled';
  additionalInfo?: string;
}

// SOS Alert Types
interface CreateSOSAlertRequest {
  message: string;
  location: string;
  locationLat?: number;
  locationLng?: number;
}

interface SOSAlertResponse {
  id: number;
  userId: number;
  userName: string;
  message: string;
  location: string;
  locationLat?: number;
  locationLng?: number;
  timestamp: string;
  acknowledged: boolean;
  acknowledgedAt?: string;
}
```

---

## 📝 Best Practices

1. **Always check network connectivity before API calls**
2. **Implement retry logic for failed requests**
3. **Cache location data to avoid repeated GPS calls**
4. **Show loading indicators during API calls**
5. **Provide meaningful error messages to users**
6. **Log errors for debugging**
7. **Test on real devices with actual network conditions**

---

**Last Updated:** October 2025

