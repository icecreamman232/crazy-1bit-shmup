using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameManagerState
{
    STANDBY = 0,
    PLAYING = 1,
    LOSE    = 2,
    RESET   = 3,
    IDLE    = -1,
}


public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    #region Game State
    public GameManagerState currentGameState;
    public void SetState(GameManagerState newState)
    {
        currentGameState = newState;
    }
    #endregion

    #region Score Management
    public TextMeshProUGUI scoreNumberText;
    public int currentScore;

    #endregion

    #region Coin Management
    public int currentCoin;
    #endregion

    #region Level
    public EndlessModeDO endlessModeData;
    private const float speedFactor = 0.08f;
    public int currentWaveIndex;
    public float currentSpeed;
    #endregion

    #region Sound & Music
    public AudioSource BGM;
    public AudioSource sfx;
    public AudioClip monster_die_sfx;
    #endregion

    #region Ref Holders
    public GameObject spaceShip;
    public ParticleSystem starFrontLayer;
    public ParticleSystem starBackLayer;
    public GameObject dieExplosionFX;
    public CameraShake cameraShakeFX;
    public GameObject leftBarrel;
    public GameObject rightBarrel;
    #endregion

    #region UI
    
    public UIEndGameCanvasController uiEndgameCanvas;
    public UIHealthBarController uiShipHPBar;
    #endregion

    public RankManager rankManager;

    public WaveSpawner waveSpawner;

    // Start is called before the first frame update
    void Start()
    {
        SetState(GameManagerState.STANDBY);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentGameState)
        {
            case GameManagerState.STANDBY:
                InitGameManager();
                SetState(GameManagerState.IDLE);
                break;
            case GameManagerState.PLAYING:
                //Cheat to Death
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    spaceShip.GetComponent<SpaceShipController>().currentHP = 0;
                }
                if (spaceShip.GetComponent<SpaceShipController>().isDied)
                {
                    uiEndgameCanvas.gameObject.SetActive(true);
                    uiEndgameCanvas.PlayIntro(currentScore.ToString());
                    starBackLayer.Stop();
                    starFrontLayer.Stop();

                    waveSpawner.Reset();
                    DataManager.Instance.SaveDataToLocalStorage();
                    SetState(GameManagerState.LOSE);
                }
                break;
            case GameManagerState.LOSE:               
                break;
            case GameManagerState.RESET:
                break;
            case GameManagerState.IDLE:
                if(spaceShip.GetComponent<SpaceShipMovement>().firstTouch)
                {
                    StartGame();
                    SetState(GameManagerState.PLAYING);
                }
                break;
        }
    }
    public void OnStandBy()
    {
        uiEndgameCanvas.PlayOuttro();
        SetState(GameManagerState.STANDBY);
    }
    void InitGameManager()
    {
        StopAllCoroutines();
        SetupLevel();
        SetupCoin();
        SetupScore();
        cameraShakeFX.Setup();
        SetupBarrels();
        uiShipHPBar.HealthBarSetup();
        SetupParticleBackground();
        BGM.Play();
        spaceShip.GetComponent<SpaceShipController>().StartShip();
        rankManager.Setup();
        waveSpawner.Setup();

        currentGameState = GameManagerState.STANDBY;
    }
    private void StartGame()
    {
        waveSpawner.Run();
        StartCoroutine(OnLifeTimeCounter());
        spaceShip.GetComponent<SpaceShipController>().BeginShoot();
    }
    #region Setup Methods
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
    private void SetupBarrels()
    {
        //This barrels would prevent item, coin, monster.etc.. from going out of screen
        leftBarrel.transform.position = new Vector3(-GameHelper.SizeOfCamera().x / 2 - 0.5f, 0, 0);
        leftBarrel.GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);
        rightBarrel.transform.position = new Vector3(GameHelper.SizeOfCamera().x / 2 + 0.5f, 0, 0);
        rightBarrel.GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);
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
    #endregion

    #region Update Methods
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
    #endregion


    
    public float GetCurrentLevelSpeed()
    {
        var min = endlessModeData.min_speed;
        var max = endlessModeData.max_speed;     
        return Mathf.Clamp(min + (max - min) * (Mathf.Exp(speedFactor * currentWaveIndex)),0,endlessModeData.speed_limit);
    }
    private IEnumerator OnLifeTimeCounter()
    {
        WaitForSeconds delay = new WaitForSeconds(2.0f);
        while(true)
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
