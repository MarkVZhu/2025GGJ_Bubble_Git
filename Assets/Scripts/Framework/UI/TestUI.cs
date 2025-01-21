using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

public class TestUI : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		//UIManager.Instance.ShowPanel<MainPanel>("MainPanel");
		UIManager.Instance.ShowPanel<PausePanel>("PausePanel");
		Debug.Log("不同屏幕尺寸请调整Canvas Prefab中的Canvas Scaler组件");
		//Invoke("HidePanel",1f);
	}

	// Update is called once per frame
	void HidePanel()
	{
		UIManager.Instance.HidePanel("MainPanel");
	}
}
