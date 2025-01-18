using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

public class TestResMgr : MonoBehaviour
{
	// Update is called once per frame
	void Update()
	{
		if(Input.GetMouseButtonDown(1))
		{
			ResMgr.Instance.LoadAsync<GameObject>("TestPool/Cube", (obj) =>
			{
				Debug.Log("Test ResMgr: " + obj.name);
			});
		}
	}
}
