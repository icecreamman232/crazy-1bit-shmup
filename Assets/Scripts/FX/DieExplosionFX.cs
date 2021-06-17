using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieExplosionFX : FXItemController
{
    public ParticleSystem dieExplosionParticle;
    public override void Play()
    {
        base.Play();
        dieExplosionParticle.Play();
    }
    public override void Remove()
    {
        base.Remove();
    }
}
