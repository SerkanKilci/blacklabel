import { useEffect, useRef } from 'react';
import { useTranslation } from 'react-i18next';
import { Animated, StyleSheet, Text, View } from 'react-native';
import Svg, { Circle } from 'react-native-svg';

import { getScoreColor } from '../utils/score';

const AnimatedCircle = Animated.createAnimatedComponent(Circle);

interface ScoreRingProps {
  score: number | null;
  size?: number;
  strokeWidth?: number;
}

export function ScoreRing({ score, size = 140, strokeWidth = 12 }: ScoreRingProps) {
  const { t } = useTranslation();
  const animatedValue = useRef(new Animated.Value(0)).current;
  const radius = (size - strokeWidth) / 2;
  const circumference = 2 * Math.PI * radius;
  const color = getScoreColor(score);

  useEffect(() => {
    animatedValue.setValue(0);
    Animated.timing(animatedValue, {
      toValue: score ?? 0,
      duration: 800,
      useNativeDriver: false,
    }).start();
  }, [score, animatedValue]);

  const strokeDashoffset = animatedValue.interpolate({
    inputRange: [0, 100],
    outputRange: [circumference, 0],
  });

  return (
    <View style={{ width: size, height: size }}>
      <Svg width={size} height={size}>
        <Circle cx={size / 2} cy={size / 2} r={radius} stroke="#E0E0E0" strokeWidth={strokeWidth} fill="none" />
        <AnimatedCircle
          cx={size / 2}
          cy={size / 2}
          r={radius}
          stroke={color}
          strokeWidth={strokeWidth}
          fill="none"
          strokeDasharray={circumference}
          strokeDashoffset={strokeDashoffset}
          strokeLinecap="round"
          rotation="-90"
          origin={`${size / 2}, ${size / 2}`}
        />
      </Svg>
      <View style={styles.centerLabel}>
        <Text style={[styles.scoreText, { color }]}>{score ?? '–'}</Text>
        {score === null && <Text style={styles.unavailableText}>{t('result.scoreUnavailable')}</Text>}
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  centerLabel: {
    position: 'absolute',
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    alignItems: 'center',
    justifyContent: 'center',
  },
  scoreText: {
    fontSize: 36,
    fontWeight: '700',
  },
  unavailableText: {
    fontSize: 10,
    color: '#757575',
    marginTop: 4,
    textAlign: 'center',
    maxWidth: 80,
  },
});
