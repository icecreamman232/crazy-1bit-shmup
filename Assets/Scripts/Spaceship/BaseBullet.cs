using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    [SerializeField]
    private int damage;  
    public int Damage
    {
        set
        {
            damage = value;
            if(damage < 0)
            {
                damage = 0;
            #if UNITY_EDITOR
                Debug.Log("Damage is negative");    
            #endif
            }
        }
        get
        {
            return damage;
        }
    }
    public float bulletMoveSpeed;
    private Vector3 originPos;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Culling bullet
        if(transform.position.y >= GameHelper.GetCurrentScreenBounds().y)
        {
            Reset();
        }
        else if(transform.position.y <= -GameHelper.GetCurrentScreenBounds().y)
        {
            Reset();
        }
        else if(transform.position.x >= GameHelper.GetCurrentScreenBounds().x)
        {
            Reset();
        }
        else if(transform.position.x <= -GameHelper.GetCurrentScreenBounds().x)
        {
            Reset();
        }
    }
    public void Shoot()
    {
        //Hàm này dùng cho ship
        rigidBody.velocity = new Vector2(0f, bulletMoveSpeed);
    }
    public void Shoot(Vector3 direction)
    {
        //Hàm này dùng cho monster nên sẽ bắn thẳng từ trên xuống
        var vec = new Vector2(0f, -bulletMoveSpeed);       
        rigidBody.velocity = Quaternion.Euler(direction) * vec;
    }
    public void Reset()
    {       
        rigidBody.velocity = Vector3.zero;
        this.transform.position = originPos;
        Lean.Pool.LeanPool.Despawn(this.gameObject);
    }
}
