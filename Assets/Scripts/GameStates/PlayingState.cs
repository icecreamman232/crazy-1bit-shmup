using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState: State
{
    private SpaceShipController     ship;
    private EnvironmentCreator      environmentCreator;
    private WaveSpawner             waveSpawner;


    public override void Init(GameManager _gameManager)
    {
        gameManager = _gameManager;
        ship = _gameManager.spaceShip.GetComponent<SpaceShipController>();
        environmentCreator = _gameManager.environmentCreator;
        waveSpawner = _gameManager.waveSpawner;
    }

    public override void Update()
    {

#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Z))
        {
            ship.currentHP = 0;
        }
#endif
        if(ship.isDied)
        {
            gameManager.uiEndgameCanvas.gameObject.SetActive(true);
            gameManager.uiEndgameCanvas.PlayIntro(gameManager.currentScore.ToString());
            gameManager.starBackLayer.Stop();
            gameManager.starFrontLayer.Stop();

            //Reset stuffs
            environmentCreator.Reset();
            waveSpawner.Reset();


            //Set next state
            gameManager.SetState(new LosingState());
            
        }
    }
    private void UpdateUI()
    {

    }

}
