using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GhostController ghostController;

    private TMPro.TextMeshProUGUI title;

    private TMPro.TextMeshProUGUI scaredTimer;
    private int scaredTimerVal = 10;

    private MusicManager musicManager;

    private int score = 0;
    private int lives = 3;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        title = GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
        scaredTimer = GameObject.Find("GhostScaredTimer").GetComponent<TMPro.TextMeshProUGUI>();
        ghostController = GameObject.Find("Ghosts").GetComponent<GhostController>();
        musicManager = GameObject.Find("Managers").GetComponent<MusicManager>();
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
    }

    public void SetTransitionState()
    {
        ghostController.SetTransition();
    }

    public void SetNormalState()
    {
        ghostController.SetNormal();
        musicManager.PlayGame();
    }

    private void TimerFinished()
    {
        scaredTimer.text = "Scared: 10";
        scaredTimer.enabled = false;
        Actions.OnTimerChange -= IncrementTimer;
        Actions.OnTimerFinish -= TimerFinished;
        SetNormalState();
    }

    private void IncrementTimer()
    {
        scaredTimerVal -= 1;
        scaredTimer.text = "Scared: " + scaredTimerVal;
        if(scaredTimerVal == 3)
        {
            SetTransitionState();
        }
    }

    public void DecreaseLiveCount()
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
                break;
            default:
                break;
        }
    }
}
