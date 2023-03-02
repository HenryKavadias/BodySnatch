using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeEnemyController : EnemyController
{
    // Dictates the travel location of the agent
    protected override void AiMovement()
    {
        if (target != null)
        {
            float distFromSelf = GetDistanceFromTarget();

            FaceTarget();
            agentCtrl.DecelerateVelocity();
            if (distFromSelf > minTargetDistance)
            {
                navAgent.isStopped = false;

                navAgent.SetDestination(
                target.GetComponent<Transform>().position);
            }
            else
            {
                navAgent.isStopped = true;
                agentCtrl.DecelerateVelocity();
            }
            AiMeleeAttack();
        }
        else
        {
            ChooseTarget();
        }
    }


    // Controls the melee attack behavour of the agent
    protected void AiMeleeAttack()
    {
        float distFromSelf = GetDistanceFromTarget();

        if (distFromSelf <= minTargetDistance)
        {
            // Instantly turn to face the target
            //transform.LookAt(target.transform.position);

            if (Time.time > agentCtrl.attackCooldowns[0] && CheckMeleeAreaForTarget())
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
    }
}