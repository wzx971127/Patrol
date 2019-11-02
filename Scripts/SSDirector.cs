using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDirector : System.Object
{
    //singleton instance
    private static SSDirector instance;

    public ISceneController currentScene;
    public bool running
    {
        get;
        set;
    }

    public static SSDirector getInstance()
    {
        if (instance == null)
        {
            instance = new SSDirector();
        }
        return instance;
    }
}
