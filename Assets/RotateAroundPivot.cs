using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundPivot : MonoBehaviour
{
    public Transform pivot;
    public bool isRotate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            isRotate = true;
            StartCoroutine(AroundAPivot());
        }
        if(isRotate)
        {
            //AroundPivot();
        }
    }
    private IEnumerator AroundAPivot()
    {
        float spd = 50;
        float rot = 5;
        Vector3 dir = pivot.position - transform.position;
        while (true)
        {
            if(rot >=180)
            {
                rot = 5;
                //yield break;
            }
           // transform.position = pivot.position + (transform.position - pivot.position).normalized * 1.0f;
            dir = Quaternion.Euler(new Vector3(0, 0, spd*Time.deltaTime)) * dir;
            transform.position = dir + pivot.transform.position;
            rot += spd;
            yield return null;
        }
    }
    public void AroundPivot()
    {
        //Vector3 dir = pivot.position - transform.position;
        //dir = Quaternion.Euler(new Vector3(0, 0, 30f)) * dir + pivot.transform.position;
        //transform.position = dir;

        transform.position = pivot.position + (pivot.position - transform.position).normalized * 2.0f;
        transform.RotateAround(pivot.position, Vector3.forward,5f * Time.deltaTime);
    }

}
