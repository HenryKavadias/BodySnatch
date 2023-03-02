using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RangedEnemyController : EnemyController
{
    // Dictates the travel location of the agent
    protected override void AiMovement()
    {
        if (target != null)
        {
            float distFromSelf = GetDistanceFromTarget();

            FaceTarget();

            if (distFromSelf <= agentCtrl.attacks[0].attackRange)
            {
                AiRangedAttack();
                navAgent.isStopped = true;
                agentCtrl.DecelerateVelocity();
            }
            else
            {
                navAgent.SetDestination(
                target.GetComponent<Transform>().position);
                navAgent.isStopped = false;
            }
        }
        else
        {
            ChooseTarget();
        }
    }

    protected void AiRangedAttack()
    {
        //float distFromSelf = GetDistanceFromTarget();

        if (Time.time > agentCtrl.attackCooldowns[0] && 
            CheckFrontForTarget(agentCtrl.attacks[0].attackRange, 30))
        {
            agentCtrl.attackCooldowns[0] = Time.time + agentCtrl.attacks[0].attackRate;

            // todo?: make a sniper agent that uses the below functio if the target is in cone of sight
            // transform.LookAt(target.transform.position);

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