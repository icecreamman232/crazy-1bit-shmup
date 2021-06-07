using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class WormHoleController : EnvironmentWithCustomPath
{
    public float moveSpeed;

    public override void Setup()
    {
        base.Setup();
    }
    public override void Spawn()
    {
        base.Spawn();
        Move();
    }

    public override void Move()
    {
        base.Move();
        moveController.movementEndEvent -= OnMoveEnd;
        moveController.movementEndEvent += OnMoveEnd;
        moveController.pathContainer = introPath;
        moveController.loopType = splineMove.LoopType.none;
        moveController.moveToPath = false;
        moveController.StartMove();
    }



    private void DoVancumn()
    {
        //Will suck ship inside of this!
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Vancumn!");
            DoVancumn();
        }
    }
}
