using SWS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster03Controller : BaseMonster, IMovementWithCustomPath
{
    public MoveToTargetComponent moveToComponent;

    private void Start()
    {
        //moveToComponent.SetStart();
    }
    public PathManager IntroPath 
    { 
        get => throw new System.NotImplementedException(); 
        set => throw new System.NotImplementedException(); 
    }
    public PathManager PatrolPath 
    { 
        get => throw new System.NotImplementedException(); 
        set => throw new System.NotImplementedException(); 
    }
    public PathManager RetreatPath 
    { 
        get => throw new System.NotImplementedException(); 
        set => throw new System.NotImplementedException(); 
    }

    public override void Setup()
    {
        //base.Run();
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }

    public void Patrol()
    {
        throw new System.NotImplementedException();
    }

    public void Retreat()
    {
        throw new System.NotImplementedException();
    }
}
