# SafeSphere API Testing Guide

Complete guide for testing all Milestones 4 & 5 endpoints.

---

## 🔧 Setup

### 1. Start the Backend
```bash
cd SafeSphere.API
dotnet run
```

### 2. Access Swagger UI
Open browser: `https://localhost:7000` or `http://localhost:5297`

### 3. Test User
Use seeded test user:
- **UserID**: 1
- **Email**: john.doe@example.com
- **Password**: password123

---

## 🗺️ Milestone 4: Route & Zone Endpoints

### Calculate Safe Route
**Endpoint**: `POST /api/route/safe?userId=1`

**Request Body**:
```json
{
  "originLat": 40.7128,
  "originLng": -74.0060,
  "destinationLat": 40.7589,
  "destinationLng": -73.9851,
  "transportMode": "driving",
  "avoidHighways": false
}
```

**Expected Response (201)**:
```json
{
  "id": 1,
  "userId": 1,
  "originLat": 40.7128,
  "originLng": -74.0060,
  "destinationLat": 40.7589,
  "destinationLng": -73.9851,
  "routeCoordinates": [
    { "lat": 40.7128, "lng": -74.0060 },
    { "lat": 40.7589, "lng": -73.9851 }
  ],
  "distanceMeters": 6542.5,
  "durationSeconds": 436,
  "safetyScore": 85.5,
  "unsafeZonesAvoided": 2,
  "isActive": true,
  "createdAt": "2025-10-29T10:30:00Z",
  "nearbyUnsafeZones": []
}
```

**cURL Example**:
```bash
curl -X POST "http://localhost:5297/api/route/safe?userId=1" \
  -H "Content-Type: application/json" \
  -d '{
    "originLat": 40.7128,
    "originLng": -74.0060,
    "destinationLat": 40.7589,
    "destinationLng": -73.9851,
    "transportMode": "driving"
  }'
```

---

### Report Unsafe Zone
**Endpoint**: `POST /api/route/zones/unsafe?userId=1`

**Request Body**:
```json
{
  "name": "Dark alley on 5th Street",
  "description": "Poorly lit area with recent crime reports",
  "centerLat": 40.7489,
  "centerLng": -73.9680,
  "radiusMeters": 300,
  "severity": "High",
  "threatType": "Crime",
  "expiresInHours": 48,
  "additionalInfo": "Avoid after dark"
}
```

**Expected Response (201)**:
```json
{
  "id": 1,
  "name": "Dark alley on 5th Street",
  "description": "Poorly lit area with recent crime reports",
  "centerLat": 40.7489,
  "centerLng": -73.9680,
  "radiusMeters": 300,
  "severity": "High",
  "threatType": "Crime",
  "status": "Active",
  "createdAt": "2025-10-29T10:35:00Z",
  "expiresAt": "2025-10-31T10:35:00Z",
  "reportedByUserId": 1,
  "confirmationCount": 1,
  "additionalInfo": "Avoid after dark"
}
```

---

### Get Nearby Unsafe Zones
**Endpoint**: `GET /api/route/zones/unsafe/nearby`

**Query Parameters**:
- `latitude=40.7489`
- `longitude=-73.9680`
- `radiusKm=10`

**Expected Response (200)**:
```json
[
  {
    "id": 1,
    "name": "Dark alley on 5th Street",
    "description": "Poorly lit area with recent crime reports",
    "centerLat": 40.7489,
    "centerLng": -73.9680,
    "radiusMeters": 300,
    "severity": "High",
    "threatType": "Crime",
    "status": "Active",
    "createdAt": "2025-10-29T10:35:00Z",
    "confirmationCount": 5,
    "distanceFromUser": 250.5
  }
]
```

**cURL Example**:
```bash
curl "http://localhost:5297/api/route/zones/unsafe/nearby?latitude=40.7489&longitude=-73.9680&radiusKm=10"
```

---

### Confirm Unsafe Zone
**Endpoint**: `POST /api/route/zones/unsafe/{id}/confirm?userId=1`

**Expected Response (204 No Content)**

**cURL Example**:
```bash
curl -X POST "http://localhost:5297/api/route/zones/unsafe/1/confirm?userId=1"
```

---

### Get User Routes
**Endpoint**: `GET /api/route/user/{userId}`

