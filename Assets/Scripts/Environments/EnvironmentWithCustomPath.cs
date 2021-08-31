using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentWithCustomPath : BaseEnvironment
{
   
    [SerializeField] protected PathSegment intro;
    public PathSegment IntroPath
    {
        get
        {
            return intro;
        }
    }
    [SerializeField] protected PathSegment patrol;
    public PathSegment PatrolPath
    {
        get
        {
            return patrol;
        }
    }
    [SerializeField] protected PathSegment retreat;
    public PathSegment RetreatPath
    {
        get
        {
            return retreat;
        }
    }

    public BezierMoveController bezierMoveController;
    public System.Action OnFinishRun;
    public override void Setup()
    {
        base.Setup();
        bezierMoveController.MoveSpeed = data.baseMoveSpd;
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
        OnFinishRun?.Invoke();
    }
}
