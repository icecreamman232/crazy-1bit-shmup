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
    private PathManager introPath;
    [SerializeField]
    private PathManager patrolPath;
    [SerializeField]
    private PathManager retreatPath;

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

    public SWS.splineMove.LoopType patrolType;

    public System.Action OnFinishRun;

    public splineMove moveController;

    private void Start()
    {
        //Default loop type that made sense!
        patrolType = splineMove.LoopType.yoyo;
    }

    public override void Run()
    {
        base.Run();
        Move();
    }


    public void Move()
    {
        moveController.pathContainer = introPath;
        moveController.moveToPath = false;
        moveController.loopType = splineMove.LoopType.none;
        moveController.movementEndEvent -= OnMoveEnd;
        moveController.movementEndEvent += Patrol;
        moveController.StartMove();
        currentMovementState = MovementState.INTRO;
    }

    public void Patrol()
    {
        //if (patrolPath == null)
        //{
        //    OnFinishRun?.Invoke();
        //    return;
        //}
        PrepareToRetreat();
        currentMovementState = MovementState.PATROL;
        if (patrolPath == null)
        {
            return;
        }
        moveController.pathContainer = patrolPath;
        moveController.moveToPath = true;
        moveController.loopType = splineMove.LoopType.yoyo;
        moveController.StartMove();
    }
    void PrepareToRetreat()
    {
        moveController.movementEndEvent -= Patrol;
        if (this.gameObject == null)
        {
            OnFinishRun?.Invoke();
            return;
        }
        StartCoroutine(OnRetreating());
    }
    IEnumerator OnRetreating()
    {
        yield return new WaitForSeconds(patrolDuration);
        Retreat();
    }
    public void Retreat()
    {
        if (retreatPath == null)
        {
            OnFinishRun?.Invoke();
            return;

        }
        moveController.moveToPath = true;
        moveController.pathContainer = retreatPath;
        moveController.loopType = splineMove.LoopType.none;
        moveController.movementEndEvent += OnMoveEnd;
        moveController.StartMove();
        currentMovementState = MovementState.RETREAT;
    }
    void OnMoveEnd()
    {
        OnFinishRun?.Invoke();
    }
}
