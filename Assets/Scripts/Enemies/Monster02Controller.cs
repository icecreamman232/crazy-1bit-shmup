using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster02Controller : MonsterWithCustomPath
{
    public CircleCollider2D circleCollider;
    public int dodgeCount;
    private int counter;
    public bool isDodgeAlready;
    private bool isFinishedDodge;
    private BoxCollider2D m_Collider;


    void Start()
    {
        m_Collider = GetComponent<BoxCollider2D>();
    }


    void Update()
    {

    }
    public override void Run()
    {
        isDodgeAlready = false;
        isFinishedDodge = false;
        counter = dodgeCount;
        base.Run();
    }
    void DoDodgeIncomingBullet()
    {
        isDodgeAlready = true;
        Vector3 randomPos = Vector3.one;
        if(randomPos.x < 0)
        {
            randomPos.x -= Random.Range(0.5f, 1.0f);
        }
        else
        {
            randomPos.x += Random.Range(0.5f, 1.0f);
        }
        randomPos.y = transform.position.y + 1.0f;
        randomPos.z = 0;
        LeanTween.move(this.gameObject, randomPos, 0.5f)
            .setOnComplete(OnFinishDodge)
            .setEase(LeanTweenType.easeOutQuart);
    }
    void OnFinishDodge()
    {
        moveController.Resume();      
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            {
                if (counter > 0)
                {
                    if (circleCollider.IsTouching(collision))
                    {
                        moveController.Stop();
                        DoDodgeIncomingBullet();
                        dodgeCount--;
                    }
                }
            }
            if (counter <= 0)
            {
                base.OnTriggerEnter2D(collision);
            }
        }       
    }
}
