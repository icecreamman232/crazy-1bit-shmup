using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFlashing : MonoBehaviour
{
    public float duration;
    public float counter;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Flashing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Flashing()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        counter = 0;
        while(true)
        {

            yield return new WaitForSeconds(.25f);
            Color c = sr.color;
            float save_alpha = c.a;
            c.a = 0f;
            sr.color = c;
            yield return new WaitForSeconds(.25f);
            c.a = save_alpha;
            sr.color = c;
            counter += Time.deltaTime+0.5f;
        }
        

    }
}
