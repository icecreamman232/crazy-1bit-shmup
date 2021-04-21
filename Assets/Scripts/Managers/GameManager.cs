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
    public TextMeshProUGUI score_number_text;
    public int current_score;

    #endregion

    public ParticleSystem star_front_layer;
    public ParticleSystem star_back_layer;

    public GameObject die_explosion_fx;
    public CameraShake camera_shake_fx;


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
        current_score = 0;
        camera_shake_fx.Setup();
    }
    public void UpdateScore(int score)
    {
        current_score += score;
        score_number_text.GetComponent<Animator>().Play("score_increase_anim");
        score_number_text.text = current_score.ToString();
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
