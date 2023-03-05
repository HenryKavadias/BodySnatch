using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageableAgent : Damageable
{
    // Variable for agent to track last attacking agent.
    // Player agent needs this for death possessions so it knowns
    // which agent to possess on death (the last possessable agent that attacked it)
    protected GameObject lastAttacker = null;

    public override void Damage(float damage, GameObject attacker = null)
    {
        // prevents overlapping hit ties
        if (_health.Current <= 0)
        {
            return;
        }

        _health.Sub(damage);

        //Debug.Log(gameObject.tag + " has taken " + damage + " damage. Remaining health is " + _health.Current);

        if (attacker != null && attacker.GetComponent<AgentControl>().possessable)
        {
            lastAttacker = attacker;
            //Debug.Log("Attacker exists");
        }

        if (_health.Current <= 0) 
        {
            // Condition for the player, reduce the number of death possessions
            // count and switch player to controls for the last attacker

            // If the player has no death possessions left, then the agent dies
            if (gameObject.tag == "Player" && GetComponent<AgentControl>().deathPossessionsRef > 0)
            {
                GetComponent<AgentControl>().deathPossessionsRef -= 1;

                if (lastAttacker  != null)
                {
                    lastAttacker.GetComponent<AgentControl>().SwitchMode(
                        GetComponent<AgentControl>().deathPossessionsRef);
                }
            }

            // Remove agent from world when they are out of health
            if (gameObject.tag == "Enemy" || gameObject.tag == "Player")
            {
                GameObject GC = GameObject.FindGameObjectWithTag("GameController");
                GC.GetComponent<GameController>().RemoveAgent(gameObject);
            }
            Die();
        }
    }
}
