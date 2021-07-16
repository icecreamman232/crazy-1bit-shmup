using UnityEngine;
using System;

/// <summary>
/// Interface which represent for damagable object
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// How many damage this entity would take
    /// </summary>
    /// <param name="damage">Damage that object took</param>
    void TakeDamage(int damage);
}
