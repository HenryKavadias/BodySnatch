using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//todo: Fix how attack range is used
[System.Serializable]
public struct Attack
{
    public float attackRate, attackRange;
    public GameObject attack;
    public Transform attackSpawn;
}

// IMPORTANT NOTE: do not have more than one non-trigger hitbox on an agent.
// If an attack hits two or more of the hitboxes at the same time it will
// stuff up the possession mechanic. (or at least two hitboxes on multiple
// different child objects, don't know about multiple hitboxes on the same
// child/parent object. More testing required in that regard)
public class AgentControl : MonoBehaviour
{
    public float moveSpeed;
    //public float health;
    // Visual indicator that shows who is in control of the agent (AI or Player)
    public GameObject indicator;

    public List<Attack> attacks = null;

    [HideInInspector]
    public float[] attackCooldowns = null;

    // MonoBehaviour is Unity's generic script variable
    // Note: you can only access base variables and functions
    public MonoBehaviour pControl;
    public MonoBehaviour eControl;

    protected float decelerationRate = 0.5f;    // Must be between 0 and 1
    protected Rigidbody rb;

    private int deathPossessions = 0;
    public int deathPossessionsRef
    {
        get => deathPossessions;
        set => deathPossessions = value;
    }

    void Awake()
    {
        attackCooldowns = new float[attacks.Count];
        rb = gameObject.GetComponent<Rigidbody>();

        for (int i = 0; i < attackCooldowns.Length; i++)
        {
            attackCooldowns[i] = 0;
        }
    }

    //public void DamageAgent(float amount, GameObject attacker)
    //{
    //    health -= amount;

    //    if (health <= 0)
    //    {
    //        // condition for the player, reduce the life
    //        // count and switch player to controls for attacker
    //        if (gameObject.tag == "Player" && life > 0)
    //        {
    //            life -= 1;
    //            attacker.GetComponent<AgentControl>().SwitchMode(life);
    //        }
            
    //        // Condition for no lives
    //        GameObject GC = GameObject.FindGameObjectWithTag("GameController");
    //        GC.GetComponent<GameController>().RemoveAgent(gameObject);
    //        Destroy(gameObject);
    //    }
    //}

    // switch controls
    public void SwitchMode(int lifeCount)
    {
        if (eControl.enabled == true)
        {
            eControl.enabled = false;
            pControl.enabled = true;
            // set the life of the player to the new agent
            deathPossessionsRef = lifeCount;
        }
        else if (pControl.enabled == true)
        {
            pControl.enabled = false;
            eControl.enabled = true;
        }
    }

    // set as player
    public void SetAsPlayer(int setLifes = 0)
    {
        SetTags("Player");
        //gameObject.tag = "Player";
        pControl.enabled = true;
        eControl.enabled = false;
        deathPossessions = setLifes;
    }

    // set as enemy
    public void SetAsEnemy()
    {
        SetTags("Enemy");
        //gameObject.tag = "Enemy";
        pControl.enabled = false;
        eControl.enabled = true;
        deathPossessions = 0;
    }

    // Sets the game tag to of all child and parent
    // objects in a prefab to the same tag name
    public void SetTags(string tagName)
    {
        foreach (Transform t in gameObject.transform)
        {
            t.gameObject.tag = tagName;
        }

        gameObject.tag = tagName;
    }

    public void DecelerateVelocity()
    {
        rb.velocity = rb.velocity * decelerationRate * Time.deltaTime;
        rb.angularVelocity = rb.angularVelocity * decelerationRate * Time.deltaTime;
    }
}
