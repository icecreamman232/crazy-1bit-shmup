using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnvironment : MonoBehaviour
{
    public float lifeTime;

    public virtual void Setup()
    {

    }
    public virtual void Spawn()
    {

    }
    //Callback after environment entity is destroyed
    public System.Action OnDestroy;
}
