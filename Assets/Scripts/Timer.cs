using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float startTime = 120f;
    float currentTime = 0f;
    bool timerIsRunning = false;

    [SerializeField] TMP_Text timerText;
    Action timerEndedMethod;




    public void StartTimer(float seconds)
    {
        gameObject.SetActive(true);
        startTime = seconds;
        currentTime = startTime;
        timerIsRunning = true;
    }
    
    public void StartTimer(float seconds, Action finishAction)
    {
        gameObject.SetActive(true);
        startTime = seconds;
        currentTime = startTime;
        timerIsRunning = true;
        timerEndedMethod = finishAction;
    }

    public void PauseTimer()
    {
        if (timerIsRunning) timerIsRunning = false;
    }

    public void ResumeTimer()
    {
        if (!timerIsRunning) timerIsRunning = true;
    }

    public void FinishTimer()
    {
        currentTime = -1;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                int minutes = (int)(currentTime / 60);
                int seconds = (int)(currentTime % 60);
                timerText.text = $"{minutes}:{seconds}";
            }
            else
            {
                timerIsRunning = false;
                currentTime = 0f;
                gameObject.SetActive(false);
                timerEndedMethod?.Invoke();
            }
        }  
    }
}
