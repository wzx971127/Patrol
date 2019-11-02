using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
    void movePlayer(float h, float v);
    void setDirection(float h, float v);
    bool GameOver();
}
