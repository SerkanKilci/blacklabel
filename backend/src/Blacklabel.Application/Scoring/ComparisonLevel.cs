namespace Blacklabel.Application.Scoring;

/// <summary>
/// A 3-band classification (used for side-by-side product comparison) derived from the exact
/// same ScoreThresholds/per-additive-risk values the overall score is computed from — so a
/// product labelled "Good" here can never contradict what actually drove its score up or down.
/// </summary>
public enum ComparisonLevel
{
    Good,
    Medium,
    Bad
}
