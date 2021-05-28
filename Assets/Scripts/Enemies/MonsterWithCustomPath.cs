using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class MonsterWithCustomPath : BaseMonster, IMovementWithCustomPath
{
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
        patrolType = splineMove.LoopType.pingPong;
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
        moveController.movementEndEvent += Patrol;
        moveController.StartMove();
    }

    public void Patrol()
    {
        if (patrolPath == null)
        {
            Debug.Log("On Finish In Patrol");
            OnFinishRun?.Invoke();
            return;
        }
        moveController.pathContainer = patrolPath;
        moveController.moveToPath = true;
        moveController.loopType = patrolType;
        moveController.movementEndEvent += PrepareToRetreat;
        moveController.StartMove();
    }
    void PrepareToRetreat()
    {
        StartCoroutine(OnRetreating());
    }
    IEnumerator OnRetreating()
    {
        yield return new WaitForSeconds(patrolDuration);
        Retreat();
    }
    public void Retreat()
    {
        if (retreatPath == null) return;
        moveController.moveToPath = true;
        moveController.pathContainer = retreatPath;
        moveController.movementEndEvent += OnMoveEnd;
        moveController.StartMove();
    }
    void OnMoveEnd()
    {
        Debug.Log("On Finish In Retreat");
        OnFinishRun?.Invoke();
    }
}
