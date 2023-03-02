using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamager : Damager
{
    public float radius = 3;
    public float force = 5;

    private void OnEnable()
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        for(int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Rigidbody rigidbody))
            {
                Vector3 direction = (colliders[i].transform.position - transform.position).normalized;
                rigidbody.AddForce(direction * force, ForceMode.Impulse);
            }

            if (colliders[i].TryGetComponent(out Damageable damageable)) 
            {
                Damage(damageable);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
