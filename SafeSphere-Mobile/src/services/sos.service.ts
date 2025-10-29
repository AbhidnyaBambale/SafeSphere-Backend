import ApiService from './api.service';
import { API_CONFIG } from '../config/api.config';

/**
 * SOS Alert Service
 * Handles all SOS alert related API calls
 */

export interface CreateSOSAlertRequest {
  message: string;
  location: string;
  locationLat?: number;
  locationLng?: number;
}

export interface SOSAlertResponse {
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

export interface AcknowledgeSOSRequest {
  acknowledged: boolean;
}

class SOSService {
  /**
   * Create a new SOS alert
   */
  async createSOSAlert(
    userId: number,
    alertData: CreateSOSAlertRequest
  ): Promise<SOSAlertResponse> {
    try {
      console.log('[SOS] Creating SOS alert for user:', userId);
      console.log('[SOS] Alert data:', alertData);
      
      const response = await ApiService.post<SOSAlertResponse>(
        `${API_CONFIG.ENDPOINTS.SOS_ALERT}?userId=${userId}`,
        alertData
      );
      
      console.log('[SOS] SOS alert created successfully:', response.id);
      return response;
    } catch (error) {
      console.error('[SOS] Failed to create SOS alert:', error);
      throw error;
    }
  }

  /**
   * Get SOS alert by ID
   */
  async getSOSAlertById(id: number): Promise<SOSAlertResponse> {
    try {
      return await ApiService.get<SOSAlertResponse>(
        API_CONFIG.ENDPOINTS.GET_SOS_ALERT(id)
      );
    } catch (error) {
      console.error('[SOS] Failed to get SOS alert:', error);
      throw error;
    }
  }

  /**
   * Get all SOS alerts for a user
   */
  async getUserSOSAlerts(userId: number): Promise<SOSAlertResponse[]> {
    try {
      return await ApiService.get<SOSAlertResponse[]>(
        API_CONFIG.ENDPOINTS.GET_USER_SOS_ALERTS(userId)
      );
    } catch (error) {
      console.error('[SOS] Failed to get user SOS alerts:', error);
      throw error;
    }
  }

  /**
   * Acknowledge SOS alert
   */
  async acknowledgeSOSAlert(
    id: number,
    acknowledged: boolean = true
  ): Promise<SOSAlertResponse> {
    try {
      return await ApiService.patch<SOSAlertResponse>(
        API_CONFIG.ENDPOINTS.ACKNOWLEDGE_SOS(id),
        { acknowledged }
      );
    } catch (error) {
      console.error('[SOS] Failed to acknowledge SOS alert:', error);
      throw error;
    }
  }

  /**
   * Delete SOS alert
   */
  async deleteSOSAlert(id: number): Promise<void> {
    try {
      await ApiService.delete(API_CONFIG.ENDPOINTS.GET_SOS_ALERT(id));
    } catch (error) {
      console.error('[SOS] Failed to delete SOS alert:', error);
      throw error;
    }
  }
}

export default new SOSService();

