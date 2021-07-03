using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMovingObject : MonoBehaviour
{
    public BezierMoveController moveController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            moveController.StartMove(LoopType.PingPong);
        }
    }
}
