import React from 'react';
import { SafeAreaProvider } from 'react-native-safe-area-context';
import { Provider } from 'react-redux';
import { NavigationContainer } from '@react-navigation/native';
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs';
import { Ionicons } from '@expo/vector-icons';
import { store } from './src/store';
import PanicAlertScreen from './src/screens/PanicAlertScreen';
import SOSScreen from './src/screens/SOSScreen';

const Tab = createBottomTabNavigator();

export default function App() {
  return (
    <Provider store={store}>
      <SafeAreaProvider>
        <NavigationContainer>
          <Tab.Navigator
            screenOptions={({ route }) => ({
              tabBarIcon: ({ focused, color, size }) => {
                let iconName: keyof typeof Ionicons.glyphMap;

                if (route.name === 'Panic Alert') {
                  iconName = focused ? 'alert-circle' : 'alert-circle-outline';
                } else if (route.name === 'SOS') {
                  iconName = focused ? 'help-buoy' : 'help-buoy-outline';
                } else {
                  iconName = 'alert-circle-outline';
                }

                return <Ionicons name={iconName} size={size} color={color} />;
              },
              tabBarActiveTintColor: '#DC2626',
              tabBarInactiveTintColor: '#64748B',
              headerStyle: {
                backgroundColor: '#DC2626',
              },
              headerTintColor: '#FFFFFF',
              headerTitleStyle: {
                fontWeight: 'bold',
              },
            })}
          >
            <Tab.Screen 
              name="Panic Alert" 
              component={PanicAlertScreen}
              options={{
                title: 'SafeSphere - Panic Alert',
              }}
            />
            <Tab.Screen 
              name="SOS" 
              component={SOSScreen}
              options={{
                title: 'SafeSphere - SOS',
              }}
            />
          </Tab.Navigator>
        </NavigationContainer>
      </SafeAreaProvider>
    </Provider>
  );
}

