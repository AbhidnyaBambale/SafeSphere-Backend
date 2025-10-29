import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  ActivityIndicator,
  Alert,
  TextInput,
} from 'react-native';
import * as Location from 'expo-location';
import * as Haptics from 'expo-haptics';
import { Audio } from 'expo-av';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { createPanicAlert, clearError, clearSuccess, resetPanicAlert } from '../store/slices/panicAlertSlice';
import { Ionicons } from '@expo/vector-icons';

/**
 * Panic Alert Screen
 * Allows users to send emergency panic alerts with location
 */

const PanicAlertScreen: React.FC = () => {
  const dispatch = useAppDispatch();
  const { loading, error, success, currentAlert } = useAppSelector((state) => state.panicAlert);
  
  const [location, setLocation] = useState<Location.LocationObject | null>(null);
  const [locationLoading, setLocationLoading] = useState(false);
  const [additionalInfo, setAdditionalInfo] = useState('');
  const [sound, setSound] = useState<Audio.Sound | null>(null);

  // Temporary user ID (replace with actual user ID from auth)
  const userId = 1; // TODO: Get from authentication context

  useEffect(() => {
    requestLocationPermission();
    return () => {
      // Cleanup
      if (sound) {
        sound.unloadAsync();
      }
    };
  }, []);

  useEffect(() => {
    if (success) {
      showSuccessAlert();
      playAlertSound();
      triggerHapticFeedback();
      
      // Reset after 3 seconds
      setTimeout(() => {
        dispatch(clearSuccess());
        dispatch(resetPanicAlert());
        setAdditionalInfo('');
      }, 3000);
    }
  }, [success]);

  useEffect(() => {
    if (error) {
      Alert.alert('Error', error, [
        { text: 'OK', onPress: () => dispatch(clearError()) }
      ]);
    }
  }, [error]);

  const requestLocationPermission = async () => {
    try {
      const { status } = await Location.requestForegroundPermissionsAsync();
      if (status !== 'granted') {
        Alert.alert(
          'Permission Denied',
          'Location permission is required to send panic alerts.'
        );
        return;
      }
      getCurrentLocation();
    } catch (error) {
      console.error('Error requesting location permission:', error);
    }
  };

  const getCurrentLocation = async () => {
    try {
      setLocationLoading(true);
      const currentLocation = await Location.getCurrentPositionAsync({
        accuracy: Location.Accuracy.High,
      });
      setLocation(currentLocation);
      console.log('Current location:', currentLocation.coords);
    } catch (error) {
      console.error('Error getting location:', error);
      Alert.alert('Location Error', 'Unable to get current location. Please try again.');
    } finally {
      setLocationLoading(false);
    }
  };

  const playAlertSound = async () => {
    try {
      const { sound: alertSound } = await Audio.Sound.createAsync(
        require('../../assets/alert-sound.mp3'),
        { shouldPlay: true }
      );
      setSound(alertSound);
      await alertSound.playAsync();
    } catch (error) {
      console.error('Error playing sound:', error);
      // Sound is optional, don't show error to user
    }
  };

  const triggerHapticFeedback = async () => {
    try {
      await Haptics.notificationAsync(Haptics.NotificationFeedbackType.Success);
      
      // Triple vibration pattern
      setTimeout(() => Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Heavy), 200);
      setTimeout(() => Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Heavy), 400);
      setTimeout(() => Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Heavy), 600);
    } catch (error) {
      console.error('Error triggering haptic feedback:', error);
    }
  };

  const showSuccessAlert = () => {
    Alert.alert(
      '✅ Panic Alert Sent!',
      `Your emergency alert has been sent successfully.\n\nAlert ID: ${currentAlert?.id}\nLocation: ${location?.coords.latitude.toFixed(4)}, ${location?.coords.longitude.toFixed(4)}\n\nEmergency contacts have been notified.`,
      [{ text: 'OK' }]
    );
  };

  const handleSendPanicAlert = async () => {
    if (!location) {
      Alert.alert('No Location', 'Please wait while we get your location...');
      getCurrentLocation();
      return;
    }

    // Confirm before sending
    Alert.alert(
      'Send Panic Alert?',
      'This will immediately notify your emergency contacts and send your location.',
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Send Alert',
          style: 'destructive',
          onPress: async () => {
            try {
              await Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Medium);
              
              const alertData = {
                locationLat: location.coords.latitude,
                locationLng: location.coords.longitude,
                additionalInfo: additionalInfo || 'Emergency panic alert triggered',
              };

              console.log('Sending panic alert:', alertData);
              await dispatch(createPanicAlert({ userId, alertData })).unwrap();
            } catch (err) {
              console.error('Failed to send panic alert:', err);
            }
          },
        },
      ]
    );
  };

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Ionicons name="alert-circle" size={80} color="#DC2626" />
        <Text style={styles.title}>Panic Alert</Text>
        <Text style={styles.subtitle}>
          Press the button below to send an emergency alert to your contacts
        </Text>
      </View>

      {/* Location Status */}
      <View style={styles.locationContainer}>
        <View style={styles.locationHeader}>
          <Ionicons name="location" size={24} color="#059669" />
          <Text style={styles.locationTitle}>Current Location</Text>
        </View>
        {locationLoading ? (
          <ActivityIndicator size="small" color="#059669" />
        ) : location ? (
          <View>
            <Text style={styles.locationText}>
              📍 Lat: {location.coords.latitude.toFixed(6)}
            </Text>
            <Text style={styles.locationText}>
              📍 Lng: {location.coords.longitude.toFixed(6)}
            </Text>
            <Text style={styles.locationAccuracy}>
              Accuracy: ±{location.coords.accuracy?.toFixed(0)}m
            </Text>
          </View>
        ) : (
          <TouchableOpacity onPress={getCurrentLocation}>
            <Text style={styles.refreshButton}>Tap to get location</Text>
          </TouchableOpacity>
        )}
      </View>

      {/* Additional Info Input */}
      <View style={styles.inputContainer}>
        <Text style={styles.inputLabel}>Additional Information (Optional)</Text>
        <TextInput
          style={styles.input}
          placeholder="Describe the emergency situation..."
          value={additionalInfo}
          onChangeText={setAdditionalInfo}
          multiline
          numberOfLines={3}
          maxLength={500}
        />
      </View>

      {/* Success Message */}
      {success && (
        <View style={styles.successContainer}>
          <Ionicons name="checkmark-circle" size={24} color="#059669" />
          <Text style={styles.successText}>Alert sent successfully!</Text>
        </View>
      )}

      {/* Error Message */}
      {error && (
        <View style={styles.errorContainer}>
          <Ionicons name="close-circle" size={24} color="#DC2626" />
          <Text style={styles.errorText}>{error}</Text>
        </View>
      )}

      {/* Panic Button */}
      <TouchableOpacity
        style={[styles.panicButton, (loading || !location) && styles.panicButtonDisabled]}
        onPress={handleSendPanicAlert}
        disabled={loading || !location}
        activeOpacity={0.8}
      >
        {loading ? (
          <ActivityIndicator size="large" color="#FFFFFF" />
        ) : (
          <>
            <Ionicons name="alert-circle-outline" size={48} color="#FFFFFF" />
            <Text style={styles.panicButtonText}>SEND PANIC ALERT</Text>
          </>
        )}
      </TouchableOpacity>

      {/* Info Box */}
      <View style={styles.infoBox}>
        <Ionicons name="information-circle-outline" size={20} color="#64748B" />
        <Text style={styles.infoText}>
          Your location and emergency details will be sent to all your emergency contacts
        </Text>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#F8FAFC',
    padding: 20,
  },
  header: {
    alignItems: 'center',
    marginTop: 40,
    marginBottom: 30,
  },
  title: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#DC2626',
    marginTop: 16,
  },
  subtitle: {
    fontSize: 16,
    color: '#64748B',
    textAlign: 'center',
    marginTop: 8,
    paddingHorizontal: 20,
  },
  locationContainer: {
    backgroundColor: '#FFFFFF',
    borderRadius: 16,
    padding: 20,
    marginBottom: 20,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 8,
    elevation: 3,
  },
  locationHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 12,
  },
  locationTitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#1E293B',
    marginLeft: 8,
  },
  locationText: {
    fontSize: 14,
    color: '#475569',
    marginVertical: 2,
    fontFamily: 'monospace',
  },
  locationAccuracy: {
    fontSize: 12,
    color: '#94A3B8',
    marginTop: 4,
  },
  refreshButton: {
    color: '#3B82F6',
    fontSize: 14,
    fontWeight: '600',
  },
  inputContainer: {
    marginBottom: 20,
  },
  inputLabel: {
    fontSize: 14,
    fontWeight: '600',
    color: '#1E293B',
    marginBottom: 8,
  },
  input: {
    backgroundColor: '#FFFFFF',
    borderRadius: 12,
    padding: 16,
    fontSize: 14,
    color: '#1E293B',
    borderWidth: 1,
    borderColor: '#E2E8F0',
    minHeight: 80,
    textAlignVertical: 'top',
  },
  successContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#D1FAE5',
    padding: 16,
    borderRadius: 12,
    marginBottom: 20,
  },
  successText: {
    color: '#059669',
    fontSize: 16,
    fontWeight: '600',
    marginLeft: 12,
  },
  errorContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#FEE2E2',
    padding: 16,
    borderRadius: 12,
    marginBottom: 20,
  },
  errorText: {
    color: '#DC2626',
    fontSize: 14,
    marginLeft: 12,
    flex: 1,
  },
  panicButton: {
    backgroundColor: '#DC2626',
    borderRadius: 20,
    padding: 24,
    alignItems: 'center',
    justifyContent: 'center',
    shadowColor: '#DC2626',
    shadowOffset: { width: 0, height: 8 },
    shadowOpacity: 0.3,
    shadowRadius: 16,
    elevation: 8,
    marginBottom: 20,
    minHeight: 120,
  },
  panicButtonDisabled: {
    backgroundColor: '#94A3B8',
    shadowOpacity: 0.1,
  },
  panicButtonText: {
    color: '#FFFFFF',
    fontSize: 20,
    fontWeight: 'bold',
    marginTop: 12,
    letterSpacing: 1,
  },
  infoBox: {
    flexDirection: 'row',
    backgroundColor: '#F1F5F9',
    padding: 16,
    borderRadius: 12,
    alignItems: 'center',
  },
  infoText: {
    flex: 1,
    fontSize: 12,
    color: '#64748B',
    marginLeft: 12,
    lineHeight: 18,
  },
});

export default PanicAlertScreen;

