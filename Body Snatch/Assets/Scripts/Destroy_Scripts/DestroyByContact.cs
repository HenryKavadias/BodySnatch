using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    // Damage of attack
    public float damageDealt;

    public bool melee = false;

    private GameObject attacker;

    // Reference to active game controller script
    //private GameController gameController;

    public GameObject attackerRef
    {
        get => attacker;
        set => attacker = value;
    }

    //todo?: create a hitbox that despawns after a timer, but other objects only get damaged by it once
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ATK Hit: " + other.tag + " ATKer is: " + attacker.tag);
        
        // Can't collide with the boundary
        if (other.tag == "Boundary")
        {
            CheckForDestory();
            return;
        }

        // If enemy attacks player
        if (other.tag == "Player" && attacker.tag == "Enemy")
        {
            //Debug.Log("Enemy HIT!");
            //other.transform.root.gameObject.GetComponent<AgentControl>().DamageAgent(damageDealt, attacker);
            CheckForDestory();
        }

        // If player attacks enemy
        if (other.tag == "Enemy" && attacker.tag == "Player")
        {
            //Debug.Log("Player HIT!");
            //other.transform.root.gameObject.GetComponent<AgentControl>().DamageAgent(damageDealt, attacker);
            CheckForDestory();
        }
    }

    void CheckForDestory()
    {
        if (!melee)
        {
            Destroy(gameObject);
        }
    }
    //public float GetDamage()
    //{
    //    return damageDealt;
    //}
}
