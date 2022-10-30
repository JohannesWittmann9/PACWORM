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
    }

    // Update is called once per frame
    void Update()
    {
        if (!tweener.TweenExists(cherry.transform)) cherry.SetActive(false);
    }

    private void OnEnable()
    {
        InvokeRepeating("SpawnCherry", 0, spawnRateInSeconds);
    }

    private void OnDisable()
    {
        CancelInvoke();
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
        Vector3 posCam = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f));
        Vector3 left = Vector3.zero;
        Vector3 bottom = Vector3.zero;
        Vector3 right = Vector3.zero;
        Vector3 top = Vector3.zero;
        Vector3 startPoint = Vector3.zero;
        float factor = UnityEngine.Random.Range(0, 1.0f);
        switch (side)
        {
            case 1:
                // left
                left = mainCamera.ViewportToWorldPoint(new Vector3(-0.1f, factor, 0));
                startPoint = new Vector3(left.x, left.y);
                break;
            case 2:
                // bottom
                bottom = mainCamera.ViewportToWorldPoint(new Vector3(factor, -0.1f, 0));
                startPoint = new Vector3(bottom.x, bottom.y);
                break;
            case 3:
                // right
                right = mainCamera.ViewportToWorldPoint(new Vector3(1.1f, factor, 0));
                startPoint = new Vector3(right.x, right.y);
                break;
            case 4:
                // top
                top = mainCamera.ViewportToWorldPoint(new Vector3(factor, 1.1f, 0));
                startPoint = new Vector3(top.x, top.y);
                break;
            default:
                break;
        }
        
        cherry.transform.position = startPoint;
        Vector3 middlePoint = new Vector3(posCam.x, posCam.y, 0);
        Vector3 endPos = middlePoint - (startPoint - middlePoint);
        tweener.AddTween(cherry.transform, cherry.transform.position, endPos, speed);
    }


}
