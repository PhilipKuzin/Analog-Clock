using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class TimeGetter : MonoBehaviour
{
    public event Action<int> OnCurrentTimeUpdated;

    private int _startTime;
    private float _timeIntervalForNextCheck = 3600f;
    
    public void UpdateTimeFromServer()
    {
        StartCoroutine(GetTimeFromMultipleServices(
            "https://timeapi.io/api/Time/current/zone?timeZone=Europe/Moscow",
            "https://worldtimeapi.org/api/timezone/Europe/Moscow"));

        InvokeRepeating(nameof(UpdateTimeFromServer), _timeIntervalForNextCheck, _timeIntervalForNextCheck);
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
        OnCurrentTimeUpdated?.Invoke(_startTime);
    }
}
