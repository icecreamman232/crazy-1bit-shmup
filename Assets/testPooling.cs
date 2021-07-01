using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testPooling : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.R))
        {
            ObjectPooling.Instance.SpawnFromPool("monster", transform.position, Quaternion.identity,this.transform);
        }
    }
}
