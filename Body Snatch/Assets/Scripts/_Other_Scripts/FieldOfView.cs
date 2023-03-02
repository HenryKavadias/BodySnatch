using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius = 3;
    [Range(0, 360)]
    public float angle = 100;

    public GameObject targetRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeeTarget;

    private void Start()
    {
        targetRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = null;

            // could do a for loop to search for the player object
            // if there is more than one object on the layer
            target = rangeChecks[0].transform;

            // Only set up for the first player it finds
            //foreach (Collider agent in rangeChecks)
            //{
            //    if (agent.tag == "Player")
            //    {
            //        target = agent.transform;
            //        break;
            //    }
            //}

            //if (target == null)
            //{
            //    return;
            //}

            // Check if target on mask is player
            //if (rangeChecks[0].tag == "Player")
            //{
            //    target = rangeChecks[0].transform;
            //}
            //else
            //{
            //    return;
            //}

            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeeTarget = true;
                }
                else
                {
                    canSeeTarget = false;
                }
            }
            else
            {
                canSeeTarget = false;
            }
        }
        else if (canSeeTarget) 
        { 
            canSeeTarget = false;
        }
    }
}

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if (fov.canSeeTarget)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.targetRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Rad2Deg), 0, Mathf.Cos(angleInDegrees * Mathf.Rad2Deg));
    }
}