using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class State
{
    protected GameManager gameManager;
    public abstract void Init(GameManager _gameManager);
    public abstract void Update();
}
