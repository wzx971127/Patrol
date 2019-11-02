using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour, Handle
{

    protected Vector3 bothPos;

    private bool playerIsLive;
    private Vector3 playerPos;
    private Vector3[] posSet;
    private int currentSide;
    private int sideNum;

    private bool turn;
    private bool isCatching = false;

    public int score = 1;
    public float field = 7f;
    public float speed = 1f;

    public delegate void getScore(int n);
    public event getScore escape;

    public void register(getScore s)
    {
        escape += s;
    }

    public void unRegister(getScore s)
    {
        escape -= s;
    }

    // Use this for initialization
    void Start()
    {
        transform.position = getBothPos();
        bothPos = transform.position;
    }

    void Awake()
    {
        turn = false;
        sideNum = Random.Range(3, 6);
        currentSide = 0;
        if (sideNum == 3)
        {
            posSet = new Vector3[] { new Vector3 (0, 0, 0), new Vector3 (8, 0, 0),
                new Vector3 (4, 0, 6), new Vector3 (0, 0, 0) };
        }
        else if (sideNum == 4)
        {
            posSet = new Vector3[] { new Vector3 (0, 0, 0), new Vector3 (8, 0, 0),
                new Vector3 (8, 0, 8), new Vector3 (0, 0, 8), new Vector3 (0, 0, 0) };
        }
        else
        {
            posSet = new Vector3[] { new Vector3 (0, 0, 0), new Vector3 (5, 0, 0),
                new Vector3 (7, 0, 5), new Vector3 (3, 0, 8), new Vector3 (-2, 0, 5), new Vector3 (0, 0, 0) };
        }
    }

    void OnCollisionEnter(Collision other)
    {
        turn = true;
    }

    public bool inField(Vector3 targetPos)
    {
        float distance = (transform.position - targetPos).sqrMagnitude;
        if (distance <= field * field)
        {
            return true;
        }
        return false;
    }

    public void Reaction(bool isLive, Vector3 pos)
    {
        playerIsLive = isLive;
        playerPos = pos;
    }

    public void catchPlayer()
    {
        bothPos = transform.position;
        isCatching = true;
        transform.LookAt(playerPos);
        transform.position = Vector3.Lerp(transform.position, playerPos, speed * Time.deltaTime);
    }

    public bool patrolInMap(int side)
    {
        if (isCatching && playerIsLive)
        {
            isCatching = false;
            if (escape != null)
            {
                escape(score);
            }
        }
        if (turn)
        {
            turn = false;
            Vector3 v = transform.forward;
            Quaternion dir = Quaternion.LookRotation(v);
            Quaternion toDir = Quaternion.LookRotation(-v);
            transform.rotation = Quaternion.RotateTowards(dir, toDir, 1f);
            return true;
        }
        if (transform.position != bothPos + posSet[side + 1])
        {
            transform.LookAt(bothPos + posSet[side + 1]);
            transform.position = Vector3.Lerp(transform.position,
                bothPos + posSet[side + 1], speed * Time.deltaTime);
        }
        if ((transform.position - (bothPos + posSet[side + 1])).sqrMagnitude <= 0.1f)
        {
            return true;
        }
        return false;
    }

    public Vector3 getBothPos()
    {
        while (true)
        {
            Vector3 pos = new Vector3(Random.Range(-30f, 30f), 0, Random.Range(-30f, 30f));
            if ((pos - Vector3.zero).sqrMagnitude >= 100f)
            {
                return pos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsLive && inField(playerPos))
        {
            catchPlayer();
        }
        else
        {
            if (patrolInMap(currentSide))
            {
                if (++currentSide >= sideNum)
                {
                    bothPos = transform.position;
                    currentSide = 0;
                }
            }
        }
    }
}
