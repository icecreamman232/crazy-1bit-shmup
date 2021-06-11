using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelController : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if(transform.position.x < 0)
        {
            transform.position = new Vector3(-GameHelper.SizeOfCamera().x / 2 - 0.5f, 0, 0);
            GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);
        }
        else
        {
            transform.position = new Vector3(GameHelper.SizeOfCamera().x / 2 + 0.5f, 0, 0);
            GetComponent<BoxCollider2D>().size = new Vector2(1, GameHelper.SizeOfCamera().y);
        }
        
    }

}
