using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testrotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LeanTween.rotateAroundLocal(this.gameObject, Vector3.forward, 360, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
