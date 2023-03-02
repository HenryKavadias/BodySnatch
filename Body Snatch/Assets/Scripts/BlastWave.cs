using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastWave : MonoBehaviour
{
    public int pointsCount = 100;
    public float maxRadius = 50;
    public float speed = 5;
    public float startWidth = 5;
    public float force = 5;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = pointsCount + 1;
    }

    private void Start()
    {
        StartCoroutine(Blast());
    }

    private void Damage(float currentRadius)
    {
        Collider[] hittingObjects = Physics.OverlapSphere(transform.position, currentRadius);

        for (int i = 0; i < hittingObjects.Length; i++)
        {
            Rigidbody rb = hittingObjects[i].GetComponent<Rigidbody>();

            if (!rb)
            {
                continue;
            }

            // Adds force to the objects the blast hits

            Vector3 direction = (hittingObjects[i].transform.position - transform.position).normalized;
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
    }

    private IEnumerator Blast()
    {
        float currentRadius = 0f;

        while (currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);

            Damage(currentRadius);

            yield return null;
        }
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360f / pointsCount;

        for (int i = 0; i <= pointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;

            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius / maxRadius);
    }

    // test controls
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            StartCoroutine(Blast());
        }
    }
}
