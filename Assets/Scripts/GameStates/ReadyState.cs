using System.Collections;
using UnityEngine;

public class ReadyState : State
{
    SpaceShipController shipController;
    public override void Init(GameManager _gameManager)
    {
        gameManager = _gameManager;
        shipController = gameManager.spaceShip.GetComponent<SpaceShipController>();
    }

    public override void Update()
    {
        if (shipController.firstTouch)
        {
            gameManager.environmentCreator.Run();
            gameManager.waveSpawner.Run();
            gameManager.LifeTimeCounting();
            shipController.BeginShoot();

            gameManager.SetState(new PlayingState());

        }
    }
}
