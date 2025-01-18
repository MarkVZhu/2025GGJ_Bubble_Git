using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;

public class Monster : MonoBehaviour
{
	private void Start() 
	{
		Dead();	
	}
	
	// Start is called before the first frame update
	void Dead()
	{
		//EventCenter.Instance.EventTrigger("MonsterDie", this);
		EventCenter.Instance.EventTrigger(E_EventType.E_Monster_Dead);
		//EventCenter.Instance.EventTrigger<Monster>("MonsterDie", this); //same, both work
	}
}
