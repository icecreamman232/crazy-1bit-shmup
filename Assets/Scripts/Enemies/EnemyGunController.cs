using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    public BaseBullet m_bullet;
    public Transform m_firePoint;


    public GunDO gunData;

    #region Testing
    public System.Action Done1Shot;
    public System.Action StopMoveToShoot;
    public System.Action PlayRecoilAnimation;
    #endregion
   

    private void Start()
    {
    }

    public void SetupGun()
    {
        m_bullet.bulletMoveSpeed = gunData.bulletMoveSpeed;
        m_bullet.Damage = 1;
    }
    #region test shooting
    public void ShootTest()
    {
        StartCoroutine(OnShootingTest());
    }
    IEnumerator OnShootingTest()
    {
        WaitForSeconds waveDelay = new WaitForSeconds(gunData.delayperWaveShot);
        WaitForSeconds fireRate = new WaitForSeconds(gunData.fireRate);
        float time = Time.time;
     
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
                PlayRecoilAnimation?.Invoke();
                yield return new WaitForSeconds(gunData.waveShot[j].m_delay);
            }
            Done1Shot?.Invoke();
            yield return waveDelay;
        }
        
    }
    #endregion

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
            StopMoveToShoot?.Invoke();
            for (int i = 0; i < gunData.numWavePerShot; i++)
            {
                PlayRecoilAnimation?.Invoke();
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
                Done1Shot?.Invoke();
                yield return waveDelay;
            }
            yield return fireRate;
        }
    }

}
