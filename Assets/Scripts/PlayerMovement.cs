using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private AudioClip playerMovementClip;

    private AudioSource audioSource;
    private Tweener tweener;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = 5 * movementSpeed;
        Vector3 end1 = transform.position + new Vector3(-5.0f, 0.0f, 0.0f);
        tweener.AddTween(transform, transform.position, end1, 1.5f);
        time = 4 * movementSpeed;
        Vector3 end2 = transform.position + new Vector3(-5.0f, 4.0f, 0.0f);
        tweener.AddTween(transform, transform.position, end2, time);

    }


}
