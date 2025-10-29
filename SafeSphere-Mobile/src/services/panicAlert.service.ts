import ApiService from './api.service';
import { API_CONFIG } from '../config/api.config';

/**
 * Panic Alert Service
 * Handles all panic alert related API calls
 */

export interface CreatePanicAlertRequest {
  locationLat: number;
  locationLng: number;
  additionalInfo?: string;
}

export interface PanicAlertResponse {
  id: number;
  userId: number;
  userName: string;
  locationLat: number;
  locationLng: number;
  timestamp: string;
  status: 'Active' | 'Resolved' | 'Cancelled';
  additionalInfo?: string;
}

export interface UpdatePanicAlertStatusRequest {
  status: 'Active' | 'Resolved' | 'Cancelled';
}

class PanicAlertService {
  /**
   * Create a new panic alert
   */
  async createPanicAlert(
    userId: number,
    alertData: CreatePanicAlertRequest
  ): Promise<PanicAlertResponse> {
    try {
      console.log('[PanicAlert] Creating alert for user:', userId);
      console.log('[PanicAlert] Alert data:', alertData);
      
      const response = await ApiService.post<PanicAlertResponse>(
        `${API_CONFIG.ENDPOINTS.PANIC_ALERT}?userId=${userId}`,
        alertData
      );
      
      console.log('[PanicAlert] Alert created successfully:', response.id);
      return response;
    } catch (error) {
      console.error('[PanicAlert] Failed to create alert:', error);
      throw error;
    }
  }

  /**
   * Get panic alert by ID
   */
  async getPanicAlertById(id: number): Promise<PanicAlertResponse> {
    try {
      return await ApiService.get<PanicAlertResponse>(
        API_CONFIG.ENDPOINTS.GET_PANIC_ALERT(id)
      );
    } catch (error) {
      console.error('[PanicAlert] Failed to get alert:', error);
      throw error;
    }
  }

  /**
   * Get all panic alerts for a user
   */
  async getUserPanicAlerts(userId: number): Promise<PanicAlertResponse[]> {
    try {
      return await ApiService.get<PanicAlertResponse[]>(
        API_CONFIG.ENDPOINTS.GET_USER_PANIC_ALERTS(userId)
      );
    } catch (error) {
      console.error('[PanicAlert] Failed to get user alerts:', error);
      throw error;
    }
  }

  /**
   * Update panic alert status
   */
  async updatePanicAlertStatus(
    id: number,
    status: 'Active' | 'Resolved' | 'Cancelled'
  ): Promise<PanicAlertResponse> {
    try {
      return await ApiService.patch<PanicAlertResponse>(
        API_CONFIG.ENDPOINTS.UPDATE_PANIC_STATUS(id),
        { status }
      );
    } catch (error) {
      console.error('[PanicAlert] Failed to update status:', error);
      throw error;
    }
  }

  /**
   * Delete panic alert
   */
  async deletePanicAlert(id: number): Promise<void> {
    try {
      await ApiService.delete(API_CONFIG.ENDPOINTS.GET_PANIC_ALERT(id));
    } catch (error) {
      console.error('[PanicAlert] Failed to delete alert:', error);
      throw error;
    }
  }
}

export default new PanicAlertService();

