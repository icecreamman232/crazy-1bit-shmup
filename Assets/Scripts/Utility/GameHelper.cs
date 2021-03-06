using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    #region Constants
    public const int MAX_RANK_POINTS = 5;
    public const int MAX_RANK = 9;
    #endregion

    public const string dataObjectMenuName = "Data Objects/";

    public static Vector2 sizeOfCamera = Vector2.zero;
    public static Vector2 GetCurrentScreenBounds()
    {
        if(sizeOfCamera == Vector2.zero)
        {
            ReCalculateSizeOfCamera();
        }
        return sizeOfCamera;
    }
    public static Vector2 HalfSizeOfCamera()
    {
        return GetCurrentScreenBounds() * 0.5f;
    }


    public static void ReCalculateSizeOfCamera()
    {
        Vector2 A = new Vector2();
        A.y = Camera.main.orthographicSize * 2;
        A.x = (Camera.main.aspect * Camera.main.orthographicSize) * 2;
        sizeOfCamera = A;
    }

    //public static Vector2 GetCurrentScreenBounds()
    //{
    //    return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    //}

    public static bool IsInsideScreenBounds(Vector3 positon)
    {
        bool result = false;
        if(positon.x > -GameHelper.GetCurrentScreenBounds().x/2 && positon.x < GameHelper.GetCurrentScreenBounds().x/2)
        {
            if(positon.y > -GameHelper.GetCurrentScreenBounds().y/2 && positon.y < GameHelper.GetCurrentScreenBounds().y/2)
            {
                result = true;
            }
        }
        return result;
    }
}
