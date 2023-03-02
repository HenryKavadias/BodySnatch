using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    // AI variables
    public Material enemyMat;
    public NavMeshAgent navAgent;   // AI navigation agent

    public float minTargetDistance = 0;
    public float moveTurningSpeed = 120;
    public float lookTurningSpeed = 2;
    public float maxMeleeDetectionRange = 1.1f;

    public float viewRadius = 3;
    [Range(0, 360)]
    public float angleOfView = 100;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    // Universial variables
    protected Vector3 spawnPosition;
    protected AgentControl agentCtrl;
    protected GameObject target;

    //protected bool canMeleeAtk = false;
    private bool rigConstraintToggle = false;

    void Awake()
    {
        spawnPosition = transform.position;

        targetMask = LayerMask.GetMask("Agent");
        obstructionMask = LayerMask.GetMask("Obstruction"); ;
    }
    void Start()
    {
        agentCtrl = gameObject.GetComponent<AgentControl>();

        if (agentCtrl != null)
        {
            agentCtrl.SetTags("Enemy");
            target = null;
            navAgent.enabled = true;
            agentCtrl.indicator.GetComponent<Renderer>().material = enemyMat;
            navAgent.speed = agentCtrl.moveSpeed;
            navAgent.stoppingDistance = minTargetDistance;
            navAgent.angularSpeed = moveTurningSpeed;
        }
        else
        {
            this.enabled = false;
        }
    }

    // Casts a sphere infront of the agent to that
    // detects if the player is infront of them or not
    protected bool CheckMeleeAreaForTarget()
    {
        RaycastHit hit;

        bool isHit = Physics.SphereCast(
            transform.position,
            transform.lossyScale.x / 2,
            transform.forward,
            out hit,
            maxMeleeDetectionRange);
        if (isHit)
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    // Checks for the play in its view
    protected bool CheckFrontForTarget(float vRadisu = 0, float vAngle = 0)
    {
        if (vRadisu != 0)
        {
            vRadisu = viewRadius;
        }

        if (vAngle != 0)
        {
            vAngle = angleOfView;
        }
        
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, vRadisu, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = null;

            // Only set up for the first player it finds
            foreach (Collider agent in rangeChecks)
            {
                if (agent.tag == "Player")
                {
                    target = agent.transform;
                    break;
                }
            }

            if (target == null)
            {
                return false;
            }

            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < vAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                // Check if a wall or Obstruction is in the way
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    // Generic methods with varying functionality
    protected virtual void AiController() 
    {
        AiMovement();
    }
    protected virtual void AiMovement() { }
    protected virtual void Update()
    {
        AiController();
    }

    protected void ChooseTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            target = player;
        }
        else
        {
            ReturnToSpawn();
        }
    }

    // Directs Agent to spawn then stops all movement
    protected void ReturnToSpawn()
    {
        float distFromSelf = Vector3.Distance(
            gameObject.transform.position,
                spawnPosition);

        if (distFromSelf > 0.5)
        {
            navAgent.isStopped = false;

            navAgent.SetDestination(
            spawnPosition);

            rigConstraintToggle = true;
        }
        else if (rigConstraintToggle)
        {
            RigidbodyConstraints cons =
                gameObject.GetComponent<Rigidbody>().constraints;

            gameObject.GetComponent<Rigidbody>().constraints =
                RigidbodyConstraints.FreezeAll;
            gameObject.GetComponent<Rigidbody>().constraints =
               cons;
            navAgent.isStopped = true;

            rigConstraintToggle = false;
        }
    }

    protected float GetDistanceFromTarget()
    {
        return Vector3.Distance(gameObject.transform.position, target.transform.position);
    }

    protected void FaceTarget()
    {
        Quaternion lookRot = Quaternion.LookRotation(
            target.transform.position - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            lookRot,
            Time.deltaTime * lookTurningSpeed);
    }
}
