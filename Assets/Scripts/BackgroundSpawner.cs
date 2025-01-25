using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    public GameObject backgroundPrefab; // The background prefab
    public Transform cameraTransform;  // The camera's transform
    public float backgroundHeight = 1920f; // Height of the background prefab

    private Queue<GameObject> activeBackgrounds = new Queue<GameObject>();
    private float spawnThreshold; // The Y-position threshold for spawning a new background
    private float lastSpawnY; // The Y-position of the last spawned background

    void Start()
    {
        // Initialize the spawn threshold
        spawnThreshold = cameraTransform.position.y + backgroundHeight;

        // Spawn the initial backgrounds to fill the screen
        for (int i = 0; i < 2; i++)
        {
            SpawnBackground(i * backgroundHeight);
        }
    }

    void Update()
    {
        // Check if the camera has reached the spawn threshold
        if (cameraTransform.position.y + backgroundHeight > spawnThreshold)
        {
            // Spawn a new background above the last one
            SpawnBackground(lastSpawnY + backgroundHeight);

            // Update the spawn threshold
            spawnThreshold = lastSpawnY + backgroundHeight;

            // Remove the bottom background to optimize performance
            if (activeBackgrounds.Count > 3) // Keep only 3 backgrounds in the scene
            {
                Destroy(activeBackgrounds.Dequeue());
            }
        }
    }

    void SpawnBackground(float yPosition)
    {
        // Instantiate the background prefab
        GameObject newBackground = Instantiate(backgroundPrefab, new Vector3(0, yPosition, 0), Quaternion.identity);
        activeBackgrounds.Enqueue(newBackground);

        // Update the last spawn Y position
        lastSpawnY = yPosition;
    }
}
