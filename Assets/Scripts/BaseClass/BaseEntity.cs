using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base Class for everything objects/ entities in the game included: monster and environments

public abstract class BaseEntity : MonoBehaviour
{
    [Header("Basic Stats")]
    public int baseHP;
    public int maxHP;
    public int currentHP;

    public float baseMoveSpeed;
    public float currentMoveSpeed;

    public abstract void Setup();
    public abstract void Spawn();

}
