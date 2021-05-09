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
    public GameManagerState current_game_state;
    public void SetState(GameManagerState new_state)
    {
        current_game_state = new_state;
    }
    #endregion

    #region Score Management
    public TextMeshProUGUI score_number_text;
    public int current_score;

    #endregion

    #region Coin Management
    public int current_coin;
    //public TextMeshProUGUI coin_text;
    #endregion

    #region Level
    public List<GameObject> list_monster_lanes;
    public EndlessModeDO endless_mode_data;
    const float speed_factor = 0.08f;
    public int wave_index;
    public float current_spd;
    #endregion

    #region Sound & Music
    public AudioSource BGM;
    public AudioSource sfx;
    public AudioClip monster_die_sfx;
    #endregion

    #region Ref Holders
    public GameObject space_ship;
    public ParticleSystem star_front_layer;
    public ParticleSystem star_back_layer;
    public GameObject die_explosion_fx;
    public CameraShake camera_shake_fx;
    public GameObject left_barrel;
    public GameObject right_barrel;
    #endregion

    #region UI
    
    public UIEndGameCanvasController ui_endgame_controller;
    public UIHealthBarController ui_ship_health_bar;
    #endregion

    public RankManager rank_manager;
    // Start is called before the first frame update
    void Start()
    {
        SetState(GameManagerState.STANDBY);
    }

    // Update is called once per frame
    void Update()
    {
        switch (current_game_state)
        {
            case GameManagerState.STANDBY:
                InitGameManager();
                SetState(GameManagerState.IDLE);
                break;
            case GameManagerState.PLAYING:
                //Cheat to Death
                if(Input.GetKeyDown(KeyCode.Z))
                {
                    space_ship.GetComponent<SpaceShipController>().current_hp = 0;
                }
                if (space_ship.GetComponent<SpaceShipController>().isDied)
                {
                    ui_endgame_controller.gameObject.SetActive(true);
                    ui_endgame_controller.PlayIntro(current_score.ToString());
                    star_back_layer.Stop();
                    star_front_layer.Stop();
                    for (int i = 0; i < 5; i++)
                    {
                        list_monster_lanes[i].GetComponent<MonsterLaneController>().StopAllCoroutines();
                    }
                    DataManager.Instance.SaveDataToLocalStorage();
                    SetState(GameManagerState.LOSE);
                }

                break;
            case GameManagerState.LOSE:
                
                break;
            case GameManagerState.RESET:
                break;
            case GameManagerState.IDLE:
                if(space_ship.GetComponent<SpaceShipMovement>().firstTouch)
                {
                    StartGame();
                    SetState(GameManagerState.PLAYING);
                }
                break;
        }
    }


    public void OnStandBy()
    {
        ui_endgame_controller.PlayOuttro();
        SetState(GameManagerState.STANDBY);
    }
    void InitGameManager()
    {
        StopAllCoroutines();
        current_spd = 1;
        wave_index = 1;
        current_coin = 0;
        //coin_text.text = current_coin.ToString();
        current_score = 0;
        score_number_text.text = current_score.ToString();
        camera_shake_fx.Setup();

        left_barrel.transform.position = new Vector3(-GameHelper.SizeOfCamera().x/2-0.5f, 0, 0);
        left_barrel.GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);
        right_barrel.transform.position = new Vector3(GameHelper.SizeOfCamera().x/2+0.5f, 0, 0);
        right_barrel.GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);

        ui_ship_health_bar.HealthBarSetup();

        var main_front = star_front_layer.main;
        var main_back = star_back_layer.main;
        current_spd = GetCurrentLevelSpeed();
        main_front.simulationSpeed = current_spd;
        main_back.simulationSpeed = current_spd;

        star_back_layer.Play();
        star_front_layer.Play();
        
        BGM.Play();
        space_ship.GetComponent<SpaceShipController>().StartShip();

        rank_manager.Setup();
        current_game_state = GameManagerState.STANDBY;

    }
    void StartGame()
    {
        for (int i = 0; i < 5; i++)
        {
            list_monster_lanes[i].GetComponent<MonsterLaneController>().StartMonsterLane(endless_mode_data.min_delay_limit, endless_mode_data.max_delay_limit);
        }
        StartCoroutine(count_time());
        space_ship.GetComponent<SpaceShipController>().BeginShoot();
    }
    public void UpdateScore(int score)
    {
        current_score += score;
        score_number_text.text = current_score.ToString();
    }
    public void UpdateCoin(int value)
    {
        current_coin += value;
        //coin_text.text = current_coin.ToString();
    }
    public float GetCurrentLevelSpeed()
    {
        var min = endless_mode_data.min_speed;
        var max = endless_mode_data.max_speed;     
        return Mathf.Clamp(min + (max - min) * (Mathf.Exp(speed_factor * wave_index)),0,endless_mode_data.speed_limit);
    }
    IEnumerator count_time()
    {
        WaitForSeconds delay = new WaitForSeconds(2.0f);
        while(true)
        {
            yield return delay;
            wave_index += 1;
            for (int i = 0; i < 5; i++)
            {
                list_monster_lanes[i].GetComponent<MonsterLaneController>().UpdateSpawnRate(endless_mode_data.min_decrease_rate, endless_mode_data.max_decrease_rate);
            }
            var main_front = star_front_layer.main;
            var main_back = star_back_layer.main;
            current_spd = GetCurrentLevelSpeed();
            main_front.simulationSpeed = current_spd;
            main_back.simulationSpeed = current_spd;
        }
    }

    public void CreateDieFx(Vector3 position)
    {
        var obj = Lean.Pool.LeanPool.Spawn(die_explosion_fx, position, Quaternion.identity);
        obj.GetComponent<ParticleSystem>().Play();
        StartCoroutine(DespawnDieFX(obj));
    }
    IEnumerator DespawnDieFX(GameObject die_fx)
    {
        yield return new WaitUntil(() => die_fx.GetComponent<ParticleSystem>().isEmitting == false);
        Lean.Pool.LeanPool.Despawn(die_fx);
    }
}
