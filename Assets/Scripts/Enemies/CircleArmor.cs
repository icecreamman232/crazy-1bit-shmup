using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleArmor : MonoBehaviour
{
    public Transform pivot;
    private float speed;
    // Start is called before the first frame update
 
    public void Active(float _speed)
    {
        speed = _speed;
        StartCoroutine(RotateAroundPivot());
    }
    private IEnumerator RotateAroundPivot()
    {
        Vector3 dir = pivot.position - transform.position;
        float rot = 0;
        while(true)
        {
            if(rot >=180)
            {
                rot = 0;
            }
            dir = Quaternion.Euler(new Vector3(0, 0, speed * Time.deltaTime)) * dir;
            rot += speed * Time.deltaTime;
            transform.position = dir + pivot.transform.position;
            yield return null;
        }
    }
}
