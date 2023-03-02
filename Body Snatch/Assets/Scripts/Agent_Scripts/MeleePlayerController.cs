using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleePlayerController : PlayerController
{
    protected override void PlayerControls()
    {
        gameObject.GetComponent<Rigidbody>().rotation = Quaternion.identity;
        PlayerMovement();
        FaceMouse();
        // 1. Melee attack
        if (Input.GetMouseButtonDown(0) && 
            agentCtrl.attacks.Count >= 1 && 
            agentCtrl.attacks[0].attack != null)
        {
            PlayerMeleeAttack();
        }
    }
    protected override void PlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        gameObject.GetComponent<Rigidbody>().velocity = movement * playerSpeed;

        gameObject.GetComponent<Rigidbody>().position = new Vector3
            (gameObject.GetComponent<Rigidbody>().position.x,
            0.0f,
            gameObject.GetComponent<Rigidbody>().position.z);
    }

    protected void PlayerMeleeAttack()
    {
        if (Time.time > agentCtrl.attackCooldowns[0])
        {
            agentCtrl.attackCooldowns[0] = Time.time + agentCtrl.attacks[0].attackRate;

            // Spawns attack hitbox
            GameObject currentAttack = Instantiate(agentCtrl.attacks[0].attack,
                agentCtrl.attacks[0].attackSpawn.position,
                agentCtrl.attacks[0].attackSpawn.rotation);
            // Gives the attack a reference to the attack. Must always be done
            currentAttack.GetComponent<AgentTriggerDamager>().SetAttacker(gameObject);
            currentAttack.GetComponent<AgentTriggerDamager>().SetMeleePosition(0);
        }
    }

    // todo: attacks go through the floor, not out from where the agent is facing
    //protected void PlayerRangedAttack()
    //{
    //    if (Time.time > agent.attackCooldowns[1])
    //    {
    //        agent.attackCooldowns[1] = Time.time + agent.attacks[1].attackSpeed;

    //        // Spawns attack hitbox
    //        GameObject currentAttack = Instantiate(agent.attacks[1].attack,
    //            agent.attacks[1].attackSpawn.position,
    //            agent.attacks[1].attackSpawn.rotation);
    //        // Gives the attack a reference to the attack. Must always be done
    //        currentAttack.GetComponent<DestroyByContact>().attackerRef = gameObject;
    //    }
    //}

    //protected override void Update()
    //{
    //    PlayerControls();
    //}
}
