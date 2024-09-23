public class TimeGetterMediator 
{
    private TimeGetter _timeGetter;
    private AnalogClockBehaviour _analogClockBehaviour;

    public TimeGetterMediator(TimeGetter timeGetter, AnalogClockBehaviour analogClockBehaviour)
    {
        _timeGetter = timeGetter;
        _analogClockBehaviour = analogClockBehaviour;

        _timeGetter.OnCurrentTimeUpdated += UpdateHandsPosition;
        _analogClockBehaviour.OnClockStarted += UpdateTimeInGetter;
    }

    private void UpdateTimeInGetter()
    {
        _timeGetter.UpdateTimeFromServer();
    }

    private void UpdateHandsPosition(int time)
    {
        _analogClockBehaviour.UpdateCurrentTime(time);
    }
}
