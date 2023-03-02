using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    // Destroys any object that exits the boundary
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}
