using System.Collections;
using UnityEngine;

public class ReadyState : State
{
    SpaceShipController shipController;
    InputManager inputManager;
    public override void Init(GameManager _gameManager)
    {
        gameManager = _gameManager;
        inputManager = InputManager.Instance;
        inputManager.EnableInput();
        shipController = gameManager.spaceShip.GetComponent<SpaceShipController>();
    }

    public override void Update()
    {
        
        if (inputManager.firstKeyPressed)
        {

            UIManager.Instance.holdToPlayUI.Hide();

            gameManager.environmentCreator.Run();
            gameManager.waveSpawner.Run();
            gameManager.LifeTimeCounting();
            shipController.BeginShoot();

            gameManager.SetState(new PlayingState());

        }
    }
}
