using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private AnalogClockBehaviour _analogClock;
    [SerializeField] private DigitalClockBehaviour _digitalClock;

    private void Awake()
    {
        ClockMediator mediator = new ClockMediator(_analogClock, _digitalClock);
    }
}
