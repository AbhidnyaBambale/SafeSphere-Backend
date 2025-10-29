import { configureStore } from '@reduxjs/toolkit';
import panicAlertReducer from './slices/panicAlertSlice';
import sosAlertReducer from './slices/sosAlertSlice';

/**
 * Redux Store Configuration
 * Combines all reducers and middleware
 */

export const store = configureStore({
  reducer: {
    panicAlert: panicAlertReducer,
    sosAlert: sosAlertReducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        // Ignore these action types
        ignoredActions: ['panicAlert/create/fulfilled', 'sosAlert/create/fulfilled'],
      },
    }),
});

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

