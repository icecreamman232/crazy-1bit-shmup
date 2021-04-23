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
    [Header("GameState")]
    public GameManagerState current_game_state;
    public void SetState(GameManagerState new_state)
    {
        current_game_state = new_state;
    }
    #endregion

    #region Score Management
    [Header("Score Management")]
    public TextMeshProUGUI score_number_text;
    public int current_score;

    #endregion

    #region Coin Management
    [Header("Coin Management")]
    public int current_coin;
    public TextMeshProUGUI coin_text;
    #endregion

    #region Level
    [Header("Levels")]
    public List<GameObject> list_monster_lanes;
    public EndlessModeDO endless_mode_data;
    public const float speed_factor = 0.08f;
    public int wave_index;
    public float current_spd;
    #endregion

    #region Sound & Music
    public AudioSource BGM;
    #endregion




    #region Ref Holders
    [Header("Ref Holders")]
    public ParticleSystem star_front_layer;
    public ParticleSystem star_back_layer;

    public GameObject die_explosion_fx;
    public CameraShake camera_shake_fx;

    public GameObject left_barrel;
    public GameObject right_barrel;

    #endregion


    

    // Start is called before the first frame update
    void Start()
    {
        SetState(GameManagerState.STANDBY);
        //InitGameManager();
    }

    // Update is called once per frame
    void Update()
    {
        switch(current_game_state)
        {
            case GameManagerState.STANDBY:
                InitGameManager();
                SetState(GameManagerState.PLAYING);
                break;
            case GameManagerState.PLAYING:
                break;
            case GameManagerState.LOSE:
                break;
            case GameManagerState.RESET:
                break;
            case GameManagerState.IDLE:
                break;
        }
    }
    void InitGameManager()
    {

        StopAllCoroutines();
        current_spd = 1;
        wave_index = 1;
        current_coin = 0;
        coin_text.text = current_coin.ToString();
        current_score = 0;
        score_number_text.text = current_score.ToString();
        camera_shake_fx.Setup();

        left_barrel.transform.position = new Vector3(-GameHelper.SizeOfCamera().x/2-0.5f, 0, 0);
        left_barrel.GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);
        right_barrel.transform.position = new Vector3(GameHelper.SizeOfCamera().x/2+0.5f, 0, 0);
        right_barrel.GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);


        StartCoroutine(count_time());
        BGM.Play();

        current_game_state = GameManagerState.STANDBY;

    }
    public void UpdateScore(int score)
    {
        current_score += score;
        score_number_text.GetComponent<Animator>().Play("score_increase_anim");
        score_number_text.text = current_score.ToString();
    }
    public void UpdateCoin(int value)
    {
        current_coin += value;
        coin_text.text = current_coin.ToString();
    }
    public float GetCurrentLevelSpeed(int index)
    {
        var min = endless_mode_data.min_speed;
        var max = endless_mode_data.max_speed;
        
        var result = min + (max - min) * (Mathf.Exp(speed_factor * index));
        Debug.Log("Result=" + result);
        return result;
    }
    IEnumerator count_time()
    {
        while(true)
        {
            yield return new WaitForSeconds(2.0f);
            Debug.Log("Increase spd");
            wave_index += 1;
            var main_front = star_front_layer.main;
            var main_back = star_back_layer.main;
            current_spd = GetCurrentLevelSpeed(wave_index);
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
