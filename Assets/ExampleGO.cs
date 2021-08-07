using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGO : MonoBehaviour
{
    public Vector2 ScreenBounds;
    public Vector2 ConvertedSBounds;
    // Start is called before the first frame update
    void Start()
    {
        ScreenBounds.y = Camera.main.orthographicSize * 2;
        ScreenBounds.x = Camera.main.aspect * Camera.main.orthographicSize * 2;
        ConvertedSBounds = Camera.main.ScreenToWorldPoint(new Vector3(ScreenBounds.x, ScreenBounds.y, Camera.main.transform.position.z));
        transform.position = new Vector2(ConvertedSBounds.x/2, ConvertedSBounds.y/2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
