using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;


public class SpaceShipMovement : MonoBehaviour
{
    public float sensitivity;
    public bool isTouching;
    public SpriteRenderer shipSprite;
    private float shipSpriteWidth;
    private float shipSpriteHeight;
    private float lastPosX;
    private Animator shipAnimator;
    public Animator holdToPlayAnimator;

    public bool firstTouch;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        shipSpriteWidth = shipSprite.bounds.size.x;
        shipSpriteHeight = shipSprite.bounds.size.y;
        shipAnimator = GetComponent<Animator>();
        isTouching = false;
        firstTouch = false;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTouching) return;
        var screenDelta = LeanGesture.GetScreenDelta();
        if (screenDelta == Vector2.zero) return;
        TranslateShip(screenDelta);
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
        isTouching = false;
        firstTouch = false;
    }
    public void OnTouchDown(LeanFinger fingers)
    {
        if(!firstTouch)
        {
            firstTouch = true;
            holdToPlayAnimator.Play("holdtoplay_outtro_anim");
        }
        isTouching = true;
    }
    public void OnTouchUp(LeanFinger fingers)
    {
        isTouching = false; 
        shipAnimator.Play("ship_idle");
    }
    public void SetShipPosition()
    {
        mainCamera = Camera.main;
        var postion = new Vector3(0, -GameHelper.GetCurrentScreenBounds().y + shipSpriteHeight * 1.5f, 0);
        var target = postion;
        lastPosX = postion.x;
        postion.y -= 3.0f;
        transform.position = postion;
        isTouching = false;

        LeanTween.moveY(gameObject, target.y, 1.2f)
            .setEase(LeanTweenType.easeOutBack)
            .setOnComplete(OnCompleteSetupShipPosition);
    }
    void OnCompleteSetupShipPosition()
    {
        OnEnableTouch();
        holdToPlayAnimator.gameObject.SetActive(true);
        holdToPlayAnimator.Play("holdtoplay_intro_anim");
    }
    void TranslateShip(Vector2 delta)
    {
        if(mainCamera != null)
        {
            var screenPts = mainCamera.WorldToScreenPoint(transform.position);
            screenPts += (Vector3)delta * sensitivity;
            var worldPts = mainCamera.ScreenToWorldPoint(screenPts);
            var screenBounds = GameHelper.GetCurrentScreenBounds();
            
            if (worldPts.x <= -screenBounds.x + shipSpriteWidth*0.5f) worldPts.x = -screenBounds.x+ shipSpriteWidth * 0.5f;
            if (worldPts.x >= screenBounds.x - shipSpriteWidth *0.5f) worldPts.x = screenBounds.x - shipSpriteWidth * 0.5f;
            if (worldPts.y <= -screenBounds.y) worldPts.y = -screenBounds.y;

            //Ship keep turning left
            if(worldPts.x < lastPosX)
            {
               shipAnimator.Play("ship_turn_left_anim");
            }
            //Ship keep turning right
            else if (worldPts.x > lastPosX)
            {
                shipAnimator.Play("ship_turn_right_anim");
            }
            else
            {
                shipAnimator.Play("ship_idle");
            }

            lastPosX = worldPts.x;
            transform.position = worldPts;
        }
        else
        {
            Debug.LogError("Camera is null");
        }
    }
}
