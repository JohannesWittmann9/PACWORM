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
    private bool ghostDead = false;
    private bool ghostsScared = false;
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
