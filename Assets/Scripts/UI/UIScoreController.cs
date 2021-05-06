using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreController : MonoBehaviour
{
    public TextMeshProUGUI score_text;
    public UINumberCounter number_counter_controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setup(int score)
    {
        score_text.text = score.ToString();
    }
    public void UpdateUIScore(int last_score,int new_score)
    {
        //score_text.text = new_score.ToString();
        number_counter_controller.StartCounting(last_score,new_score);
    }
}
