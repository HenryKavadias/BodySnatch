using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to make in world UI objects face the camera
public class Billboard : MonoBehaviour
{
    private Camera _cam;

    // Get Camera
    void Start()
    {
        _cam = Camera.main;
    }

    // Make object face camera
    void LateUpdate()
    {
        transform.LookAt(transform.position + _cam.transform.forward);
    }
}
