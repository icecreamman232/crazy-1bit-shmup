using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
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
    public List<GameObject> list_monster_lanes;
    #endregion

    public ParticleSystem star_front_layer;
    public ParticleSystem star_back_layer;

    public GameObject die_explosion_fx;
    public CameraShake camera_shake_fx;

    public GameObject left_barrel;
    public GameObject right_barrel;

    public AudioSource BGM;

    // Start is called before the first frame update
    void Start()
    {
        InitGameManager();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void InitGameManager()
    {
        current_coin = 0;
        coin_text.text = current_coin.ToString();
        current_score = 0;
        score_number_text.text = current_score.ToString();
        camera_shake_fx.Setup();

        left_barrel.transform.position = new Vector3(-GameHelper.SizeOfCamera().x/2-0.5f, 0, 0);
        left_barrel.GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);
        right_barrel.transform.position = new Vector3(GameHelper.SizeOfCamera().x/2+0.5f, 0, 0);
        right_barrel.GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);

        BGM.Play();
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
