using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

public class Task : MonoBehaviour
{
	void Start()
	{
		//EventCenter.Instance.AddEventListener<Monster>("MonsterDie", MonsterDieTodo);
		EventCenter.Instance.AddEventListener(E_EventType.E_Monster_Dead, MonsterDieTodo);
	}
	
	private void MonsterDieTodo(Monster info)
	{
		Debug.Log("Monster Die Task: " + info.name);
	}
	
	private void MonsterDieTodo()
	{
		Debug.Log("Monster Die Task: " + "No parameters");
	}
	
	private void OnDestroy() 
	{
		//EventCenter.Instance.RemoveEventListener<Monster>("MonsterDie", MonsterDieTodo);
		EventCenter.Instance.RemoveEventListener(E_EventType.E_Monster_Dead, MonsterDieTodo);
	}
}
