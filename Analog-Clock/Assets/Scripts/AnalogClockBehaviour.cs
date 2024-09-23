using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AnalogClockBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _secondsHand;
    [SerializeField] private GameObject _minutesHand;
    [SerializeField] private GameObject _hoursHand;

    public event Action<int> OnTimeUpdated;

    private int _currentTime;
    private int _startTime;
    private float _secondsMultiplier = 1f;
    private float _timeIntervalForNextCheck = 3600f;

    private void Start()
    {
        UpdateTimeFromServer();

        InvokeRepeating(nameof(UpdateTimeFromServer), _timeIntervalForNextCheck, _timeIntervalForNextCheck);
    }

    private void Update()
    {
        UpdateClockHands();
    }

    private void UpdateTimeFromServer()
    {
        StartCoroutine(GetTimeFromMultipleServices(
            "https://timeapi.io/api/Time/current/zone?timeZone=Europe/Amsterdam",
            "https://worldtimeapi.org/api/timezone/Europe/Amsterdam"));
    }

    private void UpdateClockHands()
    {
        _currentTime = Mathf.RoundToInt(Time.time * _secondsMultiplier) + _startTime;
        _secondsHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 60, 0);
        _minutesHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 3600, 0);
        _hoursHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 43200, 0);

        OnTimeUpdated?.Invoke(_currentTime);
    }

    private IEnumerator GetTimeFromMultipleServices(string first, string second)
    {
        yield return StartCoroutine(GetTimeFromService(first));

        if (_startTime == 0)
            yield return StartCoroutine(GetTimeFromService(second));

        if (_startTime == 0)
            Debug.LogError("error of getting time from both services");
    }

    private IEnumerator GetTimeFromService(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                Debug.LogError($"error extracting time from {url}: {webRequest.error}");
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log($"received response from {url}: {jsonResponse}"); 

                TimeResponse timeResponse = JsonUtility.FromJson<TimeResponse>(jsonResponse);
                
                if (timeResponse != null && !string.IsNullOrEmpty(timeResponse.dateTime))
                    UpdateCurrentTime(timeResponse.dateTime);
                else
                    Debug.LogError($"error invalid JSON response");
            }
        }
    }

    private void UpdateCurrentTime(string datetime)
    {
        DateTime dateTime = DateTime.Parse(datetime);
        _startTime = (int)(dateTime.TimeOfDay.TotalSeconds);
        Debug.Log("time is updated");
        UpdateClockHands();
    }
}