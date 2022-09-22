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

    public Vector3 topLeft;
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

    private void CreateLevel(Vector3 startPos)
    {
        int y = levelMap.GetLength(0);
        int x = levelMap.GetLength(1);

        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                // Fields around current
                // 8 just to store a none tile
                int left = 8;
                int right = 8;
                int top = 8;
                int bottom = 8;
                int leftTop = 8;
                int rightTop = 8;
                int leftBottom = 8;
                int rightBottom = 8;

                int number = levelMap[i, j];
                GameObject prefab = GetGameObjectForNumber(number);
                Vector3 position = startPos;
                position.x += j;
                position.y -= i;

                // Set all fields around
                if(i > 0)
                {
                    top = levelMap[i - 1, j];
                }
                if(j > 0)
                {
                    left = levelMap[i, j - 1];
                }
                if(i < (y - 1))
                {
                    bottom = levelMap[i + 1, j];
                }
                if(j < (x - 1))
                {
                    right = levelMap[i, j + 1];
                }
                if(i > 0 && j < (x - 1))
                {
                    rightTop = levelMap[i - 1, j + 1];
                }
                if (j > 0 && i > 0)
                {
                    leftTop = levelMap[i - 1, j - 1];
                }
                if (i < (y - 1) && j < (x - 1))
                {
                    rightBottom = levelMap[i + 1, j + 1];
                }
                if (j > 0 && i < (y - 1))
                {
                    leftBottom = levelMap[i + 1, j - 1];
                }

                // Field is Empty
                if (prefab == null)
                {
                    continue;
                }
                // Field is Item
                else if (number == 5 || number == 6)
                {
                    InstantiateNewTile(prefab, position, Quaternion.identity);
                    continue;
                }
                // Field is Wall
                else if (IsWall(number))
                {
                    Quaternion q;
                    if(IsItemOrEmpty(top) || IsItemOrEmpty(bottom))
                    {
                        q = Quaternion.Euler(0, 0, 0);
                    }
                    else //if(IsItemOrEmpty(left) || IsItemOrEmpty(right))
                    {
                        q = Quaternion.Euler(0, 0, 90);
                    }

                    InstantiateNewTile(prefab, position, q);
                    continue;
                }
                // Field is Corner
                else if (number == 1 || number == 3)
                {
                    Quaternion q = Quaternion.identity;
                    if (IsItemOrEmpty(top) && IsItemOrEmpty(right))
                    {
                        q = Quaternion.Euler(0, 0, 180);
                    }
                    else if(IsItemOrEmpty(bottom) && IsItemOrEmpty(right))
                    {
                        q = Quaternion.Euler(0, 0, 90);
                    }
                    else if (IsItemOrEmpty(top) && IsItemOrEmpty(left))
                    {
                        q = Quaternion.Euler(0, 0, 270);
                    }
                    else if (IsItemOrEmpty(bottom) && IsItemOrEmpty(left))
                    {
                        q = Quaternion.Euler(0, 0, 0);
                    }
                    else if (IsWall(right) && IsWall(bottom) && IsItemOrEmpty(rightBottom))
                    {
                        q = Quaternion.Euler(0, 0, 270);
                    }
                    else if (IsWall(right) && IsWall(top) && IsItemOrEmpty(rightTop))
                    {
                        q = Quaternion.Euler(0, 0, 0);
                    }
                    else if (IsWall(left) && IsWall(bottom) && IsItemOrEmpty(leftBottom))
                    {
                        q = Quaternion.Euler(0, 0, 180);
                    }
                    else if (IsWall(left) && IsWall(top) && IsItemOrEmpty(leftTop))
                    {
                        q = Quaternion.Euler(0, 0, 90);
                    }

                    InstantiateNewTile(prefab, position, q);
                    continue;
                }
                // Field is junction
                else if(number == 7)
                {
                    Quaternion q = Quaternion.identity;
                    if (IsWall(right) && IsWall(bottom) && IsItemOrEmpty(rightBottom) && IsOutOfMapOrItem(left) && !IsOutOfMapOrItem(top))
                    {
                        q = Quaternion.Euler(0, 0, 270);
                    }
                    else if (IsWall(right) && IsWall(bottom) && IsItemOrEmpty(rightBottom) && IsOutOfMapOrItem(top))
                    {
                        q = Quaternion.Euler(0, 0, 180);
                    }
                    else if (IsWall(right) && IsWall(top) && IsItemOrEmpty(rightTop) && IsOutOfMapOrItem(left) && !IsOutOfMapOrItem(bottom))
                    {
                        q = Quaternion.Euler(0, 0, 270);
                    }
                    else if (IsWall(right) && IsWall(top) && IsItemOrEmpty(rightTop) && IsOutOfMapOrItem(bottom))
                    {
                        q = Quaternion.Euler(0, 0, 0);
                    }
                    else if (IsWall(left) && IsWall(bottom) && IsItemOrEmpty(leftBottom) && IsOutOfMapOrItem(right) && !IsOutOfMapOrItem(top))
                    {
                        q = Quaternion.Euler(0, 0, 90);
                    }
                    else if (IsWall(left) && IsWall(bottom) && IsItemOrEmpty(leftBottom) && IsOutOfMapOrItem(top))
                    {
                        q = Quaternion.Euler(0, 0, 180);
                    }
                    else if (IsWall(left) && IsWall(top) && IsItemOrEmpty(leftTop) && IsOutOfMapOrItem(right) && !IsOutOfMapOrItem(bottom))
                    {
                        q = Quaternion.Euler(0, 0, 90);
                    }
                    else if (IsWall(left) && IsWall(top) && IsItemOrEmpty(leftTop) && IsOutOfMapOrItem(bottom))
                    {
                            q = Quaternion.Euler(0, 0, 0);
                    }
                    

                    InstantiateNewTile(prefab, position, q);
                    continue;
                }

            }
        }
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

    private bool IsWall(int number)
    {
        return number == 2 || number == 4;
    }

    private bool IsOutOfMapOrItem(int number)
    {
        return number == 8 || IsItemOrEmpty(number);
    }
}
