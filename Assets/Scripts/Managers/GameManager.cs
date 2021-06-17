﻿using System.Collections;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    #region Fields
    public State currentState;
    public int currentWaveIndex;
    public float currentSpeed;
    public int currentCoin;
    public int currentScore;
    #endregion

    #region Reference
    public GameObject spaceShip;
    public WaveSpawner waveSpawner;
    public EnvironmentCreator environmentCreator;
    public ParticleSystem starFrontLayer;
    public ParticleSystem starBackLayer;
    
    public RankManager rankManager;

    public AudioSource BGM;
    public AudioSource sfx;
    public AudioClip monster_die_sfx;

    public UIEndGameCanvasController uiEndgameCanvas;
    public UIHealthBarController uiShipHPBar;
    public TextMeshProUGUI scoreNumberText;
    public EndlessModeDO endlessModeData;
    #endregion


    #region FX
    [Header("FX")]
    public GameObject dieExplosionFX;
    public CameraShake cameraShakeFX;
    public GameObject explosionTransformFX;
    #endregion


    private const float speedFactor = 0.08f;

    private void Start()
    {
        SetState(new StandbyState());
    }

    private void Update()
    {
        currentState.Update();
    }
    public void SetState(State nextState)
    {
        currentState = nextState;
        currentState.Init(this);
    }
    public void Init()
    {
        SetupLevel();
        SetupCoin();
        SetupScore();
        cameraShakeFX.Setup();
        uiShipHPBar.HealthBarSetup();
        SetupParticleBackground();
        BGM.Play();
        spaceShip.GetComponent<SpaceShipController>().StartShip();
        rankManager.Setup();
        waveSpawner.Setup();
        environmentCreator.Setup();
    }
    public void OnClickReset()
    {
        uiEndgameCanvas.PlayOuttro();
        SetState(new StandbyState());
    }
    private void SetupLevel()
    {
        currentSpeed = 1;
        currentWaveIndex = 1;
    }
    private void SetupCoin()
    {
        //Data
        currentCoin = 0;

        //UI
        //coin_text.text = current_coin.ToString();
    }
    private void SetupScore()
    {
        //Data
        currentScore = 0;

        //UI
        scoreNumberText.text = currentScore.ToString();
    }
    public void UpdateCoin(int value)
    {
        currentCoin += value;
        //coin_text.text = current_coin.ToString();
    }
    public void UpdateScore(int score)
    {
        currentScore += score;
        scoreNumberText.text = currentScore.ToString();
    }
    private void SetupParticleBackground()
    {
        var front = starFrontLayer.main;
        var back = starBackLayer.main;
        currentSpeed = GetCurrentLevelSpeed();
        front.simulationSpeed = currentSpeed;
        back.simulationSpeed = currentSpeed;

        starBackLayer.Play();
        starFrontLayer.Play();
    }
    public float GetCurrentLevelSpeed()
    {
        var min = endlessModeData.minSpeed;
        var max = endlessModeData.maxSpeed;
        return Mathf.Clamp(min + (max - min) * (Mathf.Exp(speedFactor * currentWaveIndex)), 0, endlessModeData.speedLimit);
    }
    public void LifeTimeCounting()
    {
        StartCoroutine(OnLifeTimeCounter());
    }
    private IEnumerator OnLifeTimeCounter()
    {
        WaitForSeconds delay = new WaitForSeconds(2.0f);
        while (true)
        {
            yield return delay;
            currentWaveIndex += 1;
            var front = starFrontLayer.main;
            var back = starBackLayer.main;
            currentSpeed = GetCurrentLevelSpeed();
            front.simulationSpeed = currentSpeed;
            back.simulationSpeed = currentSpeed;
        }
    }
    public void CreateDieFx(Vector3 position)
    {
        var obj = Lean.Pool.LeanPool.Spawn(dieExplosionFX, position, Quaternion.identity);
        obj.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DespawnDieFX(obj));
    }
    private IEnumerator DespawnDieFX(GameObject dieFX)
    {
        yield return new WaitUntil(() => dieFX.GetComponent<ParticleSystem>().isEmitting == false);
        Lean.Pool.LeanPool.Despawn(dieFX);
    }
}
