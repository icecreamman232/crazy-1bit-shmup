using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MeteorController : EnvironmentWithCustomPath
{
    public override void Setup()
    {
        base.Setup();
    }
    public override void Spawn()
    {
        base.Spawn();
        Move();
    }
    public override void Move()
    {
        base.Move();
        GetComponent<Animator>().Play("meteor_idle");
        GetComponent<Animator>().SetFloat("RotationSpeedModifier", 2.0f);

        bezierMoveController.OnMoveEnd -= OnMoveEnd;
        bezierMoveController.OnMoveEnd += OnMoveEnd;
        bezierMoveController.SetPath(intro);
        bezierMoveController.Stop();
        bezierMoveController.StartMove(LoopType.None);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameHelper.IsInsideScreenBounds(transform.position))
        {
            if(collision.gameObject.CompareTag("Enemy"))
            {
                IDamageable dmgable = collision.gameObject.GetComponent<IDamageable>();
                if(dmgable!=null)
                {
                    dmgable.TakeDamage(impactDamage);
                }
            }
            if (collision.gameObject.CompareTag("Player"))
            {
                bezierMoveController.Stop();
                FXManager.Instance.CreateFX(fxID.DIE_MONSTER_EXPLOSION, transform.position);
                OnMoveEnd();
            }
        }
    }
}
