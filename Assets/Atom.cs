using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atoms : MonoBehaviour
{
    public int particleCount = 10;
    public float minRadius = 5f;
    public float maxRadius = 20f;
    public float speed = 5f; // Angular speed of the electrons
    public GameObject nucleus;
    public GameObject particlePrefab;
    public GameObject[] particles;

    void Awake()
    {
        if (nucleus == null)
            nucleus = gameObject;

        // Change the nucleus color to distinguish it visually.
        Renderer nucleusRenderer = nucleus.GetComponent<Renderer>();
        if (nucleusRenderer == null)
            nucleusRenderer = nucleus.AddComponent<MeshRenderer>();
        nucleusRenderer.material.color = Color.red;

        particles = new GameObject[particleCount];

        for (int i = 0; i < particleCount; i++)
        {
            particles[i] = particlePrefab != null ? Instantiate(particlePrefab) : GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Renderer particleRenderer = particles[i].GetComponent<Renderer>();
            particleRenderer.material.color = Color.cyan;

            float radius = Random.Range(minRadius, maxRadius);
            float angle = Random.Range(0f, 360f);
            float height = Random.Range(-maxRadius, maxRadius);

            Vector3 position = new Vector3(radius * Mathf.Cos(angle), height, radius * Mathf.Sin(angle));
            particles[i].transform.position = nucleus.transform.position + position;
        }
    }

    void Update()
    {
        for (int i = 0; i < particleCount; i++)
        {
            Transform particle = particles[i].transform;
            float radius = Vector3.Distance(particle.position, nucleus.transform.position);
            float angle = Mathf.Atan2(particle.position.z - nucleus.transform.position.z, particle.position.x - nucleus.transform.position.x);
            angle += speed * Time.deltaTime;
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);
            particle.position = new Vector3(x, particle.position.y, z) + nucleus.transform.position;
        }
    }
}