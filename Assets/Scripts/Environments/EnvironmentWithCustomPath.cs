using SWS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentWithCustomPath : BaseEnvironment
{
   
    [SerializeField]
    protected PathSegment intro;

    [SerializeField]
    protected PathSegment patrol;

    [SerializeField]
    protected PathSegment retreat;

    public BezierMoveController bezierMoveController;

    public override void Setup()
    {
        base.Setup();
        bezierMoveController.MoveSpeed = baseMoveSpeed;
    }

    public virtual void Move()
    {
        
    }

    public virtual void Patrol()
    {
        
    }

    public virtual void Retreat()
    {
        
    }
    public virtual void Stop()
    {

    }
    public virtual void OnMoveEnd()
    {
        //Entity finished its run and ready to be removed
        OnDestroy?.Invoke();
    }
}
