using System;
using TMPro;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField] private TMP_InputField _alarmField;

    public event Action OnAlarmTriggered;
    public event Action OnAlarmSettingByHands;
    public event Action OnAlarmConfirmedByHands;
    public bool IsAlarmSet => _alarmTimeInSeconds > 0;

    private int _alarmTimeInSeconds = -1;

    public void ConfirmAlarmByTextInput ()
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

            Debug.Log($"alarm set on {alarmTimeSpan} by text input");
        }
    }

    public void ConfirmAlarmByHandsMoving ()
    {
        if (IsAlarmSet)
        {
            Debug.Log("alarm is alredy exist, wait the trigger");
            return;
        }

        OnAlarmConfirmedByHands?.Invoke();
    }

    public void SetAlarmTimeInSeconds(int time)
    {
        _alarmTimeInSeconds = time;
        TimeSpan timeSpans = TimeSpan.FromSeconds(time);

        Debug.Log($"alarm set on {timeSpans} by hands moving");
    }

    public void SetAlarmByHandsMoving()
    {
        OnAlarmSettingByHands?.Invoke();
    }

    public void CheckOnTrigger(int time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        Debug.Log($"current time {timeSpan}");

        if (time == _alarmTimeInSeconds)
            TriggerAlarm();
    }
    public void TriggerAlarm()
    {
        _alarmTimeInSeconds = -1;
        Debug.Log("TRRR-TRRR-TRRR-TRRR :)");
    }
}
