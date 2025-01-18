using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MarkFramework;
using UnityEngine;

public class TestJson : MonoBehaviour
{
	public class DataForSave
	{
		public string name;
		public bool sex;
		public int age;
		public int[] ids;
	}
	
	// Start is called before the first frame update
	void Start()
	{
		DataForSave dtSave = new DataForSave();
		dtSave.name = "Data";
		dtSave.sex = true;
		dtSave.age = 20;
		dtSave.ids = new int[] {1, 2, 3};
		
		JsonMgr.Instance.SaveData(dtSave, "JsonTestData");
		
		DataForSave dtLoad = JsonMgr.Instance.LoadData<DataForSave>("JsonTestData");
		Debug.Log("Save Location: C:\\Users\\<user>\\AppData\\LocalLow\\<company name>");
		Debug.Log($"Load Data - {dtLoad.name} : age {dtSave.age}");
	}
}
