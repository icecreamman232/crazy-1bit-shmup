using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotation : MonoBehaviour
{
    public Transform pivot;
    public bool doRotationToward;
    public bool doRotationEuler;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.R))
        {
            if(doRotationToward)
            {
                DoRotationToward();
            }
            else if(doRotationEuler)
            {
                DoRotationByLocalEuler();
            }
        }
    }
    public void DoRotationToward()
    {
        StartCoroutine(DoingRotationToward());
    }
    public void DoRotationByLocalEuler()
    {
        StartCoroutine(DoingRotaionByLocalEuler());
    }
    IEnumerator DoingRotationToward()
    {
        float targetAngle = 30f;
        
        Quaternion targetQuaternion = Quaternion.AngleAxis(targetAngle, Vector3.forward);
        while(true)
        {
            if(transform.rotation == targetQuaternion)
            {
                targetAngle += 30f;
                if(targetAngle > 360)
                {
                    yield break;
                }
                targetQuaternion = Quaternion.AngleAxis(targetAngle, Vector3.forward);                
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator DoingRotaionByLocalEuler()
    {
        float angle = 0;
        while(true)
        {
            if(angle >=360)
            {
                
                angle = 0;
                yield break;
            }
            angle += rotationSpeed * Time.deltaTime;
            transform.localEulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }
    }
}
