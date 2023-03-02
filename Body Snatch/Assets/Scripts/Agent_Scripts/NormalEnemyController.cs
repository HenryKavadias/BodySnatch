using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NormalEnemyController : EnemyController
{
    // Dictates the travel location of the agent
    protected override void AiMovement()
    {
        if (target != null)
        {
            float distFromSelf = GetDistanceFromTarget();

            FaceTarget();

            // AI behaviour:
            // if the target is far way, shoot at it,
            // otherwise run towards them for a melee attack,
            // if too far way, do nothing.

            if (distFromSelf <= agentCtrl.attacks[1].attackRange)
            {
                navAgent.isStopped = true;
                AiRangedAttack();
            }
            else if (distFromSelf < (agentCtrl.attacks[1].attackRange/2) && 
                distFromSelf > minTargetDistance)
            {
                navAgent.isStopped = false;

                navAgent.SetDestination(
                target.GetComponent<Transform>().position);
                AiMeleeAttack();
            }
            else
            {
                navAgent.isStopped = true;
                AiMeleeAttack();
                agentCtrl.DecelerateVelocity();
            }
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

    protected void AiRangedAttack()
    {
        // todo: add a method to detect if a target is in front of the agent,
        // and only attack if they are

        if (Time.time > agentCtrl.attackCooldowns[1] &&
            CheckFrontForTarget(agentCtrl.attacks[1].attackRange, 30))
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

}