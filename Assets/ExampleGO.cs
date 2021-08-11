using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGO : MonoBehaviour
{
    public Vector3 moveDir;
    public float moveSpd;
    public Vector3 lastPos;

    private Rigidbody2D rgBody;
    private void Start()
    {
        rgBody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDir.x = -1;
                //transform.position += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDir.x = 1;
                //transform.position += new Vector3(1, 0, 0);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveDir.y = -1;
                //transform.position += new Vector3(0, -1, 0);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveDir.y = 1;
                //transform.position += new Vector3(0, 1, 0);
            }
        }
        else
        {
            moveDir = Vector3.zero;
        }

        transform.position += moveDir * moveSpd * Time.deltaTime;


        //moveDir.x = Input.GetAxisRaw("Horizontal");
        //moveDir.y = Input.GetAxisRaw("Vertical");

        //transform.position += moveDir * moveSpd * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        //rgBody.velocity = moveDir * moveSpd;
    }
}
