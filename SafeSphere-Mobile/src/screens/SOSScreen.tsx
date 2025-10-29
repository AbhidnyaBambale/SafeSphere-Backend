import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  StyleSheet,
  TouchableOpacity,
  ActivityIndicator,
  Alert,
  TextInput,
  KeyboardAvoidingView,
  Platform,
  ScrollView,
} from 'react-native';
import * as Location from 'expo-location';
import * as Haptics from 'expo-haptics';
import { Audio } from 'expo-av';
import { useAppDispatch, useAppSelector } from '../store/hooks';
import { createSOSAlert, clearError, clearSuccess, resetSOSAlert } from '../store/slices/sosAlertSlice';
import { Ionicons } from '@expo/vector-icons';

/**
 * SOS Screen
 * Allows users to send SOS alerts with custom messages
 */

const SOSScreen: React.FC = () => {
  const dispatch = useAppDispatch();
  const { loading, error, success, currentAlert } = useAppSelector((state) => state.sosAlert);
  
  const [location, setLocation] = useState<Location.LocationObject | null>(null);
  const [locationLoading, setLocationLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [locationString, setLocationString] = useState('');
  const [sound, setSound] = useState<Audio.Sound | null>(null);

  // Temporary user ID (replace with actual user ID from auth)
  const userId = 1; // TODO: Get from authentication context

  // Pre-defined message templates
  const messageTemplates = [
    '🚨 Need immediate help!',
    '🚗 Car breakdown, need assistance',
    '🏥 Medical emergency',
    '👥 Feeling unsafe, please check on me',
    '📍 Lost and need directions',
  ];

  useEffect(() => {
    requestLocationPermission();
    return () => {
      if (sound) {
        sound.unloadAsync();
      }
    };
  }, []);

  useEffect(() => {
    if (success) {
      showSuccessAlert();
      playSuccessSound();
      triggerHapticFeedback();
      
      // Reset form after 3 seconds
      setTimeout(() => {
        dispatch(clearSuccess());
        dispatch(resetSOSAlert());
        setMessage('');
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
          'Location permission is required for SOS alerts.'
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
      
      // Try to get address from coordinates
      const address = await Location.reverseGeocodeAsync({
        latitude: currentLocation.coords.latitude,
        longitude: currentLocation.coords.longitude,
      });

      if (address && address.length > 0) {
        const addr = address[0];
        const locationStr = `${addr.street || ''} ${addr.city || ''} ${addr.region || ''}`.trim();
        setLocationString(locationStr || `${currentLocation.coords.latitude.toFixed(4)}, ${currentLocation.coords.longitude.toFixed(4)}`);
      } else {
        setLocationString(`${currentLocation.coords.latitude.toFixed(4)}, ${currentLocation.coords.longitude.toFixed(4)}`);
      }
    } catch (error) {
      console.error('Error getting location:', error);
      Alert.alert('Location Error', 'Unable to get current location.');
    } finally {
      setLocationLoading(false);
    }
  };

  const playSuccessSound = async () => {
    try {
      const { sound: successSound } = await Audio.Sound.createAsync(
        require('../../assets/success-sound.mp3'),
        { shouldPlay: true }
      );
      setSound(successSound);
      await successSound.playAsync();
    } catch (error) {
      console.error('Error playing sound:', error);
    }
  };

  const triggerHapticFeedback = async () => {
    try {
      await Haptics.notificationAsync(Haptics.NotificationFeedbackType.Success);
      setTimeout(() => Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Medium), 100);
    } catch (error) {
      console.error('Error triggering haptic feedback:', error);
    }
  };

  const showSuccessAlert = () => {
    Alert.alert(
      '✅ SOS Alert Sent Successfully!',
      `Your SOS message has been delivered.\n\nAlert ID: ${currentAlert?.id}\nMessage: "${currentAlert?.message}"\n\nEmergency contacts will respond soon.`,
      [{ text: 'OK' }]
    );
  };

  const handleSendSOS = async () => {
    if (!message.trim()) {
      Alert.alert('Message Required', 'Please enter an SOS message.');
      return;
    }

    if (!location) {
      Alert.alert('No Location', 'Please wait while we get your location...');
      getCurrentLocation();
      return;
    }

    // Confirm before sending
    Alert.alert(
      'Send SOS Alert?',
      `This will send the following message to your emergency contacts:\n\n"${message}"`,
      [
        { text: 'Cancel', style: 'cancel' },
        {
          text: 'Send SOS',
          style: 'destructive',
          onPress: async () => {
            try {
              await Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Medium);
              
              const alertData = {
                message: message.trim(),
                location: locationString || 'Location unavailable',
                locationLat: location.coords.latitude,
                locationLng: location.coords.longitude,
              };

              console.log('Sending SOS alert:', alertData);
              await dispatch(createSOSAlert({ userId, alertData })).unwrap();
            } catch (err) {
              console.error('Failed to send SOS alert:', err);
            }
          },
        },
      ]
    );
  };

  const selectTemplate = (template: string) => {
    setMessage(template);
    Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Light);
  };

  return (
    <KeyboardAvoidingView
      style={styles.container}
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
    >
      <ScrollView showsVerticalScrollIndicator={false}>
        <View style={styles.header}>
          <Ionicons name="help-buoy" size={80} color="#EF4444" />
          <Text style={styles.title}>SOS Alert</Text>
          <Text style={styles.subtitle}>
            Send a custom emergency message to your contacts
          </Text>
        </View>

        {/* Location Status */}
        <View style={styles.locationContainer}>
          <View style={styles.locationHeader}>
            <Ionicons name="location-outline" size={24} color="#3B82F6" />
            <Text style={styles.locationTitle}>Your Location</Text>
            {locationLoading && <ActivityIndicator size="small" color="#3B82F6" style={{ marginLeft: 8 }} />}
          </View>
          {location ? (
            <Text style={styles.locationText}>
              📍 {locationString}
            </Text>
          ) : (
            <TouchableOpacity onPress={getCurrentLocation}>
              <Text style={styles.refreshButton}>Tap to refresh location</Text>
            </TouchableOpacity>
          )}
        </View>

        {/* Message Templates */}
        <View style={styles.templatesContainer}>
          <Text style={styles.templatesTitle}>Quick Messages</Text>
          <View style={styles.templatesGrid}>
            {messageTemplates.map((template, index) => (
              <TouchableOpacity
                key={index}
                style={[
                  styles.templateButton,
                  message === template && styles.templateButtonActive
                ]}
                onPress={() => selectTemplate(template)}
              >
                <Text style={[
                  styles.templateText,
                  message === template && styles.templateTextActive
                ]}>
                  {template}
                </Text>
              </TouchableOpacity>
            ))}
          </View>
        </View>

        {/* Message Input */}
        <View style={styles.inputContainer}>
          <Text style={styles.inputLabel}>Your SOS Message *</Text>
          <TextInput
            style={styles.messageInput}
            placeholder="Describe your emergency situation..."
            value={message}
            onChangeText={setMessage}
            multiline
            numberOfLines={4}
            maxLength={500}
            placeholderTextColor="#94A3B8"
          />
          <Text style={styles.characterCount}>{message.length}/500</Text>
        </View>

        {/* Success Message */}
        {success && (
          <View style={styles.successContainer}>
            <Ionicons name="checkmark-circle" size={28} color="#10B981" />
            <Text style={styles.successText}>SOS sent successfully!</Text>
          </View>
        )}

        {/* Error Message */}
        {error && (
          <View style={styles.errorContainer}>
            <Ionicons name="close-circle" size={24} color="#EF4444" />
            <Text style={styles.errorText}>{error}</Text>
          </View>
        )}

        {/* Send Button */}
        <TouchableOpacity
          style={[
            styles.sosButton,
            (loading || !message.trim() || !location) && styles.sosButtonDisabled
          ]}
          onPress={handleSendSOS}
          disabled={loading || !message.trim() || !location}
          activeOpacity={0.8}
        >
          {loading ? (
            <ActivityIndicator size="large" color="#FFFFFF" />
          ) : (
            <>
              <Ionicons name="send" size={32} color="#FFFFFF" />
              <Text style={styles.sosButtonText}>SEND SOS ALERT</Text>
            </>
          )}
        </TouchableOpacity>

        {/* Info Box */}
        <View style={styles.infoBox}>
          <Ionicons name="shield-checkmark-outline" size={20} color="#64748B" />
          <Text style={styles.infoText}>
            Your message and location will be sent to all emergency contacts. They can respond immediately.
          </Text>
        </View>
      </ScrollView>
    </KeyboardAvoidingView>
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
    marginBottom: 24,
  },
  title: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#EF4444',
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
    padding: 16,
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
    marginBottom: 8,
  },
  locationTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1E293B',
    marginLeft: 8,
  },
  locationText: {
    fontSize: 14,
    color: '#475569',
    marginTop: 4,
  },
  refreshButton: {
    color: '#3B82F6',
    fontSize: 14,
    fontWeight: '600',
  },
  templatesContainer: {
    marginBottom: 20,
  },
  templatesTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#1E293B',
    marginBottom: 12,
  },
  templatesGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 8,
  },
  templateButton: {
    backgroundColor: '#F1F5F9',
    borderRadius: 12,
    paddingVertical: 10,
    paddingHorizontal: 16,
    marginRight: 8,
    marginBottom: 8,
    borderWidth: 2,
    borderColor: 'transparent',
  },
  templateButtonActive: {
    backgroundColor: '#DBEAFE',
    borderColor: '#3B82F6',
  },
  templateText: {
    fontSize: 13,
    color: '#475569',
    fontWeight: '500',
  },
  templateTextActive: {
    color: '#1E40AF',
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
  messageInput: {
    backgroundColor: '#FFFFFF',
    borderRadius: 12,
    padding: 16,
    fontSize: 16,
    color: '#1E293B',
    borderWidth: 2,
    borderColor: '#E2E8F0',
    minHeight: 120,
    textAlignVertical: 'top',
  },
  characterCount: {
    fontSize: 12,
    color: '#94A3B8',
    textAlign: 'right',
    marginTop: 4,
  },
  successContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: '#D1FAE5',
    padding: 16,
    borderRadius: 16,
    marginBottom: 20,
  },
  successText: {
    color: '#10B981',
    fontSize: 18,
    fontWeight: '700',
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
    color: '#EF4444',
    fontSize: 14,
    marginLeft: 12,
    flex: 1,
  },
  sosButton: {
    backgroundColor: '#EF4444',
    borderRadius: 20,
    padding: 20,
    alignItems: 'center',
    justifyContent: 'center',
    shadowColor: '#EF4444',
    shadowOffset: { width: 0, height: 8 },
    shadowOpacity: 0.3,
    shadowRadius: 16,
    elevation: 8,
    marginBottom: 20,
    minHeight: 100,
  },
  sosButtonDisabled: {
    backgroundColor: '#94A3B8',
    shadowOpacity: 0.1,
  },
  sosButtonText: {
    color: '#FFFFFF',
    fontSize: 18,
    fontWeight: 'bold',
    marginTop: 8,
    letterSpacing: 1,
  },
  infoBox: {
    flexDirection: 'row',
    backgroundColor: '#F1F5F9',
    padding: 16,
    borderRadius: 12,
    alignItems: 'center',
    marginBottom: 20,
  },
  infoText: {
    flex: 1,
    fontSize: 12,
    color: '#64748B',
    marginLeft: 12,
    lineHeight: 18,
  },
});

export default SOSScreen;

