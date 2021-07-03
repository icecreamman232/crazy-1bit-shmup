using System.Collections;
using UnityEngine;

public class GatlingBullet : Bullet
{
    private Coroutine fadeAway;

    public override void Shoot(Vector3 rotation)
    {
        base.Shoot(rotation);

        var vec = new Vector2(0f, MoveSpeed);
        rigidBody.velocity = Quaternion.Euler(rotation) * vec;
    }

    public override void ResetBullet()
    {
        base.ResetBullet();
        if (fadeAway != null)
        {
            StopCoroutine(fadeAway);
            fadeAway = null;
        }
        transform.localScale = Vector3.one;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        rigidBody.velocity = Vector3.zero;
        this.transform.position = originPos;
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (GameHelper.IsInsideScreenBounds(collision.transform.position))
        {
            //Layer 11 = Environment
            if (collision.gameObject.layer == 11)
            {
                if (collision.gameObject.CompareTag("Meteor"))
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
                if (collision.gameObject.CompareTag("MeteorMonsterCircle"))
                {
                    rigidBody.velocity = Vector3.zero;
                    rigidBody.bodyType = RigidbodyType2D.Dynamic;
                    Vector3 trajectory = Random.insideUnitCircle;
                    var forceVector = new Vector3(
                                Random.Range(-5f, 5f) * trajectory.x,
                                Random.Range(-3.5f, -10f) * Mathf.Abs(trajectory.y),
                                0f);
                    rigidBody.AddForce(forceVector, ForceMode2D.Impulse);
                    fadeAway = StartCoroutine(FadeAway());
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
                ResetBullet();
                yield break;
            }
            transform.localScale -= Vector3.one * (1f * Time.deltaTime);
            yield return null;
        }
    }
}