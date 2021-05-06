using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEndGameCanvasController: MonoBehaviour
{
    public Image score_bg;
    public TextMeshProUGUI score_text;
    public GameObject retry_btn;
    public GameObject menu_btn;

    public Animator endgame_animator;
    // Start is called before the first frame update
    void Start()
    {
        SetupUIEndGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetupUIEndGame()
    {
        score_bg.rectTransform.sizeDelta = new Vector2(Screen.width, 150);
        retry_btn.SetActive(false);
        menu_btn.SetActive(false);
    }
    public void PlayIntro(string score)
    {
        SetupUIEndGame();
        endgame_animator.Play("EndGame_intro_anim");
        score_text.text = score;
    }
    public void PlayOuttro()
    {
        gameObject.SetActive(false);
    }
    void OpenEndGameButtons()
    {
        retry_btn.SetActive(true);
        menu_btn.SetActive(true);
    }
}
