using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GhostController : MonoBehaviour
{
    [SerializeField] float moveDuration;
    [SerializeField] Tweener tweener;
    [SerializeField] int behaviour;
    [SerializeField] float durationToStartPos;

    private GameObject target;

    private Animator ghostAnimator;

    private Vector3 startPos;

    private List<Vector3> startPoints;
    private List<Vector3> gatePoints;

    private int normalBehaviour;

    private GhostManager manager;
    private GameManager gameManager;

    public int NormalBehaviour
    {
        get { return normalBehaviour; }
    }

    public int Behaviour
    {
        set { behaviour = value; }
    }
    
    
    private Vector3 lastPosition;
    private Vector3 currentPosition;



    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        normalBehaviour = behaviour;
        ghostAnimator = GetComponent<Animator>();
        lastPosition = transform.position;
        currentPosition = transform.position;
        target = GameObject.Find("PacStudent");

        gatePoints = new List<Vector3>();
        gatePoints.Add(new Vector3(9.5f, -5.5f, 0));
        gatePoints.Add(new Vector3(10.5f, -5.5f, 0));
        gatePoints.Add(new Vector3(9.5f, -10.5f, 0));
        gatePoints.Add(new Vector3(10.5f, -10.5f, 0));

        startPoints = new List<Vector3>();
        startPoints.Add(new Vector3(9.5f, -4.5f, 0));
        startPoints.Add(new Vector3(10.5f, -4.5f, 0));
        startPoints.Add(new Vector3(9.5f, -11.5f, 0));
        startPoints.Add(new Vector3(10.5f, -11.5f, 0));

        manager = GameObject.Find("Ghosts").GetComponent<GhostManager>();
        gameManager = GameObject.Find("Managers").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    { 
        if(transform.position == startPos)
        {
            manager.SetNormal(gameObject);
            currentPosition = startPos;
            lastPosition = startPos;
            gameManager.ResetDeadState();
        }
        if(!tweener.TweenExists(transform) && transform.position.y <= -5.5f && transform.position.y >= -10.5f && transform.position.x <= 12.5f
            && transform.position.x >= 7.5f)
        {
            Vector3 t = GetStartingPoint();
            MoveFromStartZone(t);
        }
        if(!tweener.TweenExists(transform))
        {
            Compute();
        }

    }


    private bool IsWalkable(Vector3 newPos)
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("NonWalkable");
        foreach (GameObject tile in tiles)
        {
            float distance = Vector3.Distance(tile.transform.position, newPos);
            if (distance < 0.5f)
            {
                return false;

            }
        }
        return true;
    }



    private void Compute()
    {
        ResetAnimatorStates();
        Vector3 tar = target.transform.position;
        switch (behaviour)
        {
            case 1:
                Create1Behaviour(tar);
                break;
            case 2:
                Create2Behaviour(tar);
                break;
            case 3:
                Create3Behaviour();
                break;
            case 4:
                Create4Behaviour();
                break;
            default:
                break;
        }
    }

    private void ResetAnimatorStates()
    {
        ghostAnimator.SetBool("walkUp", false);
        ghostAnimator.SetBool("walkDown", false);
        ghostAnimator.SetBool("walkRight", false);
        ghostAnimator.SetBool("walkLeft", false);
    }

    private bool IsTeleporter(Vector3 newPos)
    {
        Vector3 edge = GameObject.Find("LevelMap").GetComponent<LevelGenerator>().topLeft;
        float size = 28.0f;
        float xLeft = edge.x;
        float xRight = edge.x + size;

        if (newPos.x <= xLeft)
        {
            return true;
        }
        else if (newPos.x >= xRight)
        {
            return true;
        }

        return false;

    }

    public Vector3 GetStartingPoint()
    {
        Vector3 current = Vector3.zero;
        foreach(Vector3 point in startPoints)
        {
            if (Vector3.Distance(transform.position, point) < Vector3.Distance(transform.position, current)) current = point; 
        }

        return current;
    }

    public bool isStartingPoint(Vector3 pos)
    {
        foreach(Vector3 point in gatePoints)
        {
            if (point == pos) return true;
        }
        return false;
    }

    public void MoveToStartPos()
    {
        tweener.StopTween(transform);
        tweener.AddTween(transform, transform.position, startPos, durationToStartPos);
    }

    private void Create1Behaviour(Vector3 target)
    {
        Vector3 pacPos = target;

        Vector3 leftTile = new Vector3(currentPosition.x - 1, currentPosition.y, 0);
        Vector3 rightTile = new Vector3(currentPosition.x + 1, currentPosition.y, 0);
        Vector3 upperTile = new Vector3(currentPosition.x, currentPosition.y + 1, 0);
        Vector3 lowerTile = new Vector3(currentPosition.x, currentPosition.y - 1, 0);

        List<Vector3> directions = new List<Vector3>() { leftTile, rightTile, upperTile, lowerTile };
        List<Vector3> sortedDirections = new List<Vector3>();
        foreach(Vector3 vec in directions)
        {
            if (vec == lastPosition) continue;

            if (!IsWalkable(vec)) continue;

            if (IsTeleporter(vec)) continue;

            if (isStartingPoint(vec)) continue;

            sortedDirections.Add(vec);
        }

        System.Random random = new System.Random();
        sortedDirections = sortedDirections.OrderBy((vec) => random.Next()).ToList<Vector3>();

        // If there is no suitable direction, we still have to keep moving
        Vector3 direction = sortedDirections[0];
        foreach(Vector3 vec in sortedDirections)
        {
            float newDistance = Vector3.Distance(pacPos, vec);
            float oldDistance = Vector3.Distance(pacPos, currentPosition);
            if(newDistance >= oldDistance)
            {
                direction = vec;
            }
        }

        if(direction == leftTile) ghostAnimator.SetBool("walkLeft", true);
        else if (direction == rightTile) ghostAnimator.SetBool("walkRight", true);
        else if (direction == upperTile) ghostAnimator.SetBool("walkUp", true);
        else if (direction == lowerTile) ghostAnimator.SetBool("walkDown", true);

        lastPosition = currentPosition;
        currentPosition = direction;
        tweener.AddTween(transform, lastPosition, direction, moveDuration);
    }

    private void Create2Behaviour(Vector3 target)
    {
        Vector3 pacPos = target;

        Vector3 leftTile = new Vector3(currentPosition.x - 1, currentPosition.y, 0);
        Vector3 rightTile = new Vector3(currentPosition.x + 1, currentPosition.y, 0);
        Vector3 upperTile = new Vector3(currentPosition.x, currentPosition.y + 1, 0);
        Vector3 lowerTile = new Vector3(currentPosition.x, currentPosition.y - 1, 0);

        List<Vector3> directions = new List<Vector3>() { leftTile, rightTile, upperTile, lowerTile };
        List<Vector3> sortedDirections = new List<Vector3>();
        foreach (Vector3 vec in directions)
        {
            if (vec == lastPosition) continue;

            if (!IsWalkable(vec)) continue;

            if (IsTeleporter(vec)) continue;

            if (isStartingPoint(vec)) continue;

            sortedDirections.Add(vec);
        }

        System.Random random = new System.Random();
        sortedDirections = sortedDirections.OrderBy((vec) => random.Next()).ToList<Vector3>();

        // If there is no suitable direction, we still have to keep moving
        Vector3 direction = sortedDirections[0];
        foreach (Vector3 vec in sortedDirections)
        {
            float newDistance = Vector3.Distance(pacPos, vec);
            float oldDistance = Vector3.Distance(pacPos, currentPosition);
            if (newDistance <= oldDistance)
            {
                direction = vec;
            }
        }

        if (direction == leftTile) ghostAnimator.SetBool("walkLeft", true);
        else if (direction == rightTile) ghostAnimator.SetBool("walkRight", true);
        else if (direction == upperTile) ghostAnimator.SetBool("walkUp", true);
        else if (direction == lowerTile) ghostAnimator.SetBool("walkDown", true);

        lastPosition = currentPosition;
        currentPosition = direction;
        tweener.AddTween(transform, lastPosition, direction, moveDuration);
    }

    private void Create3Behaviour()
    {
        Vector3 leftTile = new Vector3(currentPosition.x - 1, currentPosition.y, 0);
        Vector3 rightTile = new Vector3(currentPosition.x + 1, currentPosition.y, 0);
        Vector3 upperTile = new Vector3(currentPosition.x, currentPosition.y + 1, 0);
        Vector3 lowerTile = new Vector3(currentPosition.x, currentPosition.y - 1, 0);

        List<Vector3> directions = new List<Vector3>() { leftTile, rightTile, upperTile, lowerTile };
        List<Vector3> sortedDirections = new List<Vector3>();
        foreach (Vector3 vec in directions)
        {
            if (vec == lastPosition) continue;

            if (!IsWalkable(vec)) continue;

            if (IsTeleporter(vec)) continue;

            if (isStartingPoint(vec)) continue;

            sortedDirections.Add(vec);
        }

        System.Random random = new System.Random();
        sortedDirections = sortedDirections.OrderBy((vec) => random.Next()).ToList<Vector3>();

        // If there is no suitable direction, we still have to keep moving
        Vector3 direction = sortedDirections[0];
        

        if (direction == leftTile) ghostAnimator.SetBool("walkLeft", true);
        else if (direction == rightTile) ghostAnimator.SetBool("walkRight", true);
        else if (direction == upperTile) ghostAnimator.SetBool("walkUp", true);
        else if (direction == lowerTile) ghostAnimator.SetBool("walkDown", true);

        lastPosition = currentPosition;
        currentPosition = direction;
        tweener.AddTween(transform, lastPosition, direction, moveDuration);
    }

    private void Create4Behaviour()
    {
        Vector3 leftTile = new Vector3(currentPosition.x - 1, currentPosition.y, 0);
        Vector3 rightTile = new Vector3(currentPosition.x + 1, currentPosition.y, 0);
        Vector3 upperTile = new Vector3(currentPosition.x, currentPosition.y + 1, 0);
        Vector3 lowerTile = new Vector3(currentPosition.x, currentPosition.y - 1, 0);

        List<Vector3> directions = new List<Vector3>() { leftTile, rightTile, upperTile, lowerTile };
        List<Vector3> sortedDirections = new List<Vector3>();
        foreach (Vector3 vec in directions)
        {
            if (vec == lastPosition) continue;

            if (!IsWalkable(vec)) continue;

            if (IsTeleporter(vec)) continue;

            if (isStartingPoint(vec)) continue;

            sortedDirections.Add(vec);
        }

        System.Random random = new System.Random();
        sortedDirections = sortedDirections.OrderBy((vec) => random.Next()).ToList<Vector3>();

        // If there is no suitable direction, we still have to keep moving
        Vector3 direction = sortedDirections[0];

        if (direction == leftTile) ghostAnimator.SetBool("walkLeft", true);
        else if (direction == rightTile) ghostAnimator.SetBool("walkRight", true);
        else if (direction == upperTile) ghostAnimator.SetBool("walkUp", true);
        else if (direction == lowerTile) ghostAnimator.SetBool("walkDown", true);

        lastPosition = currentPosition;
        currentPosition = direction;
        tweener.AddTween(transform, lastPosition, direction, moveDuration);
    }

    private void MoveFromStartZone(Vector3 target)
    {
        Vector3 pacPos = target;

        Vector3 leftTile = new Vector3(currentPosition.x - 1, currentPosition.y, 0);
        Vector3 rightTile = new Vector3(currentPosition.x + 1, currentPosition.y, 0);
        Vector3 upperTile = new Vector3(currentPosition.x, currentPosition.y + 1, 0);
        Vector3 lowerTile = new Vector3(currentPosition.x, currentPosition.y - 1, 0);

        List<Vector3> directions = new List<Vector3>() { leftTile, rightTile, upperTile, lowerTile };
        List<Vector3> sortedDirections = new List<Vector3>();
        foreach (Vector3 vec in directions)
        {
            if (vec == lastPosition) continue;

            if (!IsWalkable(vec)) continue;

            sortedDirections.Add(vec);
        }

        System.Random random = new System.Random();
        sortedDirections = sortedDirections.OrderBy((vec) => random.Next()).ToList<Vector3>();

        // If there is no suitable direction, we still have to keep moving
        Vector3 direction = sortedDirections[0];
        foreach (Vector3 vec in sortedDirections)
        {
            float newDistance = Vector3.Distance(pacPos, vec);
            float oldDistance = Vector3.Distance(pacPos, currentPosition);
            if (newDistance < oldDistance)
            {
                direction = vec;
            }
        }

        if (direction == leftTile) ghostAnimator.SetBool("walkLeft", true);
        else if (direction == rightTile) ghostAnimator.SetBool("walkRight", true);
        else if (direction == upperTile) ghostAnimator.SetBool("walkUp", true);
        else if (direction == lowerTile) ghostAnimator.SetBool("walkDown", true);

        lastPosition = currentPosition;
        currentPosition = direction;
        tweener.AddTween(transform, lastPosition, direction, moveDuration);
    }
}
