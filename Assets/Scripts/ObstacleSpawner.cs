using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Lists")]
    public List<GameObject> leftLaneObstacles; // Obstacles for the left lane
    public List<GameObject> centerLaneObstacles; // Obstacles for the center lane
    public List<GameObject> rightLaneObstacles; // Obstacles for the right lane

    [Header("Difficulty Modes")]
    public bool easy = true;
    public bool medium = false;
    public bool hard = false;

    [Header("Spawn Timing Settings")]
    public float minSpawnInterval = 3f; // Minimum time between spawns
    public float maxSpawnInterval = 6f; // Maximum time between spawns

    [Header("Camera Reference")]
    public Transform cameraTransform; // Reference to the main camera

    [Header("Debugging")]
    public float leftTimer = 0f;
    public float centerTimer = 0f;
    public float rightTimer = 0f;

    private float screenWidth; // Horizontal world bounds
    private float screenHeight; // Vertical world bounds

    private bool firstObstacleSpawned = false; // Track if the first obstacle has been spawned
    private Vector3 lastCameraPosition; // Tracks the last position of the camera

    void Start()
    {
        // Dynamically calculate screen bounds based on the camera
        Camera mainCamera = Camera.main;
        screenHeight = mainCamera.orthographicSize * 2f; // Vertical size is twice the orthographic size
        screenWidth = screenHeight * mainCamera.aspect; // Horizontal size depends on aspect ratio

        // Immediately spawn the first obstacle
        SpawnSingleLaneObstacle();
        firstObstacleSpawned = true;

        // Randomize initial timers
        leftTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        centerTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        rightTimer = Random.Range(minSpawnInterval, maxSpawnInterval);

        if (cameraTransform != null)
        {
            lastCameraPosition = cameraTransform.position;
        }
    }

    void Update()
    {
        if (cameraTransform != null && cameraTransform.position.y > lastCameraPosition.y)
        {
            if (easy)
            {
                EasyModeSpawning();
            }
            else if (medium)
            {
                // Logic for medium mode can be added here
            }
            else if (hard)
            {
                // Logic for hard mode can be added here
            }

            // Update last camera position
            lastCameraPosition = cameraTransform.position;
        }
    }

    void EasyModeSpawning()
    {
        if (!firstObstacleSpawned)
        {
            SpawnSingleLaneObstacle();
            firstObstacleSpawned = true;
        }

        leftTimer -= Time.deltaTime;
        centerTimer -= Time.deltaTime;
        rightTimer -= Time.deltaTime;

        if (leftTimer <= 0f || centerTimer <= 0f || rightTimer <= 0f)
        {
            SpawnSingleLaneObstacle();

            // Reset the timer for the next spawn
            leftTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
            centerTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
            rightTimer = Random.Range(minSpawnInterval, maxSpawnInterval);
        }
    }

    void SpawnSingleLaneObstacle()
    {
        // Randomly select one of the three lanes
        float[] lanePositions = { -screenWidth / 3f, 0f, screenWidth / 3f };
        int laneIndex = Random.Range(0, lanePositions.Length);
        float xPosition = lanePositions[laneIndex];
        GameObject selectedObstacle = null;

        if (laneIndex == 1) // Middle lane
        {
            // Randomize X position within the middle part of the screen
            xPosition = Random.Range(-screenWidth / 6f, screenWidth / 6f);

            // Select a random obstacle from the center lane list
            if (centerLaneObstacles.Count > 0)
            {
                selectedObstacle = centerLaneObstacles[Random.Range(0, centerLaneObstacles.Count)];
            }
        }
        else if (laneIndex == 0 && leftLaneObstacles.Count > 0) // Left lane
        {
            selectedObstacle = leftLaneObstacles[Random.Range(0, leftLaneObstacles.Count)];
            if (selectedObstacle != null)
            {
                xPosition = selectedObstacle.transform.position.x; // Use the prefab's original X position
            }
        }
        else if (laneIndex == 2 && rightLaneObstacles.Count > 0) // Right lane
        {
            selectedObstacle = rightLaneObstacles[Random.Range(0, rightLaneObstacles.Count)];
            if (selectedObstacle != null)
            {
                xPosition = selectedObstacle.transform.position.x; // Use the prefab's original X position
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
        }
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
