using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField] GameObject blueBird;
    [SerializeField] GameObject redBird;
    [SerializeField] GameObject yellowBird;
    [SerializeField] GameObject greenBird;
    [SerializeField] Tweener tweener;

    private List<Animator> ghostAnimators;
    private GameObject managers;
    private bool ghostsScared = false;


    // Start is called before the first frame update
    void Start()
    {
        ghostAnimators = new List<Animator>();
        ghostAnimators.Add(blueBird.GetComponent<Animator>());
        ghostAnimators.Add(redBird.GetComponent<Animator>());
        ghostAnimators.Add(yellowBird.GetComponent<Animator>());
        ghostAnimators.Add(greenBird.GetComponent<Animator>());

        managers = GameObject.Find("Managers");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScared()
    {
        foreach(Animator an in ghostAnimators)
        {
            ResetStates(an);
            an.SetBool("scaredState", true);
            an.SetBool("walkLeft", true);
            ghostsScared = true;
        }

    }

    public void SetTransition()
    {
        foreach (Animator an in ghostAnimators)
        {
            bool deadState = an.GetBool("deadState");
            if (!deadState)
            {
                ResetStates(an);
                an.SetBool("transitionState", true);
                an.SetBool("walkLeft", true);
            }
        }
    }

    public void SetNormal()
    {
        ghostsScared = false;
        foreach (Animator an in ghostAnimators)
        {
            bool deadState = an.GetBool("deadState");
            if (!deadState)
            {
                ResetStates(an);
                an.SetBool("normalState", true);
                an.SetBool("walkLeft", true);
            }
        }
    }

    public void SetNormal(GameObject bird)
    {
        Animator anim = bird.GetComponent<Animator>();
        //Check if its a bird
        if(anim != null)
        {
            ResetStates(anim);
            if (ghostsScared) anim.SetBool("scaredState", true);
            else anim.SetBool("normalState", true);
            Actions.OnTimerFinish -= SetNormal;
            if(!ghostsScared) managers.GetComponent<MusicManager>().PlayGame();
        }
        
    }

    public void SetDead(GameObject bird)
    {
        Animator anim = bird.GetComponent<Animator>();
        ResetStates(anim);
        anim.SetBool("deadState", true);
        Timer deadTimer = bird.GetComponent<Timer>();
        deadTimer.StartTimer(5);
        Actions.OnTimerFinish += SetNormal;
    }

    private void ResetStates(Animator animator)
    {
        animator.SetBool("normalState", false);
        animator.SetBool("transitionState", false);
        animator.SetBool("scaredState", false);
        animator.SetBool("deadState", false);
    }
}
