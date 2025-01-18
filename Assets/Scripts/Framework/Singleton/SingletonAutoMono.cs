using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarkFramework
{
	//Don't need to drag script ot add by API
	public class SingletonAutoMono<T> : MonoBehaviour where T: MonoBehaviour
	{
		private static T instance;
		public static T Instance
		{
			get
			{
				if(instance == null)
				{
					GameObject obj = new GameObject();
					//Set object's name as this script's name : Reflection
					obj.name = typeof(T).ToString();
					instance = obj.AddComponent<T>();
					DontDestroyOnLoad(obj);
				}
				return instance;
			}
		}
	}
}
