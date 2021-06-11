using System.Collections;
using UnityEngine;

public class ReadyState : State
{
    private SpaceShipMovement shipMovementController; 
    public override void Init(GameManager _gameManager)
    {
        gameManager = _gameManager;
        shipMovementController = gameManager.spaceShip.GetComponent<SpaceShipMovement>();
    }

    public override void Update()
    {
        if (shipMovementController.firstTouch)
        {
            gameManager.environmentCreator.Run();
            gameManager.waveSpawner.Run();
            gameManager.LifeTimeCounting();
            gameManager.spaceShip.GetComponent<SpaceShipController>().BeginShoot();

            gameManager.SetState(new PlayingState());

        }
    }
}
