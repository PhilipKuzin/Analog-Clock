using System;
using TMPro;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    public event Action OnAlarmTriggered;
    //public event Action <int> OnAlarmSet;
    public bool IsAlarmSet => _alarmTimeInSeconds > 0;

    private TMP_InputField _alarmField;

    private int _alarmTimeInSeconds = -1;

    private void Start()
    {
        _alarmField = GetComponentInChildren<TMP_InputField>();
    }

    public void SetAlarm ()
    {
        if (IsAlarmSet)
        {
            Debug.Log("alarm is alredy exist, wait the trigger");
            return;
        }

        string inputTime = _alarmField.text;

        if(TimeSpan.TryParse(inputTime, out TimeSpan alarmTimeSpan))
        {
            _alarmTimeInSeconds = (int)alarmTimeSpan.TotalSeconds;
            _alarmField.text = string.Empty;

            //OnAlarmSet?.Invoke(_alarmTimeInSeconds);

            Debug.Log($"alarm set on {alarmTimeSpan}");

        }
    }

    public void CheckOnTrigger(int time)
    {
        if (time == _alarmTimeInSeconds)
            TriggerAlarm();
    }

    public void TriggerAlarm()
    {
        _alarmTimeInSeconds = -1;
        Debug.Log("TRRR-TRRR-TRRR-TRRR :)");
    }
    
}
