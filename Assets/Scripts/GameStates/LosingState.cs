using System.Collections;
using UnityEngine;


public class LosingState : State
{
    public override void Init(GameManager _gameManager)
    {
        UIManager.Instance.shipHealthBarUI.Hide();     
    }

    public override void Update()
    {
            
    }
}
