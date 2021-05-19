using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster04Controller : MonsterWithSplineMove
{
    public EnemyGunController m_gun;

    public Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_gun.OnPlayAnimShooting += PlayAnimationShooting;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayAnimationShooting()
    {
        m_animator.Play("Monster04_Shot_Anim");
    }
    public override void Run()
    {
        m_gun.SetupGun();
        m_gun.Shoot();
        base.Run();
        
    }


}
