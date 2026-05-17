namespace ChangeMind.Domain.Services;

using ChangeMind.Domain.Enums;

/// <summary>
/// Computes body-fat percentage using the US Navy circumference method.
/// Returns null if required inputs are missing or the result is outside a reasonable range.
/// All lengths are in centimeters.
/// </summary>
public static class BodyFatCalculator
{
    public static decimal? CalculateUsNavy(
        Gender? gender,
        decimal? heightCm,
        decimal? waistCm,
        decimal? neckCm,
        decimal? hipCm)
    {
        if (gender is null || heightCm is null || waistCm is null || neckCm is null)
            return null;
        if (heightCm <= 0)
            return null;

        var h = (double)heightCm.Value;
        var w = (double)waistCm.Value;
        var n = (double)neckCm.Value;
        double bf;

        if (gender == Gender.Male)
        {
            var diff = w - n;
            if (diff <= 0) return null;
            bf = 86.010 * Math.Log10(diff) - 70.041 * Math.Log10(h) + 36.76;
        }
        else
        {
            if (hipCm is null) return null;
            var sum = w + (double)hipCm.Value - n;
            if (sum <= 0) return null;
            bf = 163.205 * Math.Log10(sum) - 97.684 * Math.Log10(h) - 78.387;
        }

        if (double.IsNaN(bf) || double.IsInfinity(bf)) return null;
        if (bf is < 2 or > 60) return null;

        return Math.Round((decimal)bf, 2);
    }
}
