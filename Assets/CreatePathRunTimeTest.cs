using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePathRunTimeTest : MonoBehaviour
{
    public PathSegment patrolPath;
    public BezierMoveController bezierMove;
    private void Start()
    {
        bezierMove.OnMoveEnd += End;
    }
    private void End()
    {
        bezierMove.SetPath(patrolPath);
        bezierMove.Stop();
        bezierMove.OnMoveEnd -= End;
        bezierMove.StartMove(LoopType.PingPong);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            bezierMove.StartMove(LoopType.None);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            bezierMove.Stop();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            bezierMove.Pause();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            bezierMove.Resume();
        }

    }
}
