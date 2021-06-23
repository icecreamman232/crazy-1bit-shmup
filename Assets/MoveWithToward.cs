using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithToward : MonoBehaviour
{
    public float speed;
    public Transform target;
    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isMoving = true;
        }
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
}
