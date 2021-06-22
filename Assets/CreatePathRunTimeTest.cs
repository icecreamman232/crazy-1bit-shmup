using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;

public class CreatePathRunTimeTest : MonoBehaviour
{
    public BezierMoveController bezierMove;
    private void Start()
    {
        bezierMove.OnMoveEnd += End;
    }
    private void End()
    {
        Debug.Log("End");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            bezierMove.StartMove(LoopType.Loop);
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
