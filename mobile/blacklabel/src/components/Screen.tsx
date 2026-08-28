import type { ReactNode } from 'react';
import type { StyleProp, ViewStyle } from 'react-native';
import { SafeAreaView, type Edge } from 'react-native-safe-area-context';

interface ScreenProps {
  children: ReactNode;
  edges?: Edge[];
  style?: StyleProp<ViewStyle>;
}

/** Shared safe-area wrapper so every screen insets against the notch/Dynamic Island and
 * home-indicator/gesture-bar consistently, instead of each screen guessing its own hardcoded
 * top padding (which under- or over-pads depending on device and never adapts on rotation). */
export function Screen({ children, edges = ['top', 'bottom'], style }: ScreenProps) {
  return (
    <SafeAreaView edges={edges} style={[{ flex: 1 }, style]}>
      {children}
    </SafeAreaView>
  );
}
