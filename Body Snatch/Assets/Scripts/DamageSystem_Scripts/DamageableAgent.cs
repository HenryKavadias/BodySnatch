using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageableAgent : Damageable
{
    // Todo: give damageable a references to the objects "Health" script and use that to manage its health value

    protected GameObject lastAttacker = null;

    public override void Damage(float damage, GameObject attacker = null)
    {
        _health.Sub(damage);

        //Debug.Log(gameObject.tag + " has taken " + damage + " damage. Remaining health is " + _health.Current);

        if (attacker != null)
        {
            lastAttacker = attacker;
            //Debug.Log("Attacker exists");
        }

        if (_health.Current <= 0) 
        {
            // condition for the player, reduce the life
            // count and switch player to controls for the last attacker
            if (gameObject.tag == "Player" && GetComponent<AgentControl>().deathPossessionsRef > 0)
            {
                GetComponent<AgentControl>().deathPossessionsRef -= 1;

                if (lastAttacker  != null)
                {
                    lastAttacker.GetComponent<AgentControl>().SwitchMode(
                        GetComponent<AgentControl>().deathPossessionsRef);
                }
            }

            if (gameObject.tag == "Enemy" || gameObject.tag == "Player")
            {
                // Condition for no lives
                GameObject GC = GameObject.FindGameObjectWithTag("GameController");
                GC.GetComponent<GameController>().RemoveAgent(gameObject);
            }
            Die();
        }
    }
}
