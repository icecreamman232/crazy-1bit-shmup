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

    public void MoveSafelyToShip()
    {
        //For case that Monster have no idea what it should goes then just aim to player position

        introPath.waypoints[0].position = transform.position;
        introPath.waypoints[introPath.waypoints.Length - 1].position = GameManager.Instance.spaceShip.transform.position;

        moveController.movementEndEvent -= OnMoveEnd;
        moveController.movementEndEvent += OnMoveEnd;
        moveController.pathContainer = introPath;
        moveController.moveToPath = false;
        moveController.loopType = splineMove.LoopType.none;
        moveController.StartMove();
        currentMovementState = MovementState.INTRO;
    }

}
