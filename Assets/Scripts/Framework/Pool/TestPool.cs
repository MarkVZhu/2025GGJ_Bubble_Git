using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

public class TestPool : MonoBehaviour
{

	// Update is called once per frame
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			PoolManager.Instance.GetObj("TestPool/Cube");
		}
		if(Input.GetMouseButtonDown(1))
		{
			PoolManager.Instance.GetObj("TestPool/Sphere");
		}
	}
}
