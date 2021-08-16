using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIEndGameCanvasController: BaseUIController
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

    public void SetupUIEndGame()
    {
        score_bg.rectTransform.sizeDelta = new Vector2(Screen.width, 150);
        retry_btn.SetActive(false);
        menu_btn.SetActive(false);
    }
    public override void Show()
    {
        gameObject.SetActive(true);
        SetupUIEndGame();
        endgame_animator.Play("EndGame_intro_anim");
        score_text.text = GameManager.Instance.currentScore.ToString();
    }
    public override void Hide()
    {
        gameObject.SetActive(false);
    }

    //Do not remove this function. It is used by animation
    void OpenEndGameButtons()
    {
        retry_btn.SetActive(true);
        menu_btn.SetActive(true);
    }
    
}
