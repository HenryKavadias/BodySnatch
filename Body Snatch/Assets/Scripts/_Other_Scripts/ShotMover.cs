using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMover : MonoBehaviour
{
    public float speed;         // Speed of projectile
    public float inaccuracy = 0;  // Inaccuracy of projectile

    void Start()
    {
        // Alter the direction of the of projectile randomly
        // Based on the accuracy modiefier
        if (inaccuracy != 0)
        {
            // Rotates the projectile object on its Y access transform
            GetComponent<Transform>().Rotate(
                0.0f, Random.Range(-inaccuracy, inaccuracy), 0.0f);
        }
        // Sets the speed of the projectile 
        // (its front is the postive side of the Z-axis)
        GetComponent<Rigidbody>().velocity = 
            transform.forward * speed;
    }

    public float GetSpeed()
    {
        return speed;
    }
}