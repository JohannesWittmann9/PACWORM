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

    private List<GameObject> ghosts;
    private List<Animator> ghostAnimators;


    // Start is called before the first frame update
    void Start()
    {
        ghosts = new List<GameObject>() { blueBird, redBird, yellowBird, greenBird };
        ghostAnimators = new List<Animator>();
        ghostAnimators.Add(blueBird.GetComponent<Animator>());
        ghostAnimators.Add(redBird.GetComponent<Animator>());
        ghostAnimators.Add(yellowBird.GetComponent<Animator>());
        ghostAnimators.Add(greenBird.GetComponent<Animator>());
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
        }

    }

    public void SetTransition()
    {
        foreach (Animator an in ghostAnimators)
        {
            ResetStates(an);
            an.SetBool("transitionState", true);
            an.SetBool("walkLeft", true);
        }
    }

    public void SetNormal()
    {
        foreach (Animator an in ghostAnimators)
        {
            ResetStates(an);
            an.SetBool("normalState", true);
            an.SetBool("walkLeft", true);
        }
    }

    private void ResetStates(Animator animator)
    {
        animator.SetBool("normalState", false);
        animator.SetBool("transitionState", false);
        animator.SetBool("scaredState", false);
    }
}