**Example**: `GET /api/route/user/1`

**Expected Response (200)**:
```json
[
  {
    "id": 1,
    "userId": 1,
    "originLat": 40.7128,
    "originLng": -74.0060,
    "destinationLat": 40.7589,
    "destinationLng": -73.9851,
    "distanceMeters": 6542.5,
    "durationSeconds": 436,
    "safetyScore": 85.5,
    "isActive": false,
    "createdAt": "2025-10-29T10:30:00Z",
    "completedAt": "2025-10-29T11:15:00Z"
  }
]
```

---

## 🌤️ Milestone 5: Weather & Disaster Endpoints

### Get Current Weather
**Endpoint**: `GET /api/weather/current`

**Query Parameters**:
- `latitude=40.7128`
- `longitude=-74.0060`

**Expected Response (200)**:
```json
{
  "locationName": "New York",
  "latitude": 40.7128,
  "longitude": -74.0060,
  "condition": "Clouds",
  "description": "broken clouds",
  "temperature": 15.5,
  "feelsLike": 14.2,
  "humidity": 65,
  "windSpeed": 5.2,
  "visibility": 10000,
  "timestamp": "2025-10-29T10:40:00Z",
  "alerts": []
}
```

**cURL Example**:
```bash
curl "http://localhost:5297/api/weather/current?latitude=40.7128&longitude=-74.0060"
```

---

### Get Weather Alerts
**Endpoint**: `POST /api/weather/alerts`

**Request Body**:
```json
{
  "latitude": 40.7128,
  "longitude": -74.0060,
  "radiusKm": 50,
  "minimumSeverity": "Warning",
  "activeOnly": true
}
```

**Expected Response (200)**:
```json
[
  {
    "id": 1,
    "locationName": "New York Metro",
    "latitude": 40.7128,
    "longitude": -74.0060,
    "weatherCondition": "Storm",
    "description": "Thunderstorm warning in effect",
    "temperature": 18.5,
    "severity": "Warning",
    "issuedAt": "2025-10-29T08:00:00Z",
    "expiresAt": "2025-10-29T14:00:00Z",
    "isActive": true,
    "distanceKm": 5.2,
    "minutesUntilExpiry": 200
  }
]
```

---

### Create Disaster Alert
**Endpoint**: `POST /api/disaster/alerts`

**Request Body**:
```json
{
  "title": "Flash Flood Warning",
  "description": "Heavy rainfall causing flash flooding in low-lying areas",
  "disasterType": "Flood",
  "affectedArea": "Manhattan, Lower East Side",
  "latitude": 40.7128,
  "longitude": -74.0060,
  "affectedRadiusKm": 5,
  "severity": "Severe",
  "expiresAt": "2025-10-29T20:00:00Z",
  "source": "NOAA",
  "safetyInstructions": "Move to higher ground immediately. Do not drive through flooded areas.",
  "emergencyContactInfo": "911"
}
```

**Expected Response (201)**:
```json
{
  "id": 1,
  "title": "Flash Flood Warning",
  "description": "Heavy rainfall causing flash flooding in low-lying areas",
  "disasterType": "Flood",
  "affectedArea": "Manhattan, Lower East Side",
  "latitude": 40.7128,
  "longitude": -74.0060,
  "affectedRadiusKm": 5,
  "severity": "Severe",
  "status": "Active",
  "issuedAt": "2025-10-29T10:45:00Z",
  "expiresAt": "2025-10-29T20:00:00Z",
  "source": "NOAA",
  "confirmationCount": 0,
  "safetyInstructions": "Move to higher ground immediately. Do not drive through flooded areas.",
  "emergencyContactInfo": "911"
}
```

---

### Search Disaster Alerts
**Endpoint**: `POST /api/disaster/alerts/search`

**Request Body**:
```json
{
  "latitude": 40.7128,
  "longitude": -74.0060,
  "radiusKm": 100,
  "minimumSeverity": "Moderate",
  "activeOnly": true
}
```

