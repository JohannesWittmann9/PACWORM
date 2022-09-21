using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    private int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    public GameObject outsideCorner;
    public GameObject outsideWall;
    public GameObject insideCorner;
    public GameObject insideWall;
    public GameObject pellet;
    public GameObject powerPellet;
    public GameObject outsideJunction;


    void Start()
    {
        int lengthX = levelMap.GetLength(0);
        int lengthY = levelMap.GetLength(1);
        int[,] copy = (int[,])levelMap.Clone();
        Destroy(GameObject.Find("Level1"));

        Vector3 topLeft = new Vector3(-13, 8, 0);
        CreateLevel(topLeft);

        Vector3 topRight = topLeft;
        topRight.x += lengthX - 1;
        levelMap = RotateY180(levelMap);
        CreateLevel(topRight);

        topRight.y -= lengthY + 1; ;
        levelMap = RotateX180(levelMap);
        CreateLevel(topRight);

        Vector3 bottomLeft = topLeft;
        bottomLeft.y -= lengthY + 1;
        levelMap = copy;
        levelMap = RotateX180(levelMap);
        CreateLevel(bottomLeft);

    }


    private void CreateLevel(Vector3 startPos)
    {
        

        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                int number = levelMap[i, j];
                GameObject prefab = GetGameObjectForNumber(number);
                Vector3 position = startPos;
                position.x += j;
                position.y -= i;

                if (prefab == null)
                {
                    continue;
                }

                else if (number == 5 || number == 6)
                {
                    InstantiateNewTile(prefab, position, Quaternion.identity);
                    continue;
                }

                else if (i == 0 && j == 0)
                {
                    InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 270));
                    continue;
                }

                else if (i == 0 && j != 0)
                {
                    int prev = levelMap[i, j - 1];

                    if (number == 2 && (prev == 1 || prev == 2 || prev == 7))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if (number == 1 && prev == 2)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }
                    else if (number == 7)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }
                    else if (prev == 0 && number == 1)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 270));
                        continue;
                    }
                }

                else if (i != 0 && j == 0)
                {
                    int prevUp = levelMap[i - 1, j];

                    if ((prevUp == 2 || prevUp == 1) && number == 2)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevUp == 2 && number == 1)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.identity);
                        continue;
                    }
                    else if ((prevUp == 0 || prevUp == 5 || prevUp == 6) && number == 1)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 270));
                        continue;
                    }
                    else if ((prevUp == 0 || prevUp == 5 || prevUp == 6) && (number == 2 || number == 7))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                }

                else if (i != 0 && j != 0 && j < (levelMap.GetLength(1) - 1) && i < (levelMap.GetLength(0) - 1))
                {
                    int prevUp = levelMap[i - 1, j];
                    int prevLeft = levelMap[i, j - 1];
                    int nextDown = levelMap[i + 1, j];
                    int nextRight = levelMap[i, j + 1];
                    int downRight = levelMap[i + 1, j + 1];
                    int upRight = levelMap[i - 1, j + 1];
                    int downLeft = levelMap[i + 1, j - 1];
                    int upLeft = levelMap[i - 1, j - 1];

                    if (IsItemOrEmpty(prevUp) && number == 4)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    // I dont actually think a junction can be used here, but for safety
                    else if (IsItemOrEmpty(prevUp) && number == 7)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }
                    else if (IsItemOrEmpty(prevUp) && prevLeft == 4 && number == 4)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if (!IsItemOrEmpty(prevUp) && !IsItemOrEmpty(prevLeft) && number == 3 && !IsItemOrEmpty(nextRight)
                        && !IsItemOrEmpty(nextDown) && IsItemOrEmpty(downRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 270));
                        continue;
                    }
                    else if (!IsItemOrEmpty(prevUp) && !IsItemOrEmpty(prevLeft) && number == 3 && !IsItemOrEmpty(nextRight)
                        && !IsItemOrEmpty(nextDown) && IsItemOrEmpty(upRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if (!IsItemOrEmpty(prevUp) && !IsItemOrEmpty(prevLeft) && number == 3 && !IsItemOrEmpty(nextRight)
                        && !IsItemOrEmpty(nextDown) && IsItemOrEmpty(downLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }
                    else if (!IsItemOrEmpty(prevUp) && !IsItemOrEmpty(prevLeft) && number == 3 && !IsItemOrEmpty(nextRight)
                        && !IsItemOrEmpty(nextDown) && IsItemOrEmpty(upLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (IsItemOrEmpty(prevUp) && prevLeft == 4 && number == 3 && IsItemOrEmpty(nextRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }
                    else if (IsItemOrEmpty(prevUp) && prevLeft == 4 && number == 3 && !IsItemOrEmpty(nextRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 270));
                        continue;
                    }
                    else if (IsItemOrEmpty(prevUp) && IsItemOrEmpty(prevLeft) && number == 3)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 270));
                        continue;
                    }
                    else if ((prevUp == 4 || prevUp == 3) && IsItemOrEmpty(prevLeft) && number == 3)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if (prevLeft == 4 && number == 3 && IsItemOrEmpty(nextRight) && IsItemOrEmpty(prevUp))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }
                    else if (prevLeft == 4 && number == 3 && IsItemOrEmpty(nextRight) && !IsItemOrEmpty(prevUp))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevLeft == 4 && number == 3 && !IsItemOrEmpty(nextRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if ((prevLeft == 4 || prevLeft == 3) && number == 4 && !IsItemOrEmpty(nextRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if ((prevLeft == 4 || prevLeft == 3) && number == 4 && IsItemOrEmpty(nextRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (IsItemOrEmpty(prevLeft) && number == 4)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevLeft == 3 && number == 3 && IsItemOrEmpty(prevUp))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }
                    else if (prevLeft == 3 && number == 3 && IsItemOrEmpty(nextDown))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevUp == 7 && number == 4)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevUp == 7 && number == 3 && IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevUp == 7 && number == 3 && !IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }

                    // Outer tiles
                    else if (prevLeft == 1 && number == 2)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if (prevLeft == 2 && number == 2 && IsItemOrEmpty(prevUp))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if (prevLeft == 2 && number == 2 && !IsItemOrEmpty(prevUp))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (number == 1 && IsItemOrEmpty(prevUp) && IsItemOrEmpty(nextRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }
                    else if (number == 1 && IsItemOrEmpty(nextDown) && IsItemOrEmpty(nextRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (number == 1 && IsItemOrEmpty(prevUp) && IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 270));
                        continue;
                    }
                    else if (number == 1 && IsItemOrEmpty(nextDown) && IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if (prevLeft == 2 && number == 1 && !IsItemOrEmpty(prevUp))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevUp == 1 && number == 2)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevUp == 2 && number == 1 && IsItemOrEmpty(nextRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevUp == 2 && number == 1 && !IsItemOrEmpty(nextRight))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    else if (prevUp == 7 && number == 2)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    else if (prevUp == 2 && number == 2 && IsItemOrEmpty(nextRight) && IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    

                }
                else if (i == (levelMap.GetLength(0) - 1) && j != (levelMap.GetLength(1) - 1) && i != 0)
                {
                    int prevUp = levelMap[i - 1, j];
                    int nextRight = levelMap[i, j + 1];
                    int prevLeft = levelMap[i, j + -1];

                    if (prevUp == 4 && number == 4 && IsItemOrEmpty(nextRight) && IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    if (prevUp == 4 && number == 3 && !IsItemOrEmpty(nextRight) && IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    if (prevUp == 4 && number == 3 && IsItemOrEmpty(nextRight) && prevLeft == 4)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    if (prevUp == 3 && number == 3 && IsItemOrEmpty(nextRight) && !IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    if (prevUp == 3 && number == 3 && !IsItemOrEmpty(nextRight) && IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }

                }
                else if (j == (levelMap.GetLength(1) - 1) && j != 0 && i != (levelMap.GetLength(0) - 1))
                {
                    int prevUp = levelMap[i - 1, j];
                    int prevLeft = levelMap[i, j -1];
                    int nextDown = levelMap[i + 1, j];

                    if ((prevUp == 7 || prevUp == 4 || prevUp == 3) && number == 4 && IsItemOrEmpty(prevLeft) && !IsItemOrEmpty(nextDown))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }

                    if (number == 4 && IsItemOrEmpty(nextDown))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    if (number == 4 && IsItemOrEmpty(prevUp) && !IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }

                    if (number == 3 && !IsItemOrEmpty(nextDown) && !IsItemOrEmpty(prevLeft) && IsItemOrEmpty(prevUp))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }

                    if (number == 3 && IsItemOrEmpty(nextDown) && IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 0));
                        continue;
                    }
                    if (number == 3 && IsItemOrEmpty(nextDown) && !IsItemOrEmpty(prevLeft) && prevUp == 4)
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 90));
                        continue;
                    }
                    if (number == 3 && !IsItemOrEmpty(nextDown) && !IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }
                    if (number == 3 && !IsItemOrEmpty(nextDown) && !IsItemOrEmpty(prevLeft))
                    {
                        InstantiateNewTile(prefab, position, Quaternion.Euler(0, 0, 180));
                        continue;
                    }

                }
            }
        }
    }

    private GameObject GetGameObjectForNumber(int number)
    {
        switch (number)
        {
            case 1:
                return outsideCorner;
            case 2:
                return outsideWall;
            case 3:
                return insideCorner;
            case 4:
                return insideWall;
            case 5:
                return pellet;
            case 6:
                return powerPellet;
            case 7:
                return outsideJunction;
            default:
                return null;
        }
    }

    private void InstantiateNewTile(GameObject prefab, Vector3 position, Quaternion quaternion)
    {
        GameObject newObj = Instantiate(prefab, position, quaternion);
        newObj.transform.parent = gameObject.transform;
    }

    private bool IsItemOrEmpty(int num)
    {
        return num == 0 || num == 5 || num == 6;
    }

    private int[,] RotateY180(int[,] array)
    {
        int x = array.GetLength(0);
        int y = array.GetLength(1);

        int[,] result = (int[,])array.Clone();

        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                result[i, j] = array[i, y - 1 - j];
            }
        }

        return result;
    }

    private int[,] RotateX180(int[,] array)
    {
        int x = array.GetLength(0);
        int y = array.GetLength(1);

        int[,] result = (int[,])array.Clone();

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                result[i, j] = array[x - 1 -i, j];
            }
        }

        return result;
    }

}
