using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.myspace
{

    public class PatrolFactory : System.Object
    {

        private static PatrolFactory instance;

        public static PatrolFactory getInstance()
        {
            if (instance == null)
            {
                instance = new PatrolFactory();
            }
            return instance;
        }

        public GameObject getPatrol()
        {
            GameObject patrol = GameObject.Instantiate<GameObject>(
                    Resources.Load<GameObject>("Prefabs/ZomBear")); ;
            return patrol;
        }

        public GameObject getPatrolPlus()
        {
            GameObject patrolplus = GameObject.Instantiate<GameObject>(
                Resources.Load<GameObject>("Prefabs/Zombunny")); ;
            return patrolplus;
        }

        public void freePatrol(GameObject p)
        {
            p.SetActive(false);
        }
    }
}
