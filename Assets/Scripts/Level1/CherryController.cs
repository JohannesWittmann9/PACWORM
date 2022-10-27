using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CherryController : MonoBehaviour
{
    [SerializeField] int spawnRateInSeconds;
    [SerializeField] float speed;
    [SerializeField] Camera mainCamera;
    [SerializeField] Tweener tweener;

    private GameObject cherry;


    // Start is called before the first frame update
    void Start()
    {
        cherry = GameObject.Find("BonusCherry");
        cherry.SetActive(false);
        InvokeRepeating("SpawnCherry", 0, spawnRateInSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        if (!tweener.TweenExists(cherry.transform)) cherry.SetActive(false);
    }

    private void SpawnCherry()
    {
        cherry.SetActive(true);
        StartCherry();
    }

    private void StartCherry()
    {
        System.Random rand = new System.Random();
        int side = rand.Next(1, 5);
        Vector3 camPos = mainCamera.transform.position;
        float cameraYHalf = mainCamera.orthographicSize;
        float cameraXHalf = cameraYHalf * mainCamera.aspect;
        Vector3 left = new Vector3(0 - cameraXHalf - 1, 0, 0);
        Vector3 bottom = new Vector3(0, 0 - cameraYHalf - 1, 0);
        Vector3 right = new Vector3(0 + cameraXHalf + 1, 0, 0);
        Vector3 top = new Vector3(0, 0 + cameraYHalf + 1, 0);
        Vector3 startPoint = Vector3.zero;
        float y, x;
        switch (side)
        {
            case 1:
                // left
                y = UnityEngine.Random.Range(cameraYHalf, -cameraYHalf);
                startPoint = new Vector3(left.x, y);
                break;
            case 2:
                // bottom
                x = UnityEngine.Random.Range(cameraXHalf, -cameraXHalf);
                startPoint = new Vector3(x, bottom.y);
                break;
            case 3:
                // right
                y = UnityEngine.Random.Range(cameraYHalf, -cameraYHalf);
                startPoint = new Vector3(right.x, y);
                break;
            case 4:
                // top
                x = UnityEngine.Random.Range(cameraXHalf, -cameraXHalf);
                startPoint = new Vector3(x, top.y);
                break;
            default:
                break;
        }

        cherry.transform.position = startPoint;
        Vector3 endPos = Vector3.zero - startPoint;
        tweener.AddTween(cherry.transform, cherry.transform.position, endPos, speed);
    }


}
