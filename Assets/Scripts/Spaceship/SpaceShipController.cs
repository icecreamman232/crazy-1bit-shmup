using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public int base_hp;
    public int current_hp;


    public float firerate;
    public int damage;

    public Lean.Pool.LeanGameObjectPool bullet_pool;
    public Transform fire_point;

    #region UI
    public UIHeartController heart_panel;


    #endregion

    void Start()
    {
        StartShip();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartShip();
        }
    }


    /// <summary>
    /// Init all parameters and start ship
    /// </summary>
    void StartShip()
    {
        current_hp = base_hp;
        StartCoroutine(Shoot());
    }
    void UpdateHP(int damage)
    {
        current_hp -= damage;
    }
    IEnumerator Shoot()
    {
        while(true)
        {
            var bullet = bullet_pool.Spawn(fire_point.position, Quaternion.identity);
            bullet.GetComponent<BaseBullet>().damage = damage;
            bullet.GetComponent<BaseBullet>().Shoot();
            yield return new WaitForSeconds(firerate);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag =="Enemy")
        {

            Debug.Log("Ship hit enemies");
            UpdateHP(1);
            heart_panel.UpdateHeartUI();
        }
    }
}
