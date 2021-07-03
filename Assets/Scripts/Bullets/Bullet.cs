using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BulletType
{
    SHIP_BULLET     = 0,
    MONSTER_BULLET  = 1,
}
public class Bullet : MonoBehaviour
{
    public BulletType type;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float bulletMoveSpeed;

    #region Getter Setter
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
            if (damage < 0)
            {
                damage = 0;
#if UNITY_EDITOR
                Debug.Log("Damage is negative");
#endif
            }
        }
    }
    public float MoveSpeed
    {
        get
        {
            return bulletMoveSpeed;
        }
        set
        {
            bulletMoveSpeed = value;
            if (bulletMoveSpeed < 0)
            {
                bulletMoveSpeed = 0;
#if UNITY_EDITOR
                Debug.Log("Bullet MoveSpeed is negative");
#endif
            }
        }
    }
    #endregion

    protected Vector3 originPos;
    protected Rigidbody2D rigidBody;
    protected Animator animator;

    private void Awake()
    {
        originPos   = transform.position;
        rigidBody   = GetComponent<Rigidbody2D>();
        animator    = GetComponent<Animator>();
    }
    public void Update()
    {
        if (transform.position.y >= GameHelper.GetCurrentScreenBounds().y)
        {
            ResetBullet();
        }
        else if (transform.position.y <= -GameHelper.GetCurrentScreenBounds().y)
        {
            ResetBullet();
        }
        else if (transform.position.x >= GameHelper.GetCurrentScreenBounds().x + 1.0f)
        {
            ResetBullet();
        }
        else if (transform.position.x <= -GameHelper.GetCurrentScreenBounds().x - 1.0f)
        {
            ResetBullet();
        }
    }
    public virtual void Shoot(Vector3 rotation) { }
    public virtual void ResetBullet() { }
    public virtual void OnTriggerEnter2D(Collider2D collision) { }
}
