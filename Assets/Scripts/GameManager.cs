using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    private GhostController ghostController;
    private PacStudentController studentController;
    private CherryController cherryContoller;
    private Timer timer;

    private TMPro.TextMeshProUGUI title;

    private TMPro.TextMeshProUGUI scaredTimer;
    private TMPro.TextMeshProUGUI countdown;
    private TMPro.TextMeshProUGUI gameTimer;
    private GameObject gameOverScreen;
    private int scaredTimerVal;
    private int gameStartTimer;

    private double secondsTillStart;

    private MusicManager musicManager;

    private int score;
    private int lives;
    private bool ghostDead;
    private bool ghostsScared;
    private bool gameStarted;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    private void OnEnable()
    {
        scaredTimerVal = 10;
        gameStartTimer = 3;
        secondsTillStart = 0;
        score = 0;
        lives = 3;
        ghostDead = false;
        ghostsScared = false;
        gameStarted = false;
        title = GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
        scaredTimer = GameObject.Find("GhostScaredTimer").GetComponent<TMPro.TextMeshProUGUI>();
        ghostController = GameObject.Find("Ghosts").GetComponent<GhostController>();
        studentController = GameObject.Find("PacStudent").GetComponent<PacStudentController>();
        musicManager = GameObject.Find("BackgroundMusic").GetComponent<MusicManager>();
        timer = GetComponent<Timer>();
        countdown = GameObject.Find("Counter").GetComponent<TMPro.TextMeshProUGUI>();
        cherryContoller = GameObject.Find("CherryController").GetComponent<CherryController>();
        gameTimer = GameObject.Find("GameTimer").GetComponent<TMPro.TextMeshProUGUI>();
        gameOverScreen = GameObject.Find("GameOver");
        gameOverScreen.SetActive(false);
        StartGameTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            secondsTillStart += (double)Time.deltaTime;
            ComputeGameTimer();
            CheckGameOver();
        }
        
    }


    private void StartGameTimer()
    {
        countdown.text = gameStartTimer.ToString();
        timer.StartTimer(gameStartTimer + 1);
        Actions.OnTimerChange += ChangeGameStartTimer;
        Actions.OnTimerFinish += StartGame;
        
    }

    private void ChangeGameStartTimer(GameObject obj)
    {
        gameStartTimer -= 1;
        if (gameStartTimer > 0) countdown.text = gameStartTimer.ToString();
        else
        { 
            countdown.text = "GO!";
        }
    }

    private void StartGame(GameObject obj)
    {
        gameStartTimer = 3;
        Actions.OnTimerChange -= ChangeGameStartTimer;
        Actions.OnTimerFinish -= StartGame;

        GameObject countdownParent = GameObject.Find("Countdown");
        countdownParent.SetActive(false);

        studentController.enabled = true;
        cherryContoller.enabled = true;

        musicManager.PlayGame();

        gameStarted = true;
    }

    private void CheckGameOver()
    {
        bool pelletsEaten = GameObject.FindGameObjectsWithTag("Pellet").Length == 0;
        if(pelletsEaten || lives == 0)
        {
            if (score > PlayerPrefs.GetInt("score"))
            {
                PlayerPrefs.SetInt("score", score);
                PlayerPrefs.SetString("time", gameTimer.text.Substring(6));
            } else if(score == PlayerPrefs.GetInt("score"))
            {
                string currentTime = gameTimer.text.Substring(6);
                string prevTime = PlayerPrefs.GetString("time");
                int preMinutes = Convert.ToInt32(prevTime.Substring(0, 2));
                int preSeconds = Convert.ToInt32(prevTime.Substring(3, 2));
                int preMillies = Convert.ToInt32(prevTime.Substring(6, 2));

                int curMinutes = Convert.ToInt32(currentTime.Substring(0, 2));
                int curSeconds = Convert.ToInt32(currentTime.Substring(3, 2));
                int curMillies = Convert.ToInt32(currentTime.Substring(6, 2));

                if((preMinutes > curMinutes) || (preMinutes == curMinutes && preSeconds > curSeconds) ||
                    (preMinutes == curMinutes && preSeconds == curSeconds && preMillies > curMillies))
                {
                    PlayerPrefs.SetString("time", currentTime);
                }
            }

            gameStarted = false;
            musicManager.Stop();
            gameOverScreen.SetActive(true);
            studentController.enabled = false;
            cherryContoller.enabled = false;
            ghostController.enabled = false;
            studentController.SetDeadState();
            Timer gameOverTimer = gameOverScreen.GetComponent<Timer>();
            gameOverTimer.StartTimer(3);
            Actions.OnTimerFinish += ExitGame;
        }
    }

    private void ExitGame(GameObject obj)
    {
        Actions.OnTimerFinish -= ExitGame;
        GetComponent<UIManager>().LoadStartScreen();
    }

    private void ComputeGameTimer()
    {
        int seconds = (int) secondsTillStart % 60;
        int minutes = ((int)secondsTillStart) / 60;
        int millies = (int)((secondsTillStart - (int) secondsTillStart) * 100.0d);
        string secondsString = String.Format("{0:D2}", seconds);
        string minutesString = String.Format("{0:D2}", minutes);
        string milliesString = String.Format("{0:D2}", millies);
        gameTimer.text = $"Time: {minutesString}:{secondsString}:{milliesString}";
    }

    public void AddPoints(int amount)
    {
        score += amount;
        title.text = "Score: " + score;
    }

    public void SetScaredState()
    {
        ghostController.SetScared();
        musicManager.PlayScared();
        scaredTimer.enabled = true;
        Actions.OnTimerChange += IncrementTimer;
        Actions.OnTimerFinish += TimerFinished;
        ghostsScared = true;
    }

    public void SetTransitionState()
    {
        ghostController.SetTransition();
    }

    public void SetNormalState()
    {
        ghostController.SetNormal();
        if(!ghostDead) musicManager.PlayGame();
        studentController.SetNormalState();
    }

    public void SetDeadState(GameObject bird)
    {
        musicManager.PlayGhostDead();
        ghostController.SetDead(bird);
        ghostDead = true;
    }

    private void TimerFinished(GameObject obj)
    {
        if(obj.Equals(gameObject))
        {
            scaredTimer.text = "Scared: 10";
            scaredTimer.enabled = false;
            scaredTimerVal = 10;
            Actions.OnTimerChange -= IncrementTimer;
            Actions.OnTimerFinish -= TimerFinished;
            SetNormalState();
            ghostsScared = false;
        }
        else
        {
            ghostDead = false;
            if(!ghostsScared) musicManager.PlayGame();
        }
    }

    private void IncrementTimer(GameObject obj)
    {
        if (obj.Equals(gameObject))
        {
            scaredTimerVal -= 1;
            scaredTimer.text = "Scared: " + scaredTimerVal;
            if (scaredTimerVal <= 3)
            {
                SetTransitionState();
            }
        }
        
    }

    public bool DecreaseLiveCount()
    {
        lives -= 1;
        switch (lives)
        {
            case 2:
                GameObject.Find("Live3").SetActive(false);
                break;
            case 1:
                GameObject.Find("Live2").SetActive(false);
                break;
            case 0:
                GameObject.Find("Live1").SetActive(false);
                return true;
            default:
                break;
        }
        return false;
    }
}
