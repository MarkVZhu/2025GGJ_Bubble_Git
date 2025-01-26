using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
	public GameObject[] backgroundPrefabs; // The background prefab
	public Transform cameraTransform;  // The camera's transform
	public float backgroundHeight = 10; // Height of the background prefab
	public float initialOffset = 1f; // Initial generated at background.y - initialOffset 
	private ObstacleSpawner obstacleSpawner;

	private Queue<GameObject> activeBackgrounds = new Queue<GameObject>();
	private float spawnThreshold; // The Y-position threshold for spawning a new background
	private float lastSpawnY; // The Y-position of the last spawned background

	void Start()
	{
		// Initialize the spawn threshold
		spawnThreshold = cameraTransform.position.y + backgroundHeight;
		obstacleSpawner = GetComponent<ObstacleSpawner>();
		
		// Spawn the initial backgrounds to fill the screen
		for (int i = 0; i < 2; i++)
		{
			SpawnBackground(i * backgroundHeight); // Spawn backgrounds stacked vertically
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
			spawnThreshold = lastSpawnY;

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
		GameObject newBackground = Instantiate(backgroundPrefabs[obstacleSpawner.CheckDifficulty()], new Vector3(0, yPosition - initialOffset, 0), Quaternion.identity);

		// Add the new background to the queue
		activeBackgrounds.Enqueue(newBackground);

		// Update the last spawn Y position
		lastSpawnY = yPosition;
	}
}
