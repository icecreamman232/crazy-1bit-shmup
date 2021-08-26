using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public Vector2 DesignAspect;
    public Vector2 CurrentAspect;
    public Vector2 ScreenBounds;
    public bool ShowGizmo;

    private Camera mainCamera;
    private void Awake()
    {
        ShowGizmo = true;
        mainCamera = Camera.main;
        GetCurrentAspect();
        ResizeCamera2D();
        //CropViewport();
        GetCameraSize();
    }
    private void Start()
    {
        
    }
    private void GetCurrentAspect()
    {
        //For the PC, the width is always bigger than the height
        var ratio = (float)Screen.width / (float)Screen.height;
        if(ratio >= 1.77f)
        {
            CurrentAspect = new Vector2(16, 9);
            return;
        }
        else if(ratio >= 1.6f)
        {
            CurrentAspect = new Vector2(16, 10);
            return;
        }
        else if (ratio >= 1.3f)
        {
            CurrentAspect = new Vector2(4, 3);
            return;
        }
    }
    private void CropViewport()
    {
        /*
         * Common screen aspect would be 16:9. For the PC, the screen always lies horizontally, like this [--]
         * But the game would take the vertical way, would keep the height and crop the width
         * As we have 16:9 for common screen and the target aspect would be 3:4
         * We would keep the ratio 9 for 4 and find the width ratio would be 6.75 (9:4x3)
         * From there we calculate to crop the width and set the viewport centered
         */
        Rect newViewport = new Rect();
        newViewport.width = (CurrentAspect.y / DesignAspect.y * DesignAspect.x) / CurrentAspect.x;
        newViewport.height = 1;
        newViewport.x = (1 - newViewport.width)/2;
        newViewport.y = 0;
        
        mainCamera.rect = newViewport;
    }

    /// <summary>
    /// Resize camera view size based on aspect different
    /// </summary>
    private void ResizeCamera2D()
    {
        //For the PC the width is always bigger than the height
        var defaultRatio = DesignAspect.x > DesignAspect.y ? 
            DesignAspect.x / DesignAspect.y : DesignAspect.y / DesignAspect.x;
        var currentRatio = (float)Screen.width > (float)Screen.height ?
            (float)Screen.width / (float)Screen.height : (float)Screen.height / (float)Screen.width;
        var zoomValue = currentRatio / defaultRatio;
        mainCamera.orthographicSize = mainCamera.orthographicSize * zoomValue;
    }
    private void GetCameraSize()
    {
        //Follow Unity Document https://docs.unity3d.com/ScriptReference/Camera-orthographicSize.html
        ScreenBounds.y = mainCamera.orthographicSize * 2;
        ScreenBounds.x = mainCamera.aspect * mainCamera.orthographicSize * 2;
    }
}
