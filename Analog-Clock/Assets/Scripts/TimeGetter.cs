using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeGetter : MonoBehaviour
{
    public event Action<int> OnCurrentTimeUpdated;

    private string[] _urls =
    {
       "https://timeapi.io/api/Time/current/zone?timeZone=Europe/Moscow",
       "https://worldtimeapi.org/api/timezone/Europe/Moscow"
    };

    private int _startTime;
    private float _timeIntervalForNextCheck = 3600f;
    private float _elapsedTime = 0f;

    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _timeIntervalForNextCheck)
        {
            _elapsedTime = 0f;
            UpdateTimeFromServer();
        }
    }

    public void UpdateTimeFromServer()
    {
        StartCoroutine(GetTimeFromMultipleServices());
    }

    private IEnumerator GetTimeFromMultipleServices()
    {
        for (int i = 0; i < _urls.Length; i++)
        {
            yield return StartCoroutine(GetTimeFromService(_urls[i]));

            if (_startTime != 0)
                yield break; 
        }
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
        OnCurrentTimeUpdated?.Invoke(_startTime);
    }
}
