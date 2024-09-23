using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private AnalogClockBehaviour _analogClock;
    [SerializeField] private DigitalClockBehaviour _digitalClock;
    [SerializeField] private TimeGetter _timeGetter;
    [SerializeField] private Alarm _alarm;

    private void Awake()
    {
        ClocksMediator mediator = new ClocksMediator(_analogClock, _digitalClock);
        TimeGetterMediator getterMediator = new TimeGetterMediator(_timeGetter, _analogClock);
        AlarmMediator alarmMediator = new AlarmMediator(_alarm, _analogClock);
    }
}
