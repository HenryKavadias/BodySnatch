using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class NormalPlayerController : PlayerController
{
    protected override void PlayerControls()
    {
        gameObject.GetComponent<Rigidbody>().rotation = Quaternion.identity;
        PlayerMovement();
        FaceMouse();

        // 1. Melee attack
        // 2. Ranged attack
        if (Input.GetMouseButtonDown(0) && 
            agentCtrl.attacks.Count >= 1 && 
            agentCtrl.attacks[0].attack != null)
        {
            PlayerMeleeAttack();
        }
        else if (Input.GetMouseButton(1) && 
            agentCtrl.attacks.Count >= 2 && 
            agentCtrl.attacks[1].attack != null)
        {
            PlayerRangedAttack();
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

            // Adds the attack range of the attack relative to the intial spawn position for the attack
            // spawnPosition + front of position * attack range
            Vector3 spawnPos = agentCtrl.attacks[0].attackSpawn.position +
                agentCtrl.attacks[0].attackSpawn.transform.forward * agentCtrl.attacks[0].attackRange;

            // Spawns attack hitbox
            GameObject currentAttack = Instantiate(
                agentCtrl.attacks[0].attack,
                spawnPos,
                agentCtrl.attacks[0].attackSpawn.rotation);

            // Gives the attack a reference to the attack and sets the position. Must always be done
            //currentAttack.GetComponent<DamageOnContact>().attackerRef = gameObject;
            currentAttack.GetComponent<AgentTriggerDamager>().SetAttacker(gameObject);
            currentAttack.GetComponent<AgentTriggerDamager>().SetMeleePosition(0);
        }
    }

    protected void PlayerRangedAttack()
    {
        if (Time.time > agentCtrl.attackCooldowns[1])
        {
            agentCtrl.attackCooldowns[1] = Time.time + agentCtrl.attacks[1].attackRate;

            // Spawns attack hitbox
            GameObject currentAttack = Instantiate(agentCtrl.attacks[1].attack,
                agentCtrl.attacks[1].attackSpawn.position,
                agentCtrl.attacks[1].attackSpawn.rotation);

            // Gives the attack a reference to the attack. Must always be done
            currentAttack.GetComponent<AgentTriggerDamager>().SetAttacker(gameObject);

            // Set range of attack. Must always be done
            currentAttack.GetComponent<DestroyByTime>().SetRange(agentCtrl.attacks[1].attackRange);
        }
    }

    //protected override void Update()
    //{
    //    PlayerControls();
    //}
}
