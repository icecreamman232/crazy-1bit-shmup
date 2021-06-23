using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnvironmentType
{
    NO_TAKE_DMG   = 0,
    CAN_TAKE_DMG        = 1,
}


public class BaseEnvironment : BaseEntity
{
    public int id;
    public int impactDamage;
    public EnvironmentType environmentType;
    //Callback after environment entity is destroyed
    public System.Action OnDestroy;
    public override void Setup()
    {
        currentHP = baseHP;
        if(environmentType == EnvironmentType.CAN_TAKE_DMG)
        {
            StartCoroutine(CheckDie());
        }
        
    }
    public override void Spawn()
    {
        
    }
    public virtual IEnumerator CheckDie()
    {
        yield return new WaitUntil(() => currentHP <= 0);
        
        FXManager.Instance.CreateFX(1, transform.position);
        GameManager.Instance.sfx.PlayOneShot(GameManager.Instance.monster_die_sfx, 0.3f);
        //GameManager.Instance.cameraShakeFX.Shake();
        currentMoveSpeed = baseMoveSpeed;
        OnDestroy?.Invoke();
        gameObject.SetActive(false);
    }
    
}
