using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool timerStarted = false;
    private float secondCounter = 0;
    private int timer = 0;


    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            secondCounter += Time.deltaTime;
            if(secondCounter >= 1.0f)
            {
                timer -= 1;
                secondCounter -= 1.0f;
                if(gameObject.name == "Managers") Actions.OnTimerChange(gameObject);
            }
            if(timer == 0)
            {
                Actions.OnTimerFinish(gameObject);
                timerStarted = false;
            }
        }
        else
        {
            secondCounter = 0;
        }
    }

    public void StartTimer(int seconds)
    {
        timer = seconds;
        secondCounter = 0;
        timerStarted = true;
    }
}
