using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public enum MovementState
{
    INTRO   = 0,
    PATROL  = 1,
    RETREAT = 2,
}

public class MonsterWithCustomPath : BaseMonster, IMovementWithCustomPath
{
    public MovementState currentMovementState;

    [SerializeField]
    protected PathManager introPath;
    [SerializeField]
    protected PathManager patrolPath;
    [SerializeField]
    protected PathManager retreatPath;

    public PathManager IntroPath
    {
        get
        {
            return introPath;
        }
        set
        {
            introPath = value;
        }
    }
    public PathManager PatrolPath
    {
        get
        {
            return patrolPath;
        }
        set
        {
            patrolPath = value;
        }
    }
    public PathManager RetreatPath
    {
        get
        {
            return retreatPath;
        }
        set
        {
            retreatPath = value;
        }
    }


    public float patrolDuration;

    public System.Action OnFinishRun;

    public splineMove moveController;

    private void Start()
    {

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
    public virtual void OnMoveEnd()
    {
        OnFinishRun?.Invoke();
    }
}
