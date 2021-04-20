using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{
    public ParticleSystem gun_particle;
    void Start()
    {
        
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
        gun_particle.gameObject.SetActive(true);
        if(gun_particle.isPlaying)
        {
            gun_particle.Stop();
            gun_particle.Play();
        }
    }
    void test(int a)
    {

    }
}
