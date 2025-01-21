using UnityEngine;
using TMPro; // Required for TextMeshPro

public class ProgressTracker : MonoBehaviour
{
    public Camera mainCamera; // Reference to the main camera
    public float unitsPerPoint = 5f; // Distance in units required to gain one point
    public TextMeshProUGUI scoreText; // TextMeshProUGUI to display the score

    public int score = 0; // Current score
    private float lastScoreY; // Tracks the Y position where the last score increment occurred

    void Start()
    {
        if (mainCamera != null)
        {
            // Initialize lastScoreY with the camera's starting Y position
            lastScoreY = mainCamera.transform.position.y;
            UpdateScoreUI();
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            float currentCameraY = mainCamera.transform.position.y;

            // Check if the camera has moved enough to gain a point
            if (currentCameraY - lastScoreY >= unitsPerPoint)
            {
                // Increment the score
                score++;

                // Update the last scoring position
                lastScoreY += unitsPerPoint;

                // Update the UI
                UpdateScoreUI();
            }
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}
