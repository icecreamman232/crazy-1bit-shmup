using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIHeartController : MonoBehaviour
{
    public int last_index;
    public List<Image> list_heart_ui;
    // Start is called before the first frame update
    void Start()
    {
        last_index = list_heart_ui.Count-1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateHeartUI()
    {
        list_heart_ui[last_index].GetComponent<Animator>().Play("heart_flash_anim");
        last_index -= 1;
        if(last_index <0)
        {
            last_index = 0;
        }
    }
}
