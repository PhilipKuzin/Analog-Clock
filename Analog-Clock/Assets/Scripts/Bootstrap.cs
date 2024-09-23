using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private AnalogClockBehaviour _analogClock;
    [SerializeField] private DigitalClockBehaviour _digitalClock;

    private void Awake()
    {
        ClocksMediator mediator = new ClocksMediator(_analogClock, _digitalClock);
    }
}
