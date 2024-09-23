using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private AnalogClockBehaviour _analogClock;
    [SerializeField] private DigitalClockBehaviour _digitalClock;
    [SerializeField] private TimeGetter _timeGetter;

    private void Awake()
    {
        ClocksMediator mediator = new ClocksMediator(_analogClock, _digitalClock);
        TimeGetterMediator getterMediator = new TimeGetterMediator(_timeGetter, _analogClock);
    }
}
