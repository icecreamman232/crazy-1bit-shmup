using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class UINumberCounter : MonoBehaviour
{
    //Score need to count to
    public int target_number;
    public int current_number;
    public TextMeshProUGUI text;
    /// <summary>
    /// Gọi vào function này khi kết thúc đếm số
    /// </summary>
    public UnityEvent callback_function; 


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
    public void StartCounting(int current, int target)
    {
        current_number = current;
        target_number = target;
        count_increase_factor = Mathf.RoundToInt(target/(duration*60));
        if(count_increase_factor <=0)
        {
            count_increase_factor = 1;
        }
        text.text = current_number.ToString();
        StartCoroutine(CountScore());
    }
    IEnumerator CountScore()
    {
        WaitForSeconds delay = new WaitForSeconds(0.005f);
        while(current_number >= 0)
        {
            current_number += count_increase_factor;
            text.text = current_number.ToString();
            yield return delay;
            if (current_number >= target_number)
            {
                callback_function?.Invoke();
            }
            yield return new WaitUntil(() => current_number < target_number);
        }
    }
}
