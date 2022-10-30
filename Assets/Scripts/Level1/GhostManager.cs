using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] GameObject blueBird;
    [SerializeField] GameObject redBird;
    [SerializeField] GameObject yellowBird;
    [SerializeField] GameObject greenBird;

    private List<Animator> ghostAnimators;
    private List<GhostController> controllers;
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

        controllers = new List<GhostController>();
        controllers.Add(blueBird.GetComponent<GhostController>());
        controllers.Add(redBird.GetComponent<GhostController>());
        controllers.Add(yellowBird.GetComponent<GhostController>());
        controllers.Add(greenBird.GetComponent<GhostController>());

        managers = GameObject.Find("Managers");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStateController(bool enabled)
    {
        foreach(GhostController cont in controllers)
        {
            cont.enabled = enabled;
        }
    }

    public void SetScared()
    {
        foreach(Animator an in ghostAnimators)
        {
            ResetStates(an);
            an.SetBool("scaredState", true);
            ghostsScared = true;
        }

        foreach(GhostController cntrl in controllers)
        {
            cntrl.Behaviour = 1;
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
            }
        }

        foreach (GhostController cntrl in controllers)
        {
            cntrl.Behaviour = 1;
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

        foreach (GhostController cntrl in controllers)
        {
            cntrl.Behaviour = cntrl.NormalBehaviour;
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
            int deadGhosts = CountDeadBirds();
            MusicManager mm = GameObject.Find("BackgroundMusic").GetComponent<MusicManager>();
            if (!ghostsScared) mm.PlayGame();
            else if(deadGhosts == 0) mm.PlayScared();
            GhostEnabled(bird, true);
        }
        
    }

    public void SetDead(GameObject bird)
    {
        Animator anim = bird.GetComponent<Animator>();
        ResetStates(anim);
        anim.SetBool("deadState", true);
        GhostEnabled(bird, false);
        bird.GetComponent<GhostController>().MoveToStartPos();

    }

    private void ResetStates(Animator animator)
    {
        animator.SetBool("normalState", false);
        animator.SetBool("transitionState", false);
        animator.SetBool("scaredState", false);
        animator.SetBool("deadState", false);
    }

    private void GhostEnabled(GameObject ghost, bool enabled)
    {
        ghost.GetComponent<BoxCollider2D>().enabled = enabled;   
    }

    private int CountDeadBirds()
    {
        int counter = 0;
        foreach(Animator anim in ghostAnimators)
        {
            if (anim.GetBool("deadState")) counter += 1;
        }
        return counter;
    }
}
