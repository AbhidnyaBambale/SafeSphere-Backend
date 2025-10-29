import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import SOSService, {
  CreateSOSAlertRequest,
  SOSAlertResponse,
} from '../../services/sos.service';
import { ApiError } from '../../services/api.service';

/**
 * SOS Alert Redux Slice
 * Manages SOS alert state with async actions
 */

interface SOSAlertState {
  alerts: SOSAlertResponse[];
  currentAlert: SOSAlertResponse | null;
  loading: boolean;
  error: string | null;
  success: boolean;
}

const initialState: SOSAlertState = {
  alerts: [],
  currentAlert: null,
  loading: false,
  error: null,
  success: false,
};

// Async Thunks

export const createSOSAlert = createAsyncThunk<
  SOSAlertResponse,
  { userId: number; alertData: CreateSOSAlertRequest },
  { rejectValue: string }
>(
  'sosAlert/create',
  async ({ userId, alertData }, { rejectWithValue }) => {
    try {
      const response = await SOSService.createSOSAlert(userId, alertData);
      return response;
    } catch (error: any) {
      const apiError = error as ApiError;
      return rejectWithValue(apiError.message || 'Failed to create SOS alert');
    }
  }
);

export const fetchUserSOSAlerts = createAsyncThunk<
  SOSAlertResponse[],
  number,
  { rejectValue: string }
>(
  'sosAlert/fetchUserAlerts',
  async (userId, { rejectWithValue }) => {
    try {
      const response = await SOSService.getUserSOSAlerts(userId);
      return response;
    } catch (error: any) {
      const apiError = error as ApiError;
      return rejectWithValue(apiError.message || 'Failed to fetch SOS alerts');
    }
  }
);

export const acknowledgeSOSAlert = createAsyncThunk<
  SOSAlertResponse,
  { id: number; acknowledged: boolean },
  { rejectValue: string }
>(
  'sosAlert/acknowledge',
  async ({ id, acknowledged }, { rejectWithValue }) => {
    try {
      const response = await SOSService.acknowledgeSOSAlert(id, acknowledged);
      return response;
    } catch (error: any) {
      const apiError = error as ApiError;
      return rejectWithValue(apiError.message || 'Failed to acknowledge SOS alert');
    }
  }
);

export const deleteSOSAlert = createAsyncThunk<
  number,
  number,
  { rejectValue: string }
>(
  'sosAlert/delete',
  async (id, { rejectWithValue }) => {
    try {
      await SOSService.deleteSOSAlert(id);
      return id;
    } catch (error: any) {
      const apiError = error as ApiError;
      return rejectWithValue(apiError.message || 'Failed to delete SOS alert');
    }
  }
);

// Slice

const sosAlertSlice = createSlice({
  name: 'sosAlert',
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
    clearSuccess: (state) => {
      state.success = false;
    },
    resetSOSAlert: (state) => {
      state.currentAlert = null;
      state.error = null;
      state.success = false;
    },
  },
  extraReducers: (builder) => {
    // Create SOS Alert
    builder
      .addCase(createSOSAlert.pending, (state) => {
        state.loading = true;
        state.error = null;
        state.success = false;
      })
      .addCase(createSOSAlert.fulfilled, (state, action: PayloadAction<SOSAlertResponse>) => {
        state.loading = false;
        state.currentAlert = action.payload;
        state.alerts.unshift(action.payload); // Add to beginning of array
        state.success = true;
        state.error = null;
      })
      .addCase(createSOSAlert.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || 'Failed to create SOS alert';
        state.success = false;
      });

    // Fetch User SOS Alerts
    builder
      .addCase(fetchUserSOSAlerts.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchUserSOSAlerts.fulfilled, (state, action: PayloadAction<SOSAlertResponse[]>) => {
        state.loading = false;
        state.alerts = action.payload;
        state.error = null;
      })
      .addCase(fetchUserSOSAlerts.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || 'Failed to fetch SOS alerts';
      });

    // Acknowledge SOS Alert
    builder
      .addCase(acknowledgeSOSAlert.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(acknowledgeSOSAlert.fulfilled, (state, action: PayloadAction<SOSAlertResponse>) => {
        state.loading = false;
        state.currentAlert = action.payload;
        // Update in alerts array
        const index = state.alerts.findIndex(alert => alert.id === action.payload.id);
        if (index !== -1) {
          state.alerts[index] = action.payload;
        }
        state.error = null;
      })
      .addCase(acknowledgeSOSAlert.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || 'Failed to acknowledge SOS alert';
      });

    // Delete SOS Alert
    builder
      .addCase(deleteSOSAlert.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deleteSOSAlert.fulfilled, (state, action: PayloadAction<number>) => {
        state.loading = false;
        state.alerts = state.alerts.filter(alert => alert.id !== action.payload);
        if (state.currentAlert?.id === action.payload) {
          state.currentAlert = null;
        }
        state.error = null;
      })
      .addCase(deleteSOSAlert.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || 'Failed to delete SOS alert';
      });
  },
});

export const { clearError, clearSuccess, resetSOSAlert } = sosAlertSlice.actions;
export default sosAlertSlice.reducer;

