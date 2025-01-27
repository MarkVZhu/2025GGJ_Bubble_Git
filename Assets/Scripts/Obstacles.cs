using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
	[Header("Spawn Level Settings")]
	public int minSpawnLevel = 1; // Minimum difficulty level required for this obstacle to spawn
	public int maxSpawnLevel = 10; // Maximum difficulty level for this obstacle to spawn

	private bool hasCollided = false; // Tracks if the collision has already been logged

	void OnTriggerEnter2D(Collider2D other)
	{
		// Check if the colliding object has the "Player" tag (assuming the bubble is tagged "Player")
		if (other.CompareTag("Player") && !hasCollided)
		{
			hasCollided = true; // Mark the collision as logged

			// Get the SpriteRenderer component of the bubble
			SpriteRenderer bubbleRenderer = other.GetComponent<SpriteRenderer>();

			if (bubbleRenderer != null)
			{
				// Change the bubble's color to red
				bubbleRenderer.color = Color.red;
			}

			// Debug log the collision with the obstacle's name
			Debug.Log($"Bubble collided with {gameObject.name}.");
			GameManager.Instance.EndGame();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		// Reset the collision state when the bubble exits the obstacle
		if (other.CompareTag("Player"))
		{
			hasCollided = false;
		}
	}
}
