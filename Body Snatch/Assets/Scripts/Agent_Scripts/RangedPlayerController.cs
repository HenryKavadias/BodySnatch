using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangedPlayerController : PlayerController
{
    protected override void PlayerControls()
    {
        gameObject.GetComponent<Rigidbody>().rotation = Quaternion.identity;
        PlayerMovement();
        FaceMouse();
        // 2. Ranged attack
        if (Input.GetMouseButton(0) && 
            agentCtrl.attacks.Count >= 1 && 
            agentCtrl.attacks[0].attack != null)
        {
            PlayerRangedAttack();
            //Debug.Log("Pow");
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

    // todo: attacks go through the floor, not out from where the agent is facing
    protected void PlayerRangedAttack()
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

            // Set range of attack. Must always be done
            currentAttack.GetComponent<DestroyByTime>().SetRange(agentCtrl.attacks[0].attackRange);
        }
    }
}
