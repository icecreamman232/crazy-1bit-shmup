using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UINumberCounter : MonoBehaviour
{
    //Score need to count to
    public int target_number;
    public int current_number;
    public TextMeshProUGUI text;

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
        current_number = 0;
        target_number = target;
        count_increase_factor = Mathf.RoundToInt(target/(duration*60));
        text.text = current_number.ToString();
        StartCoroutine(CountScore());
    }
    IEnumerator CountScore()
    {   
        while(true)
        {
            current_number += count_increase_factor;
            text.text = current_number.ToString();
            yield return new WaitForSeconds(0.005f);
            yield return new WaitUntil(() => current_number < target_number);
        }
    }
}
