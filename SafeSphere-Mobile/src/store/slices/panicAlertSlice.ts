import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';
import PanicAlertService, {
  CreatePanicAlertRequest,
  PanicAlertResponse,
} from '../../services/panicAlert.service';
import { ApiError } from '../../services/api.service';

/**
 * Panic Alert Redux Slice
 * Manages panic alert state with async actions
 */

interface PanicAlertState {
  alerts: PanicAlertResponse[];
  currentAlert: PanicAlertResponse | null;
  loading: boolean;
  error: string | null;
  success: boolean;
}

const initialState: PanicAlertState = {
  alerts: [],
  currentAlert: null,
  loading: false,
  error: null,
  success: false,
};

// Async Thunks

export const createPanicAlert = createAsyncThunk<
  PanicAlertResponse,
  { userId: number; alertData: CreatePanicAlertRequest },
  { rejectValue: string }
>(
  'panicAlert/create',
  async ({ userId, alertData }, { rejectWithValue }) => {
    try {
      const response = await PanicAlertService.createPanicAlert(userId, alertData);
      return response;
    } catch (error: any) {
      const apiError = error as ApiError;
      return rejectWithValue(apiError.message || 'Failed to create panic alert');
    }
  }
);

export const fetchUserPanicAlerts = createAsyncThunk<
  PanicAlertResponse[],
  number,
  { rejectValue: string }
>(
  'panicAlert/fetchUserAlerts',
  async (userId, { rejectWithValue }) => {
    try {
      const response = await PanicAlertService.getUserPanicAlerts(userId);
      return response;
    } catch (error: any) {
      const apiError = error as ApiError;
      return rejectWithValue(apiError.message || 'Failed to fetch panic alerts');
    }
  }
);

export const updatePanicAlertStatus = createAsyncThunk<
  PanicAlertResponse,
  { id: number; status: 'Active' | 'Resolved' | 'Cancelled' },
  { rejectValue: string }
>(
  'panicAlert/updateStatus',
  async ({ id, status }, { rejectWithValue }) => {
    try {
      const response = await PanicAlertService.updatePanicAlertStatus(id, status);
      return response;
    } catch (error: any) {
      const apiError = error as ApiError;
      return rejectWithValue(apiError.message || 'Failed to update panic alert status');
    }
  }
);

export const deletePanicAlert = createAsyncThunk<
  number,
  number,
  { rejectValue: string }
>(
  'panicAlert/delete',
  async (id, { rejectWithValue }) => {
    try {
      await PanicAlertService.deletePanicAlert(id);
      return id;
    } catch (error: any) {
      const apiError = error as ApiError;
      return rejectWithValue(apiError.message || 'Failed to delete panic alert');
    }
  }
);

// Slice

const panicAlertSlice = createSlice({
  name: 'panicAlert',
  initialState,
  reducers: {
    clearError: (state) => {
      state.error = null;
    },
    clearSuccess: (state) => {
      state.success = false;
    },
    resetPanicAlert: (state) => {
      state.currentAlert = null;
      state.error = null;
      state.success = false;
    },
  },
  extraReducers: (builder) => {
    // Create Panic Alert
    builder
      .addCase(createPanicAlert.pending, (state) => {
        state.loading = true;
        state.error = null;
        state.success = false;
      })
      .addCase(createPanicAlert.fulfilled, (state, action: PayloadAction<PanicAlertResponse>) => {
        state.loading = false;
        state.currentAlert = action.payload;
        state.alerts.unshift(action.payload); // Add to beginning of array
        state.success = true;
        state.error = null;
      })
      .addCase(createPanicAlert.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || 'Failed to create panic alert';
        state.success = false;
      });

    // Fetch User Panic Alerts
    builder
      .addCase(fetchUserPanicAlerts.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchUserPanicAlerts.fulfilled, (state, action: PayloadAction<PanicAlertResponse[]>) => {
        state.loading = false;
        state.alerts = action.payload;
        state.error = null;
      })
      .addCase(fetchUserPanicAlerts.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || 'Failed to fetch panic alerts';
      });

    // Update Panic Alert Status
    builder
      .addCase(updatePanicAlertStatus.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updatePanicAlertStatus.fulfilled, (state, action: PayloadAction<PanicAlertResponse>) => {
        state.loading = false;
        state.currentAlert = action.payload;
        // Update in alerts array
        const index = state.alerts.findIndex(alert => alert.id === action.payload.id);
        if (index !== -1) {
          state.alerts[index] = action.payload;
        }
        state.error = null;
      })
      .addCase(updatePanicAlertStatus.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || 'Failed to update panic alert status';
      });

    // Delete Panic Alert
    builder
      .addCase(deletePanicAlert.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(deletePanicAlert.fulfilled, (state, action: PayloadAction<number>) => {
        state.loading = false;
        state.alerts = state.alerts.filter(alert => alert.id !== action.payload);
        if (state.currentAlert?.id === action.payload) {
          state.currentAlert = null;
        }
        state.error = null;
      })
      .addCase(deletePanicAlert.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload || 'Failed to delete panic alert';
      });
  },
});

export const { clearError, clearSuccess, resetPanicAlert } = panicAlertSlice.actions;
export default panicAlertSlice.reducer;

