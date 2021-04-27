using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


public class SpaceShipMovement : MonoBehaviour
{
    public float sensitivity;
    bool isTouching;
    public SpriteRenderer ship_sprite;
    float ship_sprite_width;
    float ship_sprite_height;
    public float last_pos_x;
    public Animator ship_animator;

    // Start is called before the first frame update
    void Start()
    {
        ship_sprite_width = ship_sprite.bounds.size.x;
        ship_sprite_height = ship_sprite.bounds.size.y;
        isTouching = false;
        SetShipPosition();
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
    public void OnEnableTouch()
    {
        LeanTouch.OnFingerDown += OnTouchDown;
        LeanTouch.OnFingerUp += OnTouchUp;
    }
    public void OnDisableTouch()
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
        ship_animator.Play("ship_idle");
    }
    public void SetShipPosition()
    {
        var postion = new Vector3(0, -GameHelper.get_current_screenbound().y + ship_sprite_height * 1.5f, 0);
        last_pos_x = postion.x;
        transform.position = postion;

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

            var screen_bound = GameHelper.get_current_screenbound();
            

            if (world_pts.x <= -screen_bound.x + ship_sprite_width*0.5f) world_pts.x = -screen_bound.x+ ship_sprite_width * 0.5f;
            if (world_pts.x >= screen_bound.x - ship_sprite_width *0.5f) world_pts.x = screen_bound.x - ship_sprite_width * 0.5f;
            world_pts.y = -screen_bound.y+ship_sprite_height*1.5f; 

            //Ship keep turning left
            if(world_pts.x < last_pos_x)
            {
               ship_animator.Play("ship_turn_left_anim");
            }
            //Ship keep turning right
            else if (world_pts.x > last_pos_x)
            {
                ship_animator.Play("ship_turn_right_anim");
            }
            else
            {
                ship_animator.Play("ship_idle");
            }
            last_pos_x = world_pts.x;


            transform.position = world_pts;
        }
        else
        {
            Debug.LogError("Camera is null");
        }
    }
}
