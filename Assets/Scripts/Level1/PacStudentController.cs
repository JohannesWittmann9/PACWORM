using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{

    [SerializeField] Tweener tweener;
    [SerializeField] float moveDuration;
    private KeyCode lastInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        if (!tweener.TweenExists(transform))
        {
            switch (lastInput)
            {
                case KeyCode.W:
                    Vector3 newPos = new Vector3(transform.position.x, transform.position.y + 1);
                    tweener.AddTween(transform, transform.position, newPos, moveDuration);
                    break;
                case KeyCode.A:
                    newPos = new Vector3(transform.position.x - 1, transform.position.y);
                    tweener.AddTween(transform, transform.position, newPos, moveDuration);
                    break;
                case KeyCode.S:
                    newPos = new Vector3(transform.position.x, transform.position.y - 1);
                    tweener.AddTween(transform, transform.position, newPos, moveDuration);
                    break;
                case KeyCode.D:
                    newPos = new Vector3(transform.position.x + 1, transform.position.y);
                    tweener.AddTween(transform, transform.position, newPos, moveDuration);
                    break;
                default:
                    break;
            }
        }
    }
}
