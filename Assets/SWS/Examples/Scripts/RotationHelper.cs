/*  This file is part of the "Simple Waypoint System" project by FLOBUK.
 *  You are only allowed to use these resources if you've bought them from the Unity Asset Store.
 * 	You shall not license, sublicense, sell, resell, transfer, assign, distribute or
 * 	otherwise make available to any third party the Service or the Content. */

using UnityEngine;
using DG.Tweening;

/// <summary>
/// Simple helper script to rotate an object over a period of time.
/// <summary>
public class RotationHelper : MonoBehaviour
{
    /// <summary>
    /// Total rotation duration.
    /// <summary>
    public float duration;

    /// <summary>
    /// Rotation value for rotating the object.
    /// <summary>
    public int rotation;


    void Start()
    {
        transform.DORotate(new Vector3(rotation, 0, 0), duration, DG.Tweening.RotateMode.LocalAxisAdd)
                 .SetEase(DG.Tweening.Ease.Linear)
                 .SetLoops(-1);
    }
}
