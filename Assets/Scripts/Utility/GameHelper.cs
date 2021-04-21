﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    public static Vector2 sizeOfCamera = Vector2.zero;
    public static Vector2 SizeOfCamera()
    {
        if(sizeOfCamera == Vector2.zero)
        {
            ReCalculateSizeOfCamera();
        }
        return sizeOfCamera;
    }
    public static Vector2 HalfSizeOfCamera()
    {
        return SizeOfCamera() * 0.5f;
    }
    public static float BotLimitation = -HalfSizeOfCamera().y;
    public static float TopLimitation = SizeOfCamera().y;
    public static void ReCalculateSizeOfCamera()
    {
        Vector2 A = new Vector2();
        A.y = Camera.main.orthographicSize * 2;
        A.x = (Camera.main.aspect * Camera.main.orthographicSize) * 2;
        sizeOfCamera = A;
    }
    
}
