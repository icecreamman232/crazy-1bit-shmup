using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMonsterRocket : Bullet
{
    
    public override void Shoot(Vector3 rotation)
    {
        base.Shoot(rotation);

        animator.Play("DodgeMonster_bullet_idle");
        var vec = new Vector2(0f, -MoveSpeed);
        rigidBody.velocity = Quaternion.Euler(rotation) * vec;
    }
    public override void ResetBullet()
    {
        base.ResetBullet();

        rigidBody.velocity = Vector3.zero;
        this.transform.position = originPos;
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (GameHelper.IsInsideScreenBounds(collision.transform.position))
        {
            // Layer 9 = Player
            if (collision.gameObject.layer == 9)
            {
                if(collision.gameObject.CompareTag("Player"))
                {
                    //Just 1 dmg because of ship only have 3 lives
                    collision.GetComponent<SpaceShipController>().HandleGetHitByEntity(1);
                    ResetBullet();
                }   
            }
            // Layer 8 = Enemy
            if (collision.gameObject.layer == 9)
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("Hit by enemy");
                    collision.gameObject.GetComponent<BaseMonster>().UpdateHP(Damage);
                    ResetBullet();
                }
                if (collision.gameObject.CompareTag("MeteorMonsterCircle"))
                {
                    //Do nothing because this is the bullet belong to meteor monster
                }
            }
            //Layer 11 = Environment
            if (collision.gameObject.layer == 11)
            {
                if (collision.gameObject.CompareTag("Meteor"))
                {
                    collision.GetComponent<MeteorController>().currentHP -= Damage;
                    ResetBullet();
                }
            }
        }
    }
    
}
