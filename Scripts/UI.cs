using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

    private IUserAction action;
    private IScore score;

    public Transform player;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.

    public Text s;
    public Text gg;
    public Button re;

    Vector3 offset;                     // The initial offset from the player.

    // Use this for initialization
    void Start()
    {
        action = SSDirector.getInstance().currentScene as IUserAction;
        score = SSDirector.getInstance().currentScene as IScore;
        // Calculate the initial offset.
        offset = transform.position - player.position;

        re.gameObject.SetActive(false);
        Button btn = re.GetComponent<Button>();
        btn.onClick.AddListener(restart);
    }

    void Update()
    {
        // Create a postion the camera is aiming for based on the offset from the player.
        Vector3 playerCamPos = player.position + offset;
        // Smoothly interpolate between the camera's current position and it's player position.
        transform.position = Vector3.Lerp(transform.position, playerCamPos, smoothing * Time.deltaTime);

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        move(h, v);
        turn(h, v);
        showScore();
        gameOver();
    }

    //移动玩家
    public void move(float h, float v)
    {
        action.movePlayer(h, v);
    }

    //使玩家面向移动方向
    public void turn(float h, float v)
    {
        if (h != 0 || v != 0)
        {
            action.setDirection(h, v);
        }
    }

    //显示分数
    public void showScore()
    {
        s.text = "Score : " + score.currentScore();
    }

    //游戏结束
    public void gameOver()
    {
        if (action.GameOver())
        {
            if (!re.isActiveAndEnabled)
            {
                re.gameObject.SetActive(true);
            }
            gg.text = "Game Over!";
        }
    }

    //重新开始
    public void restart()
    {
        SceneManager.LoadScene("main");
    }
}
