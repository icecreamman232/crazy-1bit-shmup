using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovementState
{
    STOP    = -1,
    INTRO   = 0,
    PATROL  = 1,
    RETREAT = 2,
}

public class MonsterWithCustomPath : BaseMonster
{
    [Header("Paths")]
    public MovementState currentMovementState;


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
    public float patrolDuration;
    public override void Setup()
    {
        base.Setup();

        //Could change to currentMoveSpeed which grow faster by time
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
        bezierMoveController.Stop();
        bezierMoveController.OnMoveEnd -= OnMoveEnd;

        currentMovementState = MovementState.STOP;
    }
    public virtual void OnMoveEnd()
    {
        OnFinishRun?.Invoke();
    }
}
