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
    private float secondsMultiplier = 1f;

    private void Start()
    {
        StartCoroutine(GetTimeFromService("https://timeapi.io/api/Time/current/zone?timeZone=Europe/Amsterdam"));
    }

    private void Update()
    {
        UpdateClockHands();
    }

    private void UpdateClockHands()
    {
        _currentTime = Mathf.RoundToInt(Time.time * secondsMultiplier) + _startTime;
        _secondsHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 60, 0);
        _minutesHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 3600, 0);
        _hoursHand.transform.localRotation = Quaternion.Euler(0, _currentTime * 360 / 43200, 0);

        OnTimeUpdated?.Invoke(_currentTime);
        //Debug.Log("часы обновились");
    }

    private IEnumerator GetTimeFromService(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching time: {webRequest.error}");
                Debug.LogError($"Response: {webRequest.downloadHandler.text}"); // Для отладки
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log($"Received response: {jsonResponse}"); // Для отладки
                TimeResponse timeResponse = JsonUtility.FromJson<TimeResponse>(jsonResponse);
                UpdateCurrentTime(timeResponse.dateTime);
            }
        }
    }

    private void UpdateCurrentTime(string datetime)
    {
        DateTime dateTime = DateTime.Parse(datetime);
        _startTime = (int)(dateTime.TimeOfDay.TotalSeconds);
        Debug.Log("часы обновились");
        UpdateClockHands();
    }
}

[Serializable]
public class TimeResponse
{
    public string dateTime;
}