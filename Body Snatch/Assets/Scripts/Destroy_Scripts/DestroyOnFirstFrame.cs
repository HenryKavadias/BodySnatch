using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnFirstFrame : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject);
    }
}
