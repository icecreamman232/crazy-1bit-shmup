﻿using System.Collections;
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
        moveController.movementEndEvent -= OnMoveEnd;
        moveController.movementEndEvent += OnMoveEnd;
        moveController.pathContainer = introPath;
        moveController.moveToPath = false;
        moveController.loopType = splineMove.LoopType.none;
        moveController.StartMove();
        currentMovementState = MovementState.INTRO;
    }
}