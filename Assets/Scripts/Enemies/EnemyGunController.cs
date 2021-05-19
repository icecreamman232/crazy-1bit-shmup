using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    public BaseBullet m_bullet;

    public GunDO gunData;


    private void Start()
    {
        Shoot();
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
            for(int i = 0; i < gunData.numWavePerShot; i++)
            {
                for (int j = 0; j < gunData.waveShot.Count; j++)
                {
                    var quaternion = Quaternion.Euler(gunData.waveShot[j].m_Rotation);
                    var bullet = Lean.Pool.LeanPool.Spawn(
                        m_bullet, 
                        gunData.waveShot[j].m_Position+this.transform.position, 
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
