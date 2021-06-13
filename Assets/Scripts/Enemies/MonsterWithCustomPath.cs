using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public enum MovementState
{
    STOP    = -1,
    INTRO   = 0,
    PATROL  = 1,
    RETREAT = 2,
}

public class MonsterWithCustomPath : BaseMonster
{
    public MovementState currentMovementState;

    [SerializeField]
    protected PathManager introPath;

    public float patrolDuration;
    [SerializeField]
    protected PathManager patrolPath;

    [SerializeField]
    protected PathManager retreatPath;
    public System.Action OnFinishRun;

    public splineMove moveController;

    public override void Setup()
    {
        base.Setup();

        //Could change to currentMoveSpeed which grow faster by time
        moveController.speed = baseMoveSpeed;
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
        moveController.Stop();
        moveController.movementEndEvent -= OnMoveEnd;
        moveController.pathContainer = null;
        currentMovementState = MovementState.STOP;
    }
    public virtual void OnMoveEnd()
    {
        OnFinishRun?.Invoke();
    }
}
