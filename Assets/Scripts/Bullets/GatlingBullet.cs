using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingBullet : Bullet
{
    public override void Shoot(Vector3 rotation)
    {
        base.Shoot(rotation);

        var vec = new Vector2(0f, MoveSpeed);
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

        //Layer 11 = Environment
        if (collision.gameObject.layer == 11)
        {
            if(collision.gameObject.CompareTag("Meteor"))
            {
                collision.GetComponent<MeteorController>().currentHP -= Damage;
                ResetBullet();
            }
        }
        //Layer 8 = Enemy
        if (collision.gameObject.layer == 8)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.GetComponent<BaseMonster>().UpdateHP(Damage);
                ResetBullet();
            }
        }
    }
}
