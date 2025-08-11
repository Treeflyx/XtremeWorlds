namespace Core.Common;

public sealed class RandomUtility
{
    private readonly Random _random = new();
    private readonly Lock _lock = new();

    public double NextDouble(double minValue, double maxValue)
    {
        if (double.IsNaN(minValue) || double.IsNaN(maxValue))
        {
            throw new ArgumentException("Values cannot be NaN");
        }

        if (minValue > maxValue)
        {
            throw new ArgumentException("minValue must be less than or equal to maxValue");
        }

        lock (_lock)
        {
            return minValue + _random.NextDouble() * (maxValue - minValue);
        }
    }
        
    public int NextInt(int minValue, int maxValue)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentException("minValue must be less than or equal to maxValue");
        }

        lock (_lock)
        {
            return _random.Next(minValue, maxValue + 1);
        }
    }
}