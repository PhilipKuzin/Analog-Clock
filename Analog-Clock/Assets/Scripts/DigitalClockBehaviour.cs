using System;
using TMPro;
using UnityEngine;

public class DigitalClockBehaviour : MonoBehaviour
{
    private TMP_Text _clockView;

    private void Start()
    {
        _clockView = GetComponentInChildren<TMP_Text>();
    }
    public void UpdateClock(int currentTime)
    {
        int seconds = (currentTime % 60);
        int minutes = (currentTime / 60) % 60;
        int hours = (currentTime / 3600) % 24;

        // Форматируем время как HH:MM:SS
        _clockView.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

}
