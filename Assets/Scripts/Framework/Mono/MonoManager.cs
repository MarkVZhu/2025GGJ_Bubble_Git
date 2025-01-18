using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;

namespace MarkFramework
{
	/// <summary>
	/// 1. Provide a method to add frame update for external scripts
	/// 2. Provide a method to add coroutine for external scripts
	/// </summary>
	public class MonoManager : BaseManager<MonoManager>
	{
		public MonoController controller;
		
		private MonoManager()
		{
			GameObject obj = new GameObject("MonoController");
			controller = obj.AddComponent<MonoController>();
		}
		
		// public MonoManager()
		// {
		// 	GameObject obj = new GameObject("MonoController");
		// 	controller = obj.AddComponent<MonoController>();
		// }
		
		public void AddUpdateListener(UnityAction fun)
		{
			controller.AddUpdateListener(fun);
		}
		
		public void RemoveUpdateListener(UnityAction fun)
		{
			controller.AddUpdateListener(fun);
		}

		public Coroutine StartCoroutine(IEnumerator routine)
		{
			return controller.StartCoroutine(routine);
		}

		public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
		{
			return controller.StartCoroutine(methodName, value);
		}

		public Coroutine StartCoroutine(string methodName)
		{
			return controller.StartCoroutine(methodName);
		}
		
		public void StopCoroutine(Coroutine routine)
		{
			controller.StopCoroutine(routine);
		}
	}
}
