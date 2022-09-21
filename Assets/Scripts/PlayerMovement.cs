using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private AudioClip playerMovementClip;

    private AudioSource audioSource;
    private Animator animator;
    private Tweener tweener;
    private bool coroutineRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        tweener = GetComponent<Tweener>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        if (!audioSource.isPlaying)
        {
            audioSource.clip = playerMovementClip;
            audioSource.Play();
        }
        if (!coroutineRunning)
        {
            coroutineRunning = true;
            StartCoroutine(MoveClockwise());
        }
    }

    IEnumerator MoveClockwise()
    {
        float time = 5 * movementSpeed;
        Vector3 position = transform.position;
        position.x -= 5;
        tweener.AddTween(transform, transform.position, position, time);
        yield return new WaitForSeconds(time);
        time = 4 * movementSpeed;
        position = transform.position;
        position.y += 4;
        tweener.AddTween(transform, transform.position, position, time);
        animator.SetFloat("horizontal", 0.0f);
        animator.SetFloat("vertical", 1.0f);
        yield return new WaitForSeconds(time);
        time = 5 * movementSpeed;
        position = transform.position;
        position.x += 5;
        tweener.AddTween(transform, transform.position, position, time);
        animator.SetFloat("vertical", 0.0f);
        animator.SetFloat("horizontal", 1.0f);
        yield return new WaitForSeconds(time);
        time = 4 * movementSpeed;
        position = transform.position;
        position.y -= 4;
        tweener.AddTween(transform, transform.position, position, time);
        animator.SetFloat("horizontal", 0.0f);
        animator.SetFloat("vertical", -1.0f);
        yield return new WaitForSeconds(time);
        coroutineRunning = false;
        animator.SetFloat("horizontal", -1.0f);
        animator.SetFloat("vertical", 0.0f);
    }
}
