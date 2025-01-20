using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using UnityEngine;

public class TestRankUI : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		UIManager.Instance.ShowPanel<RankInputPanel>("RankInputPanel");
		Debug.Log("不同屏幕尺寸请调整Canvas Prefab中的Canvas Scaler组件");
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
