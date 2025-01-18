using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

public class TestInput : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		InputMgr.Instance.StartOrEndCheck(true);
		
		EventCenter.Instance.AddEventListener<KeyCode>(E_EventType.E_Key_Down, CheckInputDown);
		EventCenter.Instance.AddEventListener<KeyCode>(E_EventType.E_Key_Up, CheckInputUp);
	}

	private void CheckInputDown(KeyCode code)
	{
		switch(code)
		{
			case KeyCode.W:
				Debug.Log("W Down");
				break;
			case KeyCode.A:
				Debug.Log("A Down");
				break;
			case KeyCode.S:
				Debug.Log("S Down");
				break;
			case KeyCode.D:
				Debug.Log("D Down");
				break;
		}	
	}
	
	private void CheckInputUp(KeyCode code)
	{
		switch(code)
		{
			case KeyCode.W:
				Debug.Log("W Up");
				break;
			case KeyCode.A:
				Debug.Log("A Up");
				break;
			case KeyCode.S:
				Debug.Log("S Up");
				break;
			case KeyCode.D:
				Debug.Log("D Up");
				break;
		}	
	}
}
