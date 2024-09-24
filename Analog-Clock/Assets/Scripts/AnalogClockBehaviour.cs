using System;
using UnityEngine;

public class AnalogClockBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _secondsHand;
    [SerializeField] private GameObject _minutesHand;
    [SerializeField] private GameObject _hoursHand;

    public event Action OnClockStarted;
    public event Action<int> OnTimeUpdated;
    public event Action<int> OnHandsMovedToTime;

    private int _currentTime;
    private int _startTime;

    private float _secondsMultiplier = 1f;  // change to speed check

    private bool _isClockEnable = true;

    private void Start()
    {
        OnClockStarted?.Invoke(); 
    }

    private void Update()
    {
        if (!_isClockEnable)
            return;

        UpdateClockHands();
    }

    public void UpdateCurrentTime(int time)
    {
        _startTime = time;
        UpdateClockHands();
    }

    public void DisableTheClock()
    {
        _isClockEnable = false;
    }

    public void EnableTheClock()
    {
        _isClockEnable = true;

        OnClockStarted?.Invoke();
    }

    public void GetCurrentHandsData()
    {
        float secondsAngle = _secondsHand.transform.localRotation.eulerAngles.y;
        float minutesAngle = _minutesHand.transform.localRotation.eulerAngles.y;
        float hoursAngle = _hoursHand.transform.localRotation.eulerAngles.y;

        secondsAngle = NormalizeAngle(secondsAngle);
        minutesAngle = NormalizeAngle(minutesAngle);
        hoursAngle = NormalizeAngle(hoursAngle);

        int currentSeconds = Mathf.RoundToInt(secondsAngle / 360 * 60);
        int currentMinutes = Mathf.RoundToInt(minutesAngle / 360 * 60);
        int currentHours = Mathf.RoundToInt(hoursAngle / 360 * 12);

        if (_currentTime >= 43200)
            currentHours += 12;

        currentHours = currentHours % 24;
        int resultTime = currentHours * 3600 + currentMinutes * 60 + currentSeconds;

        EnableTheClock();

        OnHandsMovedToTime?.Invoke(resultTime);
    }

    private void UpdateClockHands()
    {
        _currentTime = Mathf.RoundToInt(Time.time * _secondsMultiplier) + _startTime;
        _secondsHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 60, 0);
        _minutesHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 3600, 0);
        _hoursHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 43200, 0);

        OnTimeUpdated?.Invoke(_currentTime);
    }

    private float NormalizeAngle(float angle)
    {
        if (angle < 0)
            return angle + 360;
        if (angle >= 360)
            return angle - 360;
        return angle;
    }
}