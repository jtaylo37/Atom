using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atom : MonoBehaviour
{
    public int particleCount = 10;
    public float minRadius = 5f;
    public float maxRadius = 20f;
    public float gConst = 10f;
    public GameObject nucleus;
    public GameObject particlePrefab;
    public GameObject[] particles;

    void Awake()
    {
        if (nucleus == null)
            nucleus = gameObject;
        
        Rigidbody nucleusRb = nucleus.GetComponent<Rigidbody>();
        if (nucleusRb == null)
            nucleusRb = nucleus.AddComponent<Rigidbody>();

        nucleusRb.useGravity = false;
        nucleusRb.isKinematic = true;

        particles = new GameObject[particleCount];

        for (int i = 0; i < particleCount; i++)
        {
            particles[i] = particlePrefab != null ? Instantiate(particlePrefab) : GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Rigidbody particleRb = particles[i].AddComponent<Rigidbody>();
            particleRb.useGravity = false;
            particleRb.mass = Random.Range(0.1f, 0.4f);

            float radius = Random.Range(minRadius, maxRadius);
            float angle = Random.Range(0f, 360f);
            float height = Random.Range(-maxRadius, maxRadius);

            Vector3 position = new Vector3(radius * Mathf.Cos(angle), height, radius * Mathf.Sin(angle));
            particles[i].transform.position = nucleus.transform.position + position;

            Vector3 orbitalPlaneNormal = Vector3.Cross(position, Vector3.up).normalized;
            Vector3 velocityDirection = Vector3.Cross(orbitalPlaneNormal, position.normalized);
            float velocityMagnitude = Mathf.Sqrt(gConst * nucleusRb.mass / radius);

            particleRb.velocity = velocityDirection * velocityMagnitude;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < particleCount; i++)
        {
            Vector3 toNucleus = nucleus.transform.position - particles[i].transform.position;
            float distance = toNucleus.magnitude;
            Vector3 forceDirection = toNucleus.normalized;
            float forceMagnitude = gConst * nucleus.GetComponent<Rigidbody>().mass * particles[i].GetComponent<Rigidbody>().mass / (distance * distance);
            Vector3 force = forceDirection * forceMagnitude;
            
            particles[i].GetComponent<Rigidbody>().AddForce(force);
        }
    }
}