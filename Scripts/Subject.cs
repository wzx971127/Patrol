using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{

    protected List<Handle> listeners = new List<Handle>();

    public abstract void Attach(Handle listener);

    public abstract void Detach(Handle listener);

    public abstract void Notify(bool live, Vector3 pos);
}
