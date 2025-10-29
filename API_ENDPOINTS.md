# SafeSphere API - Endpoints Reference

## Base URL
```
Development: https://localhost:7000
Production: https://your-domain.com
```

---

## 👤 User Endpoints

### 1. Register User
Creates a new user account with password hashing.

**Endpoint:** `POST /api/user/register`

**Request Body:**
```json
{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "phone": "+1234567890",
  "password": "securePassword123",
  "emergencyContacts": "Jane Doe:+0987654321,Police:911"
}
```

**Success Response (201 Created):**
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john.doe@example.com",
  "phone": "+1234567890",
  "emergencyContacts": "Jane Doe:+0987654321,Police:911",
  "createdAt": "2025-10-27T10:30:00Z"
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "Email already exists or registration failed"
}
```

---

### 2. Login User
Authenticates user and returns user information.

**Endpoint:** `POST /api/user/login`

**Request Body:**
```json
{
  "email": "john.doe@example.com",
  "password": "securePassword123"
}
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john.doe@example.com",
  "phone": "+1234567890",
  "emergencyContacts": "Jane Doe:+0987654321,Police:911",
  "createdAt": "2025-10-27T10:30:00Z"
}
```

**Error Response (401 Unauthorized):**
```json
{
  "message": "Invalid email or password"
}
```

---

### 3. Get User by ID
Retrieves user information by user ID.

**Endpoint:** `GET /api/user/{id}`

**URL Parameters:**
- `id` (integer) - User ID

**Success Response (200 OK):**
```json
{
  "id": 1,
  "name": "John Doe",
  "email": "john.doe@example.com",
  "phone": "+1234567890",
  "emergencyContacts": "Jane Doe:+0987654321,Police:911",
  "createdAt": "2025-10-27T10:30:00Z"
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "User not found"
}
```

---

### 4. Update User
Updates user information (partial update supported).

**Endpoint:** `PUT /api/user/{id}`

**URL Parameters:**
- `id` (integer) - User ID

**Request Body:**
```json
{
  "name": "John Updated",
  "phone": "+1234567899",
  "emergencyContacts": "Updated Contact:+1111111111"
}
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "name": "John Updated",
  "email": "john.doe@example.com",
  "phone": "+1234567899",
  "emergencyContacts": "Updated Contact:+1111111111",
  "createdAt": "2025-10-27T10:30:00Z"
}
```

---

### 5. Delete User
Deletes a user and all associated alerts.

**Endpoint:** `DELETE /api/user/{id}`

**URL Parameters:**
- `id` (integer) - User ID

**Success Response (204 No Content)**

**Error Response (404 Not Found):**
```json
{
  "message": "User not found"
}
```

---

## 🚨 Panic Alert Endpoints

### 1. Create Panic Alert
Creates a new panic alert with GPS location.

**Endpoint:** `POST /api/alert/panic?userId={userId}`

**Query Parameters:**
- `userId` (integer) - User ID

**Request Body:**
```json
{
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "additionalInfo": "Need immediate help"
}
```

**Success Response (201 Created):**
```json
{
  "id": 1,
  "userId": 1,
  "userName": "John Doe",
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "timestamp": "2025-10-27T10:35:00Z",
  "status": "Active",
  "additionalInfo": "Need immediate help"
}
```

---

### 2. Get Panic Alert by ID
Retrieves a specific panic alert.

**Endpoint:** `GET /api/alert/panic/{id}`

**URL Parameters:**
- `id` (integer) - Panic Alert ID

**Success Response (200 OK):**
```json
{
  "id": 1,
  "userId": 1,
  "userName": "John Doe",
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "timestamp": "2025-10-27T10:35:00Z",
  "status": "Active",
  "additionalInfo": "Need immediate help"
}
```

---

### 3. Get All Panic Alerts
Retrieves all panic alerts, sorted by timestamp (newest first).

**Endpoint:** `GET /api/alert/panic`

**Success Response (200 OK):**
```json
[
  {
    "id": 2,
    "userId": 2,
    "userName": "Jane Smith",
    "locationLat": 34.0522,
    "locationLng": -118.2437,
    "timestamp": "2025-10-27T11:00:00Z",
    "status": "Active",
    "additionalInfo": "Car trouble"
  },
  {
    "id": 1,
    "userId": 1,
    "userName": "John Doe",
    "locationLat": 40.7128,
    "locationLng": -74.0060,
    "timestamp": "2025-10-27T10:35:00Z",
    "status": "Resolved",
    "additionalInfo": "False alarm"
  }
]
```

---

### 4. Get Panic Alerts by User
Retrieves all panic alerts for a specific user.

**Endpoint:** `GET /api/alert/panic/user/{userId}`

**URL Parameters:**
- `userId` (integer) - User ID

**Success Response (200 OK):**
```json
[
  {
    "id": 1,
    "userId": 1,
    "userName": "John Doe",
    "locationLat": 40.7128,
    "locationLng": -74.0060,
    "timestamp": "2025-10-27T10:35:00Z",
    "status": "Active",
    "additionalInfo": "Need immediate help"
  }
]
```

---

### 5. Get Active Panic Alerts
Retrieves only panic alerts with "Active" status.

**Endpoint:** `GET /api/alert/panic/active`

**Success Response (200 OK):**
```json
[
  {
    "id": 2,
    "userId": 2,
    "userName": "Jane Smith",
    "locationLat": 34.0522,
    "locationLng": -118.2437,
    "timestamp": "2025-10-27T11:00:00Z",
    "status": "Active",
    "additionalInfo": "Car trouble"
  }
]
```

---

### 6. Update Panic Alert Status
Updates the status of a panic alert.

**Endpoint:** `PATCH /api/alert/panic/{id}/status`

**URL Parameters:**
- `id` (integer) - Panic Alert ID

**Request Body:**
```json
{
  "status": "Resolved"
}
```

**Valid Status Values:**
- `Active`
- `Resolved`
- `Cancelled`

**Success Response (200 OK):**
```json
{
  "id": 1,
  "userId": 1,
  "userName": "John Doe",
  "locationLat": 40.7128,
  "locationLng": -74.0060,
  "timestamp": "2025-10-27T10:35:00Z",
  "status": "Resolved",
  "additionalInfo": "Need immediate help"
}
```

---

### 7. Delete Panic Alert
Deletes a panic alert.

**Endpoint:** `DELETE /api/alert/panic/{id}`

**URL Parameters:**
- `id` (integer) - Panic Alert ID

**Success Response (204 No Content)**

---

## 🆘 SOS Alert Endpoints

### 1. Create SOS Alert
Creates a new SOS alert with message and location.

**Endpoint:** `POST /api/alert/sos?userId={userId}`

**Query Parameters:**
- `userId` (integer) - User ID

**Request Body:**
```json
{
  "message": "Car accident on Highway 101",
  "location": "Highway 101, San Francisco",
  "locationLat": 37.7749,
  "locationLng": -122.4194
}
```

**Success Response (201 Created):**
```json
{
  "id": 1,
  "userId": 1,
  "userName": "John Doe",
  "message": "Car accident on Highway 101",
  "location": "Highway 101, San Francisco",
  "locationLat": 37.7749,
  "locationLng": -122.4194,
  "timestamp": "2025-10-27T10:40:00Z",
  "acknowledged": false,
  "acknowledgedAt": null
}
```

---

### 2. Get SOS Alert by ID
Retrieves a specific SOS alert.

**Endpoint:** `GET /api/alert/sos/{id}`

**URL Parameters:**
- `id` (integer) - SOS Alert ID

**Success Response (200 OK):**
```json
{
  "id": 1,
  "userId": 1,
  "userName": "John Doe",
  "message": "Car accident on Highway 101",
  "location": "Highway 101, San Francisco",
  "locationLat": 37.7749,
  "locationLng": -122.4194,
  "timestamp": "2025-10-27T10:40:00Z",
  "acknowledged": false,
  "acknowledgedAt": null
}
```

---

### 3. Get All SOS Alerts
Retrieves all SOS alerts, sorted by timestamp (newest first).

**Endpoint:** `GET /api/alert/sos`

**Success Response (200 OK):**
```json
[
  {
    "id": 2,
    "userId": 2,
    "userName": "Jane Smith",
    "message": "Feeling unsafe",
    "location": "Downtown, Los Angeles",
    "locationLat": 34.0522,
    "locationLng": -118.2437,
    "timestamp": "2025-10-27T11:15:00Z",
    "acknowledged": false,
    "acknowledgedAt": null
  },
  {
    "id": 1,
    "userId": 1,
    "userName": "John Doe",
    "message": "Car accident on Highway 101",
    "location": "Highway 101, San Francisco",
    "locationLat": 37.7749,
    "locationLng": -122.4194,
    "timestamp": "2025-10-27T10:40:00Z",
    "acknowledged": true,
    "acknowledgedAt": "2025-10-27T10:45:00Z"
  }
]
```

---

### 4. Get SOS Alerts by User
Retrieves all SOS alerts for a specific user.

**Endpoint:** `GET /api/alert/sos/user/{userId}`

**URL Parameters:**
- `userId` (integer) - User ID

**Success Response (200 OK):**
```json
[
  {
    "id": 1,
    "userId": 1,
    "userName": "John Doe",
    "message": "Car accident on Highway 101",
    "location": "Highway 101, San Francisco",
    "locationLat": 37.7749,
    "locationLng": -122.4194,
    "timestamp": "2025-10-27T10:40:00Z",
    "acknowledged": true,
    "acknowledgedAt": "2025-10-27T10:45:00Z"
  }
]
```

---

### 5. Get Unacknowledged SOS Alerts
Retrieves only SOS alerts that haven't been acknowledged.

**Endpoint:** `GET /api/alert/sos/unacknowledged`

**Success Response (200 OK):**
```json
[
  {
    "id": 2,
    "userId": 2,
    "userName": "Jane Smith",
    "message": "Feeling unsafe",
    "location": "Downtown, Los Angeles",
    "locationLat": 34.0522,
    "locationLng": -118.2437,
    "timestamp": "2025-10-27T11:15:00Z",
    "acknowledged": false,
    "acknowledgedAt": null
  }
]
```

---

### 6. Acknowledge SOS Alert
Marks an SOS alert as acknowledged or unacknowledged.

**Endpoint:** `PATCH /api/alert/sos/{id}/acknowledge`

**URL Parameters:**
- `id` (integer) - SOS Alert ID

**Request Body:**
```json
{
  "acknowledged": true
}
```

**Success Response (200 OK):**
```json
{
  "id": 1,
  "userId": 1,
  "userName": "John Doe",
  "message": "Car accident on Highway 101",
  "location": "Highway 101, San Francisco",
  "locationLat": 37.7749,
  "locationLng": -122.4194,
  "timestamp": "2025-10-27T10:40:00Z",
  "acknowledged": true,
  "acknowledgedAt": "2025-10-27T10:45:00Z"
}
```

---

### 7. Delete SOS Alert
Deletes an SOS alert.

**Endpoint:** `DELETE /api/alert/sos/{id}`

**URL Parameters:**
- `id` (integer) - SOS Alert ID

**Success Response (204 No Content)**

---

## 🏥 Health Check Endpoint

### Health Status
Check if the API is running.

**Endpoint:** `GET /health`

**Success Response (200 OK):**
```json
{
  "status": "healthy",
  "timestamp": "2025-10-27T12:00:00Z"
}
```

---

## 📋 Common HTTP Status Codes

| Code | Meaning | Description |
|------|---------|-------------|
| 200 | OK | Request succeeded |
| 201 | Created | Resource created successfully |
| 204 | No Content | Request succeeded, no content returned |
| 400 | Bad Request | Invalid request data |
| 401 | Unauthorized | Authentication failed |
| 404 | Not Found | Resource not found |
| 500 | Internal Server Error | Server error |

---

## 🔒 Security Notes

1. **Password Storage**: Passwords are hashed with BCrypt before storage
2. **CORS**: Currently allows all origins for development
3. **HTTPS**: Use HTTPS in production
4. **Validation**: All inputs are validated with Data Annotations

---

## 📝 Sample Workflow

### User Registration and Alert Creation

```bash
# 1. Register a new user
curl -X POST "https://localhost:7000/api/user/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test User",
    "email": "test@example.com",
    "phone": "+1234567890",
    "password": "password123",
    "emergencyContacts": "Emergency:911"
  }'

# Response: { "id": 3, ... }

# 2. Create a panic alert
curl -X POST "https://localhost:7000/api/alert/panic?userId=3" \
  -H "Content-Type: application/json" \
  -d '{
    "locationLat": 40.7128,
    "locationLng": -74.0060,
    "additionalInfo": "Emergency"
  }'

# 3. Get all active panic alerts
curl -X GET "https://localhost:7000/api/alert/panic/active"

# 4. Update panic alert status
curl -X PATCH "https://localhost:7000/api/alert/panic/1/status" \
  -H "Content-Type: application/json" \
  -d '{ "status": "Resolved" }'
```

---

**For interactive testing, use Swagger UI at `https://localhost:7000`**

