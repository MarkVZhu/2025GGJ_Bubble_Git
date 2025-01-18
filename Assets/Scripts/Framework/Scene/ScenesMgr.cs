using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MarkFramework
{
	/// <summary>
	/// Change Scenes
	/// 1. Asynchronous loading
	/// 2. Coroutine
	/// 3. Delegate
	/// </summary>
	public class ScenesMgr : BaseManager<ScenesMgr>
	{
		private ScenesMgr()
		{
			
		}
		
		/// <summary>
		/// Load Scene: Synchronize
		/// </summary>
		/// <param name="name"></param>
		public void LoadScene(string name, UnityAction fun)
		{
			SceneManager.LoadScene(name);
			fun();
		}
		
		public void LoadSceneAsyn(string name, UnityAction fun)
		{
			MonoManager.Instance.StartCoroutine(RealLoadSceneAsyn(name, fun));
		}
		
		/// <summary>
		/// Coroutine Load Scene Asynchronously
		/// </summary>
		/// <param name="name"></param>
		/// <param name="fun"></param>
		/// <returns></returns>
		private IEnumerator RealLoadSceneAsyn(string name, UnityAction fun)
		{
			AsyncOperation ao = SceneManager.LoadSceneAsync(name);
			
			//Progress value
			while(!ao.isDone)
			{
				//EventCenter post ao.progress to external objects
				EventCenter.Instance.EventTrigger(E_EventType.E_Progress_Update, ao.progress);
				yield return ao.progress;
			}
			
			// Do fun() after loading
			fun();
		}
	}
}
