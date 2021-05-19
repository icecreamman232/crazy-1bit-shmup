using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    public BaseBullet m_bullet;
    public Transform m_firePoint;


    public GunDO gunData;

    public System.Action OnPlayAnimShooting;

    private void Start()
    {
    }

    public void SetupGun()
    {
        m_bullet.bullet_movespeed = gunData.bulletMoveSpeed;
        m_bullet.Damage = 1;
    }
    public void Shoot()
    {
        
        StartCoroutine(OnShooting());
    }
    IEnumerator OnShooting()
    {
        WaitForSeconds waveDelay = new WaitForSeconds(gunData.delayperWaveShot);
        WaitForSeconds fireRate = new WaitForSeconds(gunData.fireRate);
        while (true)
        {
            OnPlayAnimShooting?.Invoke();
            for (int i = 0; i < gunData.numWavePerShot; i++)
            {
                for (int j = 0; j < gunData.waveShot.Count; j++)
                {
                    var quaternion = Quaternion.Euler(gunData.waveShot[j].m_Rotation);
                    var bullet = Lean.Pool.LeanPool.Spawn(
                        m_bullet,
                        gunData.waveShot[j].m_Position + m_firePoint.position,
                        quaternion);
                    bullet.Shoot(gunData.waveShot[j].m_Rotation);
                    yield return new WaitForSeconds(gunData.waveShot[j].m_delay);
                }
                yield return waveDelay;
            }
            yield return fireRate;
        }
    }

}
