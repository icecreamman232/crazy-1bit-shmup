using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class FlyingSlimeController : MonsterWithCustomPath
{
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
        
        //Remember to unsubribe event before destroy something
        bezierMoveController.OnMoveEnd -= OnMoveEnd;
        bezierMoveController.OnMoveEnd += OnMoveEnd;
        bezierMoveController.SetPath(intro);
        bezierMoveController.Stop();
        bezierMoveController.StartMove(LoopType.None);

        currentMovementState = MovementState.INTRO;
    }
}
