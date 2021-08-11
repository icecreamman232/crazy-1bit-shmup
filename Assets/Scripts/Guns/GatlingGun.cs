using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GatlingGun : Gun
{
    float firerateTimer;
    private void Update()
    {  
        if(gunState == GunState.COOLDOWN)
        {
            firerateTimer += Time.deltaTime;
            if(firerateTimer >= gunData.fireRate)
            {
                firerateTimer = 0;
                gunState = GunState.STANDBY;
            }
        }
    }
    public override void SetupGun()
    {
        base.SetupGun();
        Bullet.MoveSpeed = gunData.bulletMoveSpeed;
        gunState = GunState.STANDBY;
        coroutineShooting = null;
        firerateTimer = 0;
        
        //Damage need to be setup in Bullet Prefab
    }
    public override void Shoot()
    {
        if(gunState == GunState.STANDBY)
        {
            gunState = GunState.SHOOTING;

            base.Shoot();
            if (coroutineShooting != null)
            {
                StopCoroutine(coroutineShooting);
                coroutineShooting = null;
            }
            coroutineShooting = StartCoroutine(Shooting());
        }
        
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
        for (int i = 0; i < gunData.numWavePerShot; i++)
        {
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
            yield return waveDelay;
        }
        gunState = GunState.COOLDOWN;
    }
}
