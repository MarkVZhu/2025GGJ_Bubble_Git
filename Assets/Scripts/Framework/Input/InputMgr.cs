using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarkFramework
{
	public class InputMgr : BaseManager<InputMgr>
	{
		private bool isStart = false;
		private InputMgr()
		{
			MonoManager.Instance.AddUpdateListener(MyUpdate);
		}
		
		public void StartOrEndCheck(bool isOpen)
		{
			isStart = isOpen;
		}
		
		private void MyUpdate()
		{
			if(!isStart)
				return;
			CheckKeyCode(KeyCode.W);
			CheckKeyCode(KeyCode.A);
			CheckKeyCode(KeyCode.S);
			CheckKeyCode(KeyCode.D);
		}
		
		private void CheckKeyCode(KeyCode key)
		{
			if(Input.GetKeyDown(key))
				EventCenter.Instance.EventTrigger(E_EventType.E_Key_Down, key);
			if(Input.GetKeyUp(key))
				EventCenter.Instance.EventTrigger(E_EventType.E_Key_Up, key);
		}
	}
}
