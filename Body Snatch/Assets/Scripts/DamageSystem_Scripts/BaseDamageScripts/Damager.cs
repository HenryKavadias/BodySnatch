using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damager : MonoBehaviour
{
    public float damage;

    public virtual void Damage(Damageable damageable) => damageable.Damage(damage);

    // pending for use
    //protected void ApplyDamage(IDamageable damageable) => damageable.Damage(damage);
}
