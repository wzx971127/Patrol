using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.myspace;

public class SceneController : MonoBehaviour, ISceneController, IUserAction, IScore, Handle
{

    public GameObject player;

    private SSDirector director;
    private bool canOperation;
    private bool create;
    private int bearNum;
    private int ellephantNum;
    private Subject sub;
    private Animator ani;

    private Vector3 movement;   // The vector to store the direction of the player's movement.

    void Awake()
    {
        director = SSDirector.getInstance();
        sub = player.GetComponent<Player>();
        ani = player.GetComponent<Animator>();
        director.currentScene = this;
        director.currentScene.LoadResources();
        director.currentScene.CreatePatrols();
        Handle sc = director.currentScene as Handle;
        sub.Attach(sc);
        GetComponent<ScoreManager>().resetScore();
        bearNum = 0;
        ellephantNum = 0;
        create = false;
    }

    void Update()
    {
        int score = GetComponent<ScoreManager>().getScore();
        if (score % 10 == 0)
        {
            director.currentScene.CreateMore();
        }
        else
        {
            create = true;
        }
    }

    #region ISceneController
    public void LoadResources()
    {
        GameObject Environment = Instantiate<GameObject>(
            Resources.Load<GameObject>("Prefabs/Environment"));
        Environment.name = "Environment";
    }

    public void CreatePatrols()
    {      //创建游戏开始时的巡逻兵
        PatrolFactory pf = PatrolFactory.getInstance();
        for (int i = 1; i <= 12; i++)
        {
            GameObject patrol = pf.getPatrol();
            patrol.name = "ZomBear" + ++bearNum;
            Handle p = patrol.GetComponent<Patrol>();
            sub.Attach(p);
            patrol.GetComponent<Patrol>().register(GetComponent<ScoreManager>().addScore);
        }
    }

    public void CreateMore()
    {     //每增加十分，创建新的巡逻兵
        if (create)
        {
            PatrolFactory pf = PatrolFactory.getInstance();
            for (int i = 1; i <= 3; i++)
            {
                GameObject patrol = pf.getPatrol();
                patrol.name = "ZomBear" + ++ellephantNum;
                Handle p = patrol.GetComponent<Patrol>();
                sub.Attach(p);
                patrol.GetComponent<Patrol>().register(GetComponent<ScoreManager>().addScore);
            }
            for (int i = 1; i <= 3; i++)
            {
                GameObject patrolplus = pf.getPatrolPlus();
                patrolplus.name = "Zombunny" + ++bearNum;
                Handle p = patrolplus.GetComponent<Patrol>();
                sub.Attach(p);
                patrolplus.GetComponent<Patrol>().register(GetComponent<ScoreManager>().addScore);
            }
            create = false;
        }
    }
    #endregion

    #region IUserAction
    public void movePlayer(float h, float v)
    {
        if (canOperation)
        {
            player.GetComponent<Player>().move(h, v);
            if (h == 0 && v == 0)
            {
                ani.SetTrigger("stop");
            }
            else
            {
                ani.SetTrigger("move");
            }
        }
    }

    public void setDirection(float h, float v)
    {
        if (canOperation)
        {
            player.GetComponent<Player>().turn(h, v);
        }
    }

    public bool GameOver()
    {
        return (!canOperation);
    }
    #endregion

    #region ISceneController
    public int currentScore()
    {
        return GetComponent<ScoreManager>().getScore();
    }
    #endregion

    #region Handele
    public void Reaction(bool isLive, Vector3 pos)
    {
        ani.SetBool("live", isLive);
        canOperation = isLive;
    }
    #endregion
}
