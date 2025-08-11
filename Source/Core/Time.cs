using System.Timers;
using Timer = System.Timers.Timer;

namespace Core;

public enum TimeOfDay : byte
{
    None = 0,
    Day,
    Night,
    Dawn,
    Dusk
}

public delegate void HandleTimeEvent(Clock source);

public sealed class Clock
{
    private static Clock? _instance;
    private readonly Timer _timer;
    private DateTime _time;
    private double _gameSpeed;
    private TimeOfDay _timeOfDay;

    public event HandleTimeEvent? OnTimeChanged;
    public event HandleTimeEvent? OnTimeOfDayChanged;
    public event HandleTimeEvent? OnTimeSync;

    public static Clock Instance => _instance ??= new Clock();

    public DateTime Time
    {
        get => _time;
        set
        {
            _time = value;

            TimeOfDay = GetTimeOfDay(Time.Hour);

            OnTimeChanged?.Invoke(this);
        }
    }

    public double GameSpeed
    {
        get => _gameSpeed;
        set
        {
            _gameSpeed = value;

            OnTimeSync?.Invoke(this);
        }
    }

    public TimeOfDay TimeOfDay
    {
        get => _timeOfDay;
        set
        {
            _timeOfDay = value;

            OnTimeOfDayChanged?.Invoke(this);
        }
    }

    public Clock()
    {
        _timer = new Timer(6000);
        _timer.Elapsed += HandleTimerElapsed;
        _timer.Start();
    }

    private void HandleTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        OnTimeSync?.Invoke(this);
    }

    public override string ToString()
    {
        return ToString("h:mm:ss tt");
    }

    public string ToString(string format)
    {
        return Time.ToString(format);
    }

    public void Tick()
    {
        Time = Time.AddSeconds(GameSpeed);
    }

    public static TimeOfDay GetTimeOfDay(int hours)
    {
        return hours switch
        {
            < 6 => TimeOfDay.Night,
            <= 9 => TimeOfDay.Dawn,
            < 18 => TimeOfDay.Day,
            _ => TimeOfDay.Dusk
        };
    }
}