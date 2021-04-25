using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreNumberCounter : MonoBehaviour
{
    //Score need to count to
    public int target_score;
    public int current_score;
    public TextMeshProUGUI score_text;

    //second
    public float duration;

    //The more score the faster counter count
    private int count_increase_factor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartCounting(int target)
    {
        current_score = 0;
        target_score = target;
        count_increase_factor = Mathf.RoundToInt(target/(duration*60));
        score_text.text = current_score.ToString();
        StartCoroutine(CountScore());
    }
    IEnumerator CountScore()
    {   
        while(true)
        {
            current_score += count_increase_factor;
            score_text.text = current_score.ToString();
            yield return new WaitForSeconds(0.005f);
            yield return new WaitUntil(() => current_score < target_score);
        }
    }
}
