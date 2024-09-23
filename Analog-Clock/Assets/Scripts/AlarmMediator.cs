using System;

public class AlarmMediator
{
    private Alarm _alarm;
    private AnalogClockBehaviour _analogClockBehaviour;
    public AlarmMediator(Alarm alarm, AnalogClockBehaviour analogClockBehaviour)
    {
        _alarm = alarm;
        _analogClockBehaviour = analogClockBehaviour;

        _analogClockBehaviour.OnTimeUpdated += TransferClockData;
    }

    private void TransferClockData(int time)
    {
        _alarm.CheckOnTrigger(time);
    }
}
