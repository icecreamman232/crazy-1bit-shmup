using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspectScaler : MonoBehaviour
{
    public Vector2 ReferenceResolution = new Vector2(1080, 1920);
    public float default_ratio;
    public float current_ratio;
    public float zoom_factor;
    // Start is called before the first frame update
    private void Awake()
    {
        default_ratio = ReferenceResolution.y / ReferenceResolution.x;
        current_ratio = (float)Screen.height / (float)Screen.width;
        zoom_factor = current_ratio / default_ratio;
        ResetCamera2DSize();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ResetCamera2DSize()
    {
        Camera.main.orthographicSize = Camera.main.orthographicSize * zoom_factor;
    }
}