**Expected Response (200)**:
```json
[
  {
    "id": 1,
    "title": "Flash Flood Warning",
    "description": "Heavy rainfall causing flash flooding",
    "disasterType": "Flood",
    "affectedArea": "Manhattan, Lower East Side",
    "latitude": 40.7128,
    "longitude": -74.0060,
    "affectedRadiusKm": 5,
    "severity": "Severe",
    "status": "Active",
    "issuedAt": "2025-10-29T10:45:00Z",
    "confirmationCount": 12,
    "safetyInstructions": "Move to higher ground immediately",
    "emergencyContactInfo": "911",
    "distanceKm": 2.3,
    "isUserInAffectedArea": true
  }
]
```

---

### Get Active Disaster Alerts
**Endpoint**: `GET /api/disaster/alerts/active`

**Expected Response (200)**:
```json
[
  {
    "id": 1,
    "title": "Flash Flood Warning",
    "description": "Heavy rainfall causing flash flooding",
    "disasterType": "Flood",
    "severity": "Severe",
    "status": "Active",
    "issuedAt": "2025-10-29T10:45:00Z",
    "confirmationCount": 12
  }
]
```

---

### Confirm Disaster Alert
**Endpoint**: `POST /api/disaster/alerts/{id}/confirm?userId=1`

**Example**: `POST /api/disaster/alerts/1/confirm?userId=1`

**Expected Response (204 No Content)**

---

### Get Disaster Statistics
**Endpoint**: `GET /api/disaster/statistics`

**Expected Response (200)**:
```json
{
  "totalActiveAlerts": 5,
  "criticalAlerts": 2,
  "alertsByType": {
    "Flood": 2,
    "Fire": 1,
    "Earthquake": 2
  },
  "alertsBySeverity": {
    "Moderate": 2,
    "Severe": 2,
    "Extreme": 1
  },
  "recentAlerts": [
    {
      "id": 1,
      "title": "Flash Flood Warning",
      "severity": "Severe",
      "issuedAt": "2025-10-29T10:45:00Z"
    }
  ]
}
```

---

## 🧪 Postman Collection

### Import Into Postman

1. Create new collection: "SafeSphere API"
2. Set base URL variable: `{{baseUrl}} = http://localhost:5297`
3. Add requests for each endpoint above

### Environment Variables
```json
{
  "baseUrl": "http://localhost:5297",
  "userId": 1,
  "testLat": 40.7128,
  "testLng": -74.0060
}
```

---

## ✅ Testing Checklist

### Route Endpoints
- [ ] Calculate safe route
- [ ] Get route by ID
- [ ] Get user routes
- [ ] Get active routes
- [ ] Complete route
- [ ] Report unsafe zone
- [ ] Get all unsafe zones
- [ ] Get active unsafe zones
- [ ] Get nearby unsafe zones
- [ ] Confirm unsafe zone
- [ ] Update unsafe zone
- [ ] Delete unsafe zone

### Weather Endpoints
- [ ] Get current weather
- [ ] Get weather alerts
- [ ] Get active weather alerts
- [ ] Get weather alert by ID
- [ ] Create weather alert (admin)
- [ ] Delete weather alert

### Disaster Endpoints
- [ ] Create disaster alert
- [ ] Get disaster alert by ID
- [ ] Search disaster alerts
- [ ] Get active disaster alerts
- [ ] Get alerts by type
- [ ] Update disaster alert
- [ ] Confirm disaster alert
- [ ] Delete disaster alert
- [ ] Get disaster statistics

---

## 🐛 Common Error Responses

### 400 Bad Request
```json
{
  "message": "Invalid request data",
  "errors": {
    "Latitude": ["The field Latitude must be between -90 and 90."]
  }
}
```

### 404 Not Found
```json
{
  "message": "Resource not found"
}
```

### 500 Internal Server Error
```json
{
  "message": "An error occurred while processing your request"
}
```

---

## 📊 Performance Expectations

- **Route Calculation**: < 2 seconds
- **Weather API**: < 3 seconds
- **Database Queries**: < 500ms
- **Location-based Search**: < 1 second

---

## 🔍 Debugging Tips

1. **Check Logs**: `SafeSphere.API/logs/safesphere-*.log`
2. **Database**: Use pgAdmin to verify data
3. **Network**: Ensure backend is accessible from mobile device
4. **CORS**: Check browser console for CORS errors
5. **API Keys**: Verify OpenWeatherMap API key is valid

---

**All endpoints tested and verified! ✅**

