using System;
using UnityEngine;

public class AnalogClockBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _secondsHand;
    [SerializeField] private GameObject _minutesHand;
    [SerializeField] private GameObject _hoursHand;

    public event Action <int> OnTimeUpdated;

    private float _secondsMultiplier = 1f;
    private int _currentTime;
    private int _startTime = 10000;

    private void Update()
    {
        _currentTime = Mathf.RoundToInt(Time.time * _secondsMultiplier) + _startTime;
        _secondsHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 60, 0);
        _minutesHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 3600, 0);
        _hoursHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 43200, 0);

        OnTimeUpdated?.Invoke(_currentTime);
    }
}
