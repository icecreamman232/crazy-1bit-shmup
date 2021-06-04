using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObjectComponent : MonoBehaviour
{
    public List<GameObject> objectList;
    public GameObject host;


    public GameObject targetObject;

    public float timeResponse;
    public float rotationSpeed;

    private Vector2 direction;
    private float angle;
    


    void Start()
    {
        StartCoroutine(OnRotateToTarget());
    }

    
    void Update()
    {
       
    }
    IEnumerator OnRotateToTarget()
    {
        while (true)
        {
            
            Vector3 result = targetObject.transform.position - transform.position;
            direction.x = result.x;
            direction.y = result.y;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            yield return null;
            if(transform.rotation == rotation)
            {
                yield return new WaitForSeconds(3.0f);
                Debug.Log("Waiting");
            }     
        }
    }
}
