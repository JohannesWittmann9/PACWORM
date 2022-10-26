using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{

    [SerializeField] float moveDuration;
    [SerializeField] AudioClip pacMovementClip;
    [SerializeField] AudioClip pacEatClip;
    [SerializeField] AudioClip pacWallClip;

    private GameObject[] tiles;
    private Tweener tweener;
    private AudioSource audioSource;
    private Animator animator;
    private KeyCode lastInput;
    private KeyCode currentInput;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        tiles = GameObject.FindGameObjectsWithTag("NonWalkable");

    }

    // Update is called once per frame
    void Update()
    {
        IsWalkable(transform.position);
        if (Input.GetKey(KeyCode.W))
        {
            lastInput = KeyCode.W;
        }
        if (Input.GetKey(KeyCode.A))
        {
            lastInput = KeyCode.A;
        }
        if (Input.GetKey(KeyCode.S))
        {
            lastInput = KeyCode.S;
        }
        if (Input.GetKey(KeyCode.D))
        {
            lastInput = KeyCode.D;
        }

        if (tweener.TweenExists(transform))
        {
            PlayMovementClip();
        }

        bool canWalk = ComputeInput(lastInput);
        if (!canWalk) ComputeInput(currentInput);

        
    }

    private void PlayMovementClip()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = pacMovementClip;
            audioSource.Play();
        }
    }

    private void PlayWallClip()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = pacWallClip;
            audioSource.Play();
        }
        
    }

    private bool IsWalkable(Vector3 newPos)
    {
        foreach(GameObject tile in tiles)
        {
            float distance = Vector3.Distance(tile.transform.position, newPos);
            if(distance < 0.5f)
            {
                return false;
                
            }
        }
        return true;
    }

    private bool ComputeInput(KeyCode input)
    {
        if (!tweener.TweenExists(transform))
        {
            animator.SetBool("walkUp", false);
            animator.SetBool("walkDown", false);
            animator.SetBool("walkRight", false);
            animator.SetBool("walkLeft", false);

            switch (input)
            {
                case KeyCode.W:
                    Vector3 newPos = new Vector3(transform.position.x, transform.position.y + 1);
                    if (IsWalkable(newPos))
                    {
                        currentInput = input;
                        tweener.AddTween(transform, transform.position, newPos, moveDuration);
                        animator.SetBool("walkUp", true);
                        return true;
                    }
                    break;
                case KeyCode.A:
                    newPos = new Vector3(transform.position.x - 1, transform.position.y);
                    if (IsWalkable(newPos))
                    {
                        currentInput = input;
                        tweener.AddTween(transform, transform.position, newPos, moveDuration);
                        animator.SetBool("walkLeft", true);
                        return true;
                    }
                    break;
                case KeyCode.S:
                    newPos = new Vector3(transform.position.x, transform.position.y - 1);
                    if (IsWalkable(newPos))
                    {
                        currentInput = input;
                        tweener.AddTween(transform, transform.position, newPos, moveDuration);
                        animator.SetBool("walkDown", true);
                        return true;
                    }

                    break;
                case KeyCode.D:
                    newPos = new Vector3(transform.position.x + 1, transform.position.y);
                    if (IsWalkable(newPos))
                    {
                        currentInput = input;
                        tweener.AddTween(transform, transform.position, newPos, moveDuration);
                        animator.SetBool("walkRight", true);
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        /* When there is no tween we dont want to check current input */
        return true;
    }
}
