using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class TestModel : ViewModel
{
	private string testString;
	private int testInt;
	public string TestString
	{
		get => testString;
		set => ChangePropertyAndNotify(ref testString, value); //TODO:传数据集合，数据集合如何表现？
	}
	
	// 以计时器演示ViewModel的作用
	void Start()
	{
		InvokeRepeating("NewTestString", 1, 1);
	}

	void NewTestString()
	{
		TestString = testInt + ""; //改变数据，通过ViewModel通知UI层
		testInt++;
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
}
