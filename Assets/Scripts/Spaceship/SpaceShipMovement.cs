using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


public class SpaceShipMovement : MonoBehaviour
{
    public float sensitivity;
    bool isTouching;

    // Start is called before the first frame update
    void Start()
    {
        isTouching = false;
        OnEnableTouch();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTouching) return;
        var screen_delta = LeanGesture.GetScreenDelta();
        if (screen_delta == Vector2.zero) return;
        TranslateShip(screen_delta);
    }
    private void OnDestroy()
    {
        OnDisableTouch();
    }
    private void OnEnableTouch()
    {
        LeanTouch.OnFingerDown += OnTouchDown;
        LeanTouch.OnFingerUp += OnTouchUp;
    }
    private void OnDisableTouch()
    {
        LeanTouch.OnFingerDown -= OnTouchDown;
        LeanTouch.OnFingerUp -= OnTouchUp;
    }
    public void OnTouchDown(LeanFinger fingers)
    {
        isTouching = true;
    }
    public void OnTouchUp(LeanFinger fingers)
    {
        isTouching = false;
    }
  
    void TranslateShip(Vector2 delta)
    {
        var camera = Camera.main;
        if(camera!=null)
        {
            var screen_pts = camera.WorldToScreenPoint(transform.position);
            screen_pts += (Vector3)delta * sensitivity;

            var world_pts = camera.ScreenToWorldPoint(screen_pts);

            if (world_pts.x <= -2.5f) world_pts.x = -2.5f;
            if (world_pts.x >= 2.5f) world_pts.x = 2.5f;
            world_pts.y = -4;
            transform.position = world_pts;
        }
        else
        {
            Debug.LogError("Camera is null");
        }
    }
}
