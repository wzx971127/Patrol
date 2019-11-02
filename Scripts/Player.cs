using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Subject
{

    private bool isLive;
    private Vector3 position;
    private float speed;

    Vector3 movement;   // The vector to store the direction of the player's movement.

    protected List<Handle> handles = new List<Handle>();   //所有观察者

    // Use this for initialization
    void Start()
    {
        isLive = true;
        speed = 8.0f;
    }

    public override void Attach(Handle h)
    {
        handles.Add(h);
    }

    public override void Detach(Handle h)
    {
        handles.Remove(h);
    }

    public override void Notify(bool live, Vector3 pos)
    {
        foreach (Handle h in handles)
        {
            h.Reaction(live, pos);
        }
    }

    //玩家碰到巡逻兵，就死亡
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "patrol")
        {
            isLive = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        Notify(isLive, position);
    }

    public void move(float h, float v)
    {
        if (isLive)
        {
            // Set the movement vector based on the axis input.
            movement.Set(h, 0f, v);
            // Normalise the movement vector and make it proportional to the speed per second.
            movement = movement.normalized * speed * Time.deltaTime;
            // Move the player to it's current position plus the movement.
            GetComponent<Rigidbody>().MovePosition(transform.position + movement);
        }
    }

    public void turn(float h, float v)
    {
        if (isLive)
        {
            // Set the movement vector based on the axis input.
            movement.Set(h, 0f, v);
            Quaternion rot = Quaternion.LookRotation(movement);
            // Set the player's rotation to this new rotation.
            GetComponent<Rigidbody>().rotation = rot;
        }
    }
}
