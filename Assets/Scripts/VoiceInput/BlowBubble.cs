using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowBubble : MonoBehaviour
{
	public AudioSource source;
	public float maxScale;
	public AudioLoudnessDetection detector;

	private float loudnessSensibility = 100;
	
	//控制吹满泡泡的速度，数字越大速度越慢，50时全程吹满大概20帧
	[Tooltip("控制吹满泡泡的速度，数字越大速度越慢")]
	public float IncreaseDivider = 50;

	void Start()
	{
		
	}


	void Update()
	{
		float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
		loudness = GetLoudnessLevel(loudness);
		Debug.Log("Loudness: " + loudness);
		
		float intervalT = loudness/IncreaseDivider;

		if(transform.localScale.x < maxScale)
			transform.localScale += new Vector3(intervalT, intervalT, intervalT);
		else
			Debug.Log("Bubble Explode!");
	}
	
	float GetLoudnessLevel(float loudness)
	{ 
		if(loudness < 0.1)
		{
			return 0;
		}
		else if(loudness < 1)
		{
			return 0.75f;
		}
		else if(loudness < 2)
		{
			return 1.5f;
		}
		else if(loudness < 3)
		{
			return 2.5f;
		}
		else if(loudness < 5)
		{
			return 4f;
		}
		else
		{
			return 5f;
		}
	}
}
