export type ScoreBand = 'good' | 'medium' | 'poor' | 'bad';

const BAND_COLORS: Record<ScoreBand, string> = {
  good: '#2E7D32',
  medium: '#F9A825',
  poor: '#EF6C00',
  bad: '#C62828',
};

const UNAVAILABLE_COLOR = '#9E9E9E';

export function getScoreBand(score: number): ScoreBand {
  if (score >= 75) return 'good';
  if (score >= 50) return 'medium';
  if (score >= 25) return 'poor';
  return 'bad';
}

export function getScoreColor(score: number | null): string {
  return score === null ? UNAVAILABLE_COLOR : BAND_COLORS[getScoreBand(score)];
}

const COMPARISON_LEVEL_COLORS: Record<'Good' | 'Medium' | 'Bad', string> = {
  Good: BAND_COLORS.good,
  Medium: BAND_COLORS.medium,
  Bad: BAND_COLORS.poor,
};

export function getComparisonLevelColor(level: 'Good' | 'Medium' | 'Bad' | null): string {
  return level === null ? UNAVAILABLE_COLOR : COMPARISON_LEVEL_COLORS[level];
}

export function getRiskLevelColor(riskLevel: number): string {
  switch (riskLevel) {
    case 1:
      return BAND_COLORS.medium;
    case 2:
      return BAND_COLORS.poor;
    case 3:
      return BAND_COLORS.bad;
    default:
      return BAND_COLORS.good;
  }
}
