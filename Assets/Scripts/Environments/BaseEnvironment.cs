using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnvironment : BaseEntity
{
    public override void Setup()
    {
        currentHP = baseHP;
    }
    public override void Spawn()
    {
        
    }

    //Callback after environment entity is destroyed
    public System.Action OnDestroy;
}
