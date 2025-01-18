using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MarkFramework
{
	/// <summary>
	/// Mono Manager
	/// </summary>
	public class MonoController : MonoBehaviour
	{
		public event UnityAction updateEvent;
		
		void Start()
		{
			DontDestroyOnLoad(this.gameObject);
		}

		void Update()
		{
			if (updateEvent != null)
				updateEvent();
		}
		
		/// <summary>
		/// To the external, add frame update event
		/// </summary>
		/// <param name="fun"></param>
		public void AddUpdateListener(UnityAction fun)
		{
			updateEvent += fun;
		}
		
		/// <summary>
		/// To the external, remove frame update event
		/// </summary>
		/// <param name="fun"></param>
		public void RemoveUpdateListener(UnityAction fun)
		{
			updateEvent -= fun;
		}
	}

}