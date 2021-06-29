using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMonsterRocket : Bullet
{
    private Coroutine fadeAway;
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
                    collision.gameObject.GetComponent<BaseMonster>().UpdateHP(Damage);
                    ResetBullet();
                }
                if (collision.gameObject.CompareTag("MeteorMonsterCircle"))
                {
                    rigidBody.velocity = Vector3.zero;
                    rigidBody.bodyType = RigidbodyType2D.Dynamic;
                    Vector3 trajectory = Random.insideUnitCircle;
                    var forceVector = new Vector3(
                             Random.Range(-5f, 5f) * trajectory.x,
                             Random.Range(-2.5f, -10f) * Mathf.Abs(trajectory.y),
                             0f);
                    rigidBody.AddForce(forceVector, ForceMode2D.Impulse);
                    fadeAway = StartCoroutine(FadeAway());
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
    private IEnumerator FadeAway()
    {
        while (true)
        {
            if (transform.localScale == Vector3.one * 0.5f)
            {
                transform.localScale = Vector3.one;
                ResetBullet();
                yield break;
            }
            transform.localScale -= Vector3.one * (1f * Time.deltaTime);
            yield return null;
        }
    }
}
