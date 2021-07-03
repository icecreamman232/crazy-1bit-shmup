using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunDO gunData;

    [SerializeField]
    private Bullet bullet;
    [SerializeField]
    private Transform firePoint;

    protected Coroutine coroutineShooting;

    #region Getter Setter methods
    public Bullet Bullet
    {
        get
        {
#if UNITY_EDITOR
            if (bullet == null)
            {
                Debug.LogError("Bullet Prefab is NULL");
            }
#endif
            return bullet;
        }
        set
        {
            bullet = value;
        }

    }
    public Transform FirePoint
    {
        get
        {
            return firePoint;
        }
        set
        {
            firePoint = value;
        }
    }
    #endregion

    public virtual void SetupGun() { }
    public virtual void Shoot() { }
    public virtual void Stop() { }
}
