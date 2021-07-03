using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMonsterRocketGun : Gun
{
    public System.Action Done1Shot;
    public System.Action StopMoveToShoot;
    public System.Action PlayRecoilAnimation;
    public override void SetupGun()
    {
        base.SetupGun();
        Bullet.MoveSpeed = gunData.bulletMoveSpeed;
        coroutineShooting = null;
    }
    public override void Shoot()
    {
        base.Shoot();
        if (coroutineShooting != null)
        {
            StopCoroutine(coroutineShooting);
            coroutineShooting = null;
        }
        coroutineShooting = StartCoroutine(Shooting());
    }
    public override void Stop()
    {
        base.Stop();
        if (coroutineShooting != null)
        {
            StopCoroutine(coroutineShooting);
            coroutineShooting = null;
        }
    }
    private IEnumerator Shooting()
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
                        Bullet,
                        gunData.waveShot[j].m_Position + FirePoint.position,
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
