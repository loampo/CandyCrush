using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60f;
    public TextMeshProUGUI timeText;
    public GameObject TimeUpMenu;
    private void OnEnable()
    {
        timeRemaining = 60f;
        Time.timeScale = 1;
    }
    private void Start()
    {
        timeText.text = timeRemaining.ToString();
    }

    private void Update()
    {
            
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                timeRemaining = 0;
                UpdateTimerText();
                TimeUp();
            }
    }

    private void TimeUp()
    {
        Time.timeScale = 0;
        TimeUpMenu.SetActive(true);
    }

    // Aggiorna il timer con il tempo rimanente
    private void UpdateTimerText()
    {
        //int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timeText.text = string.Format("{0:00}", seconds);
    }
}
