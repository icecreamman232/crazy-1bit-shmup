using System.Collections;
using UnityEngine;


public class StandbyState : State
{
    private bool isInitDone;
    public override void Init(GameManager _gameManager)
    {
        gameManager = _gameManager;          
        isInitDone = true;     
    }

    public override void Update()
    {
        if(isInitDone)
        {
            gameManager.Init();
            InputManager.Instance.firstKeyPressed = false;
            gameManager.SetState(new ReadyState());
        }
    }
}
