import { useRouter } from 'expo-router';
import { Pressable, StyleSheet, Text, type StyleProp, type ViewStyle } from 'react-native';

interface BackButtonProps {
  style?: StyleProp<ViewStyle>;
}

/** Standalone back affordance for screens that don't otherwise have one — expo-router's Stack
 * runs with headerShown:false everywhere (see app/_layout.tsx), so there's no automatic native
 * back chevron; without this, a screen is only exitable via the OS swipe/hardware-back gesture,
 * which isn't discoverable and doesn't exist at all on web. */
export function BackButton({ style }: BackButtonProps) {
  const router = useRouter();
  return (
    <Pressable style={[styles.button, style]} onPress={() => router.back()} hitSlop={8}>
      <Text style={styles.arrow}>‹</Text>
    </Pressable>
  );
}

const styles = StyleSheet.create({
  button: {
    width: 36,
    height: 36,
    borderRadius: 18,
    backgroundColor: '#F5F5F5',
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: 12,
  },
  arrow: {
    fontSize: 22,
    fontWeight: '600',
    color: '#1A1A1A',
    marginTop: -2,
  },
});
