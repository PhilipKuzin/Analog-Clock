public class AlarmMediator
{
    private Alarm _alarm;
    private AnalogClockBehaviour _analogClockBehaviour;
    public AlarmMediator(Alarm alarm, AnalogClockBehaviour analogClockBehaviour)
    {
        _alarm = alarm;
        _analogClockBehaviour = analogClockBehaviour;

        _analogClockBehaviour.OnTimeUpdated += TransferClockData;
        _analogClockBehaviour.OnHandsMovedToTime += DoTransferHandsData;

        _alarm.OnAlarmSettingByHands += DoSettingByHandsLogic;
        _alarm.OnAlarmConfirmedByHands += DoGetCurrentHndsDataLogic;
    }

    private void DoTransferHandsData(int time)
    {
        _alarm.SetAlarmTimeInSeconds(time);
    }

    private void DoGetCurrentHndsDataLogic()
    {
        _analogClockBehaviour.GetCurrentHandsData();
    }

    private void DoSettingByHandsLogic()
    {
        _analogClockBehaviour.DisableTheClock();
    }

    private void TransferClockData(int time)
    {
        _alarm.CheckOnTrigger(time);
    }
}
