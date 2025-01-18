using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class TestBaseMgrWLock: BaseManager<TestBaseMgrWLock>
{
	private List<object> list = new List<object>();
	
	private TestBaseMgrWLock()
	{
		
	}
	
	public void TestLog()
	{
		Debug.Log("TestLog");
	}
	
	public void AddData(object data)
	{
		lock(lockObj)
		{
			list.Add(data);
		}
	}
}