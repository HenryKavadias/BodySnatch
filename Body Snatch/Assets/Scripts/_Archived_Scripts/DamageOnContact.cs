using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


// todo: what will happen if the "Attacker" is dead (destroyed)
// but the attack still exists and hits something. Their is a null reference error
public class DamageOnContact : MonoBehaviour
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

    public void SetAttacker(GameObject attackerReference)
    {
        attacker = attackerReference;
    }

    public void SetMeleePosition(int atkIndex)
    {
        if (attacker != null && melee) 
        {
            // Binds the melee attack to the attack spawn of the attacking agent
            ConstraintSource meleeCon = new ConstraintSource();
            meleeCon.sourceTransform = attacker.GetComponent<AgentControl>().attacks[atkIndex].attackSpawn;
            meleeCon.weight = 1.0f;
            GetComponent<ParentConstraint>().AddSource(meleeCon);
        }
        else if (attackerRef == null)
        {
            Debug.Log("Attacker is null");
        }
    }

    // same as destroy by contact script but doesn't delete the attack object.
    // need another script the delete object (e.g. destroy by time)
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ATK Hit: " + other.tag + " ATKer is: " + attacker.tag);

        // Can't collide with the boundary
        if (other.tag == "Boundary" || other.tag == "Wall")
        {
            CheckForDestory();
            return;
        }

        // If enemy attacks player
        if (other.tag == "Player" && attacker.tag == "Enemy")
        {
            Debug.Log("Enemy HIT!");
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
            Destroy(transform.root.gameObject);
        }
    }
}
