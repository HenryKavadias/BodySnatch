using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AgentTriggerDamager : Damager
{
    protected GameObject attacker = null; // If it has an attacker, this should assigned on creation

    public void SetAttacker(GameObject atker)
    {
        attacker = atker;
    }

    public override void Damage(Damageable damageable) => damageable.Damage(damage, attacker);

    public bool melee = false;

    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.transform.root.GetComponent<Damageable>();

        //Debug.Log("Attack landed on " + other.tag);

        if (damageable)
        {
            //Debug.Log("Attack landed on " + damageable.tag);

            if (gameObject.tag == "Attack")
            {
                // Can't collide with the boundary
                if (other.tag == "Boundary" || other.tag == "Wall")
                {
                    CheckForDestory();
                    return;
                }

                if (attacker != null)
                {
                    // If enemy attacks player
                    if (other.tag == "Player" && attacker.tag == "Enemy")
                    {
                        damageable.Damage(damage, attacker);
                        CheckForDestory();
                    }

                    // If player attacks enemy
                    if (other.tag == "Enemy" && attacker.tag == "Player")
                    {

                        damageable.Damage(damage, attacker);
                        CheckForDestory();
                    }
                }
                else
                {
                    damageable.Damage(damage);
                    CheckForDestory();
                }
            }
            else
            {
                damageable.Damage(damage);
            }
        }
    }

    public void SetMeleePosition(int atkIndex)
    {
        if (attacker != null && melee)
        {
            // Binds the melee attack to the attack spawn of the attacking agent
            ConstraintSource meleeCon = new ConstraintSource();
            meleeCon.sourceTransform =
                attacker.GetComponent<AgentControl>().attacks[atkIndex].attackSpawn;
            meleeCon.weight = 1.0f;
            GetComponent<ParentConstraint>().AddSource(meleeCon);
        }
        else if (attacker == null)
        {
            Debug.Log("Attacker is null");
        }
    }

    void CheckForDestory()
    {
        if (!melee)
        {
            Destroy(transform.root.gameObject);
        }
    }
}
