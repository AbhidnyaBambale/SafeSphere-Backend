/**
 * API Configuration for SafeSphere Backend
 * Update the BASE_URL to match your backend server
 */

// For local development on physical device, use your computer's IP address
// For Android Emulator, use 10.0.2.2
// For iOS Simulator, use localhost
export const API_CONFIG = {
  // Change this to your actual API URL
  BASE_URL: 'http://172.29.128.1:5297', // Replace with your computer's IP
  
  // Alternative URLs for different environments
  ANDROID_EMULATOR_URL: 'http://10.0.2.2:5297',
  IOS_SIMULATOR_URL: 'http://localhost:5297',
  
  TIMEOUT: 30000, // 30 seconds
  
  ENDPOINTS: {
    // User endpoints
    USER_REGISTER: '/api/user/register',
    USER_LOGIN: '/api/user/login',
    USER_PROFILE: '/api/user',
    
    // Alert endpoints
    PANIC_ALERT: '/api/alert/panic',
    SOS_ALERT: '/api/alert/sos',
    GET_USER_ALERTS: '/api/alert',
    GET_PANIC_ALERT: (id: number) => `/api/alert/panic/${id}`,
    GET_SOS_ALERT: (id: number) => `/api/alert/sos/${id}`,
    GET_USER_PANIC_ALERTS: (userId: number) => `/api/alert/panic/user/${userId}`,
    GET_USER_SOS_ALERTS: (userId: number) => `/api/alert/sos/user/${userId}`,
    UPDATE_PANIC_STATUS: (id: number) => `/api/alert/panic/${id}/status`,
    ACKNOWLEDGE_SOS: (id: number) => `/api/alert/sos/${id}/acknowledge`,
    
    // Health check
    HEALTH: '/health'
  }
};

// Helper to get the correct base URL based on platform
export const getBaseUrl = (): string => {
  // You can add platform-specific logic here
  return API_CONFIG.BASE_URL;
};

