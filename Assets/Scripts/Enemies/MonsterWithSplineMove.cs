using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;


public class MonsterWithSplineMove : BaseMonster
{
    public splineMove splineMove;

    public splineMove retreatMove;

    public float patrolTime;

    public System.Action OnFinishedRun;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void Run()
    {
        base.Run();
        splineMove.StartMove();
        if (retreatMove == null) return;
    }
}
