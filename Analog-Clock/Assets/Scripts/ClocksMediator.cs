public class ClocksMediator 
{
    private AnalogClockBehaviour _analogClock;
    private DigitalClockBehaviour _digitalClock;

    public ClocksMediator(AnalogClockBehaviour analogClock, DigitalClockBehaviour digitalClock)
    {
        _analogClock = analogClock;
        _digitalClock = digitalClock;

        _analogClock.OnTimeUpdated += UpdateDigitalClock;
    }

    private void UpdateDigitalClock(int currentTime)
    {
        _digitalClock.UpdateClock(currentTime);
    }
}
