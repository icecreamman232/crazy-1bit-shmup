using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base Class for everything entities in the game included: monsters and environments

public abstract class BaseEntity : MonoBehaviour
{
    [Header("Current Stats")]
    public float currentMoveSpeed;

    public abstract void Setup();
    public abstract void Spawn();

}
