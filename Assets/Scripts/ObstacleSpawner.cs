using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
	[Header("Camera Reference")]
	public Transform cameraTransform; // Reference to the main camera

	[Header("Obstacle Lists")]
	public List<GameObject> leftLaneObstacles; // Obstacles for the left lane
	public List<GameObject> centerLaneObstacles; // Obstacles for the center lane
	public List<GameObject> rightLaneObstacles; // Obstacles for the right lane

	[Header("Game Progression")]
	//Please change the Spawn Level Settings accordingly
	public int difficultyLevel = 0; // Tracks the current difficulty level
	public int mediumModeThreshold = 5; // Score threshold to switch to medium mode
	public int hardModeThreshold = 15; // Score threshold to switch to hard mode
	public int scoreThreshold = 15; // Score threshold to increase difficulty level

	[Header("Difficulty Modes")]
	public bool easy = true;
	public bool medium = false;
	public bool hard = false;

	[Header("Spawn Timing Settings - Easy")]
	public float easyMinSpawnInterval = 4f; // Minimum time between spawns for easy mode
	public float easyMaxSpawnInterval = 8f; // Maximum time between spawns for easy mode

	[Header("Spawn Timing Settings - Medium")]
	public float mediumMinSpawnInterval = 3f; // Minimum time between spawns for medium mode
	public float mediumMaxSpawnInterval = 7f; // Maximum time between spawns for medium mode

	[Header("Spawn Timing Settings - Hard")]
	public float hardMinSpawnInterval = 2f; // Minimum time between spawns for hard mode
	public float hardMaxSpawnInterval = 6f; // Maximum time between spawns for hard mode

	[Header("Debugging")]
	public float leftTimer = 0f;
	public float centerTimer = 0f;
	public float rightTimer = 0f;

	private float screenWidth; // Horizontal world bounds
	private float screenHeight; // Vertical world bounds

	private bool firstObstacleSpawned = false; // Track if the first obstacle has been spawned
	private Vector3 lastCameraPosition; // Tracks the last position of the camera

	private ProgressTracker progressTracker; // Reference to the ProgressTracker script

	public float timerMinSpeedThreshold = 0.1f; // Minimum speed to keep the timer running

	void Start()
	{
		// Dynamically calculate screen bounds based on the camera
		Camera mainCamera = Camera.main;
		screenHeight = mainCamera.orthographicSize * 2f; // Vertical size is twice the orthographic size
		screenWidth = screenHeight * mainCamera.aspect; // Horizontal size depends on aspect ratio

		// Immediately spawn the first obstacle
		SpawnSingleLaneObstacle();
		firstObstacleSpawned = true;

		// Randomize initial timers for easy mode
		leftTimer = Random.Range(easyMinSpawnInterval, easyMaxSpawnInterval);
		centerTimer = Random.Range(easyMinSpawnInterval, easyMaxSpawnInterval);
		rightTimer = Random.Range(easyMinSpawnInterval, easyMaxSpawnInterval);

		if (cameraTransform != null)
		{
			lastCameraPosition = cameraTransform.position;
		}

		// Get the ProgressTracker script reference
		progressTracker = FindObjectOfType<ProgressTracker>();
	}

	void Update()
	{
		float cameraSpeed = cameraTransform.position.y - lastCameraPosition.y;
		if (IsCameraMovingUpwards())
		{
			if (cameraTransform != null && cameraTransform.position.y > lastCameraPosition.y)
			{
				// Check and update difficulty level based on score
				if (progressTracker != null && progressTracker.score >= (difficultyLevel + 1) * scoreThreshold)
				{
					difficultyLevel++;
				}

				if (difficultyLevel >= hardModeThreshold)
				{
					easy = false;
					medium = false;
					hard = true;
				}
				else if (difficultyLevel >= mediumModeThreshold)
				{
					easy = false;
					medium = true;
				}

				if (easy)
				{
					EasyModeSpawning();
				}
				else if (medium)
				{
					MediumModeSpawning();
				}
				else if (hard)
				{
					HardModeSpawning();
				}

				// Update last camera position
				lastCameraPosition = cameraTransform.position;
			}
		}
	}

	bool IsCameraMovingUpwards()
	{
		return cameraTransform.position.y - lastCameraPosition.y > timerMinSpeedThreshold;
	}

	void EasyModeSpawning()
	{
		if (!firstObstacleSpawned)
		{
			SpawnSingleLaneObstacle();
			firstObstacleSpawned = true;
		}

		if (!IsCameraMovingUpwards())
		{
			return; // Pause timers if bubble is moving too slowly or downwards
		}

		leftTimer -= Time.deltaTime;
		centerTimer -= Time.deltaTime;
		rightTimer -= Time.deltaTime;

		if (leftTimer <= 0f || centerTimer <= 0f || rightTimer <= 0f)
		{
			SpawnSingleLaneObstacle();

			// Reset the timer for the next spawn
			leftTimer = Random.Range(easyMinSpawnInterval, easyMaxSpawnInterval);
			centerTimer = Random.Range(easyMinSpawnInterval, easyMaxSpawnInterval);
			rightTimer = Random.Range(easyMinSpawnInterval, easyMaxSpawnInterval);
		}
	}

	void MediumModeSpawning()
	{
		if (!IsCameraMovingUpwards())
		{
			return; // Pause timers if the camera is moving too slowly or downwards
		}

		leftTimer -= Time.deltaTime;
		centerTimer -= Time.deltaTime;
		rightTimer -= Time.deltaTime;

		// Limit the number of lanes that can spawn obstacles at the same time
		int lanesSpawned = 0;

		if (leftTimer <= 0f && lanesSpawned < 2)
		{
			SpawnLaneObstacle(0);
			leftTimer = Random.Range(mediumMinSpawnInterval, mediumMaxSpawnInterval);
			lanesSpawned++;
		}

		if (centerTimer <= 0f && lanesSpawned < 2)
		{
			SpawnLaneObstacle(1);
			centerTimer = Random.Range(mediumMinSpawnInterval, mediumMaxSpawnInterval);
			lanesSpawned++;
		}

		if (rightTimer <= 0f && lanesSpawned < 2)
		{
			SpawnLaneObstacle(2);
			rightTimer = Random.Range(mediumMinSpawnInterval, mediumMaxSpawnInterval);
			lanesSpawned++;
		}
	}

	void HardModeSpawning()
	{
		if (!IsCameraMovingUpwards())
		{
			return; // Pause timers if bubble is moving too slowly or downwards
		}

		leftTimer -= Time.deltaTime;
		centerTimer -= Time.deltaTime;
		rightTimer -= Time.deltaTime;

		// Each lane spawns obstacles independently
		if (leftTimer <= 0f)
		{
			SpawnLaneObstacle(0);
			leftTimer = Random.Range(hardMinSpawnInterval, hardMaxSpawnInterval);
		}

		if (centerTimer <= 0f)
		{
			SpawnLaneObstacle(1);
			centerTimer = Random.Range(hardMinSpawnInterval, hardMaxSpawnInterval);
		}

		if (rightTimer <= 0f)
		{
			SpawnLaneObstacle(2);
			rightTimer = Random.Range(hardMinSpawnInterval, hardMaxSpawnInterval);
		}
	}

	void SpawnSingleLaneObstacle()
	{
		int laneIndex = Random.Range(0, 3);
		SpawnLaneObstacle(laneIndex);
	}

	void SpawnLaneObstacle(int laneIndex)
	{
		float[] lanePositions = { -screenWidth / 3f, 0f, screenWidth / 3f };
		float xPosition = lanePositions[laneIndex];
		GameObject selectedObstacle = null;

		List<GameObject> laneObstacles = laneIndex switch
		{
			0 => leftLaneObstacles,
			1 => centerLaneObstacles,
			2 => rightLaneObstacles,
			_ => null
		};

		if (laneObstacles != null && laneObstacles.Count > 0)
		{
			List<GameObject> validObstacles = laneObstacles.FindAll(obstacle =>
			{
				Obstacles obstacleScript = obstacle.GetComponent<Obstacles>();
				return obstacleScript != null &&
					   difficultyLevel >= obstacleScript.minSpawnLevel &&
					   difficultyLevel <= obstacleScript.maxSpawnLevel;
			});

			if (validObstacles.Count > 0)
			{
				selectedObstacle = validObstacles[Random.Range(0, validObstacles.Count)];

				if (laneIndex == 1) // Middle lane
				{
					xPosition = Random.Range(-screenWidth / 6f, screenWidth / 6f);
				}
				else
				{
					xPosition = selectedObstacle.transform.position.x; // Use the prefab's original X position
				}
			}
		}

		// Calculate the spawn position above the camera
		float spawnY = Camera.main.transform.position.y + screenHeight / 2f + 1f; // Spawn 1 unit above the top of the visible screen

		Vector3 spawnPosition = new Vector3(xPosition, spawnY, 0f);

		// Instantiate the selected obstacle if valid
		if (selectedObstacle != null)
		{
			GameObject obstacle = Instantiate(selectedObstacle, spawnPosition, selectedObstacle.transform.rotation);
			obstacle.AddComponent<DestroyOffScreen>().Initialize(Camera.main, screenHeight);

			// Check for nearby obstacles and delete one if necessary
			CheckForNearbyObstacles(obstacle);
		}
	}

	void CheckForNearbyObstacles(GameObject currentObstacle)
	{
		float checkRadius = 1f; // Adjust the radius as needed
		Collider2D[] nearbyObstacles = Physics2D.OverlapCircleAll(currentObstacle.transform.position, checkRadius);

		int nearbyCount = 0;

		foreach (Collider2D obstacle in nearbyObstacles)
		{
			if (obstacle.gameObject != currentObstacle && obstacle.transform.position.y > cameraTransform.position.y + screenHeight / 2f)
			{
				nearbyCount++;

				// If more than two obstacles are nearby, randomly remove one
				if (nearbyCount > 2)
				{
					Destroy(obstacle.gameObject);
					break; // Ensure only one obstacle is removed
				}
			}
		}
	}
	
	public int CheckDifficulty()
	{
		if(easy) return 0;
		if(medium) return 1;
		if(hard) return 2;
		else
		 return -1;	
	}
}

public class DestroyOffScreen : MonoBehaviour
{
	private Camera mainCamera;
	private float screenHeight;

	public void Initialize(Camera camera, float height)
	{
		mainCamera = camera;
		screenHeight = height;
	}

	void Update()
	{
		// Check if the obstacle is below the bottom of the screen
		if (transform.position.y < mainCamera.transform.position.y - screenHeight / 2f - 1f)
		{
			Destroy(gameObject);
		}
	}
}
