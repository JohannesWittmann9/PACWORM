using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject pacStudent;
    [SerializeField] float xOffset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateCameraPosition();
    }

    private void CalculateCameraPosition()
    {
        Vector3 playerPos = pacStudent.transform.position;
        Vector3 cameraPos = mainCamera.transform.position;
        float halfSizeY = mainCamera.orthographicSize;
        float halfSizeX = mainCamera.orthographicSize * mainCamera.aspect;
        float cameraLeftBorder = cameraPos.x - halfSizeX;
        float cameraRightBorder = cameraPos.x + halfSizeX;
        float cameraTopBorder = cameraPos.y + halfSizeY;
        float cameraBottomBorder = cameraPos.y - halfSizeY;

        if (playerPos.x > cameraRightBorder) mainCamera.transform.position = new Vector3(cameraPos.x + xOffset, cameraPos.y, cameraPos.z);
        if (playerPos.x < cameraLeftBorder) mainCamera.transform.position = new Vector3(cameraPos.x - xOffset, cameraPos.y, cameraPos.z);
        if (playerPos.y > cameraTopBorder) mainCamera.transform.position = new Vector3(cameraPos.x, cameraPos.y + (2 * halfSizeY), cameraPos.z);
        if (playerPos.y < cameraBottomBorder) mainCamera.transform.position = new Vector3(cameraPos.x, cameraPos.y - (2 * halfSizeY), cameraPos.z);
    }
}
