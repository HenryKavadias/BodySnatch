using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    // Length of time an object
    // is present for
    public float lifetime = 0.5f;
    public float range = 0;
    
    public bool rangedAtk = false;

    private float speed = 0;

    // Stores the main camera
    private Camera cam;
    
    // Should only be called for ranged attacks
    public void SetRange(float rangeOfAtk)
    {
        if (rangedAtk && GetComponent<ShotMover>() && !GetComponent<AgentTriggerDamager>().melee)
        {
            range = rangeOfAtk;
            speed = GetComponent<ShotMover>().speed;
            CalculateLifeTime();
            StartLifeTime();
        }
        else
        {
            Debug.Log("Error, not set as ranged attack / conflicting bool.");
        }
    }

    void CalculateLifeTime()
    {
        lifetime = range / speed;
    }

    void StartLifeTime()
    {
        Destroy(gameObject, lifetime);
    }

    // Finds the main camera in the scene
    void FindMainCamera()
    {
        GameObject camera = GameObject.FindWithTag("MainCamera");

        if (camera != null)
        {
            cam = camera.GetComponent<Camera>();
        }
        if (cam == null)
        {
            Debug.Log("Cannot find 'Camera'");
        }
    }

    void Start()
    {
        if (gameObject.tag == "TextEffect")
        {
            FindMainCamera();

            gameObject.transform.LookAt(Camera.main.transform);
            transform.rotation = 
                Quaternion.LookRotation(Camera.main.transform.forward);
        }

        //Debug.Log(transform.position);

        if (!rangedAtk)
        {
            StartLifeTime();
        }
    }
}
