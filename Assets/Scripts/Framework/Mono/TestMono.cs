using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

public class TestMono : MonoBehaviour
{	
	void Start()
	{
		TestMonoInstance t = new TestMonoInstance();
		MonoManager.Instance.AddUpdateListener(t.Update);
	}
}

public class TestMonoInstance
{
	public TestMonoInstance()
	{
		MonoManager.Instance.StartCoroutine(TestCoroutine());
	}
	
	public void Update()
	{
		Debug.Log("Test Mono Instance");
	}
	
	IEnumerator TestCoroutine()
	{
		yield return new WaitForSeconds(1f);
		Debug.Log("123123");
	}
}

