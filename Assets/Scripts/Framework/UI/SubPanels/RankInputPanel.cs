using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;
using TMPro;

public class RankInputPanel : BasePanel {

	protected override void Awake()
	{
		//一定不能少 因为需要执行父类的awake来初始化一些信息 比如找控件 加事件监听
		base.Awake();
		//在下面处理自己的一些初始化逻辑
	}

	// Use this for initialization
	void Start () {
		GetControl<TextMeshProUGUI>("ScoreText").text = "Score : " + GameManager.Instance.GetScore();
	}

	private void Drag(BaseEventData data)
	{
		//拖拽逻辑
	}

	private void PointerDown(BaseEventData data)
	{
		//PointerDown逻辑
	}

	// Update is called once per frame
	void Update () {
		
	}

	public override void ShowMe()
	{
		base.ShowMe();
		//显示面板时 想要执行的逻辑 这个函数 在UI管理器中 会自动帮我们调用
		//只要重写了它  就会执行里面的逻辑
	}

	protected override void OnClick(string btnName)
	{
		base.OnClick(btnName);
		
		switch(btnName)
		{
			case "btnSubmit":
				Debug.Log("btnSubmit被点击");
				string name = GetControl<TMP_InputField>("NameInputField").text;
				
				PlayerData currentPd = new PlayerData();
				currentPd.playerName = name;
				currentPd.rankNum = 0;
				currentPd.score = GameManager.Instance.GetScore();
				//Random.Range(1000,2000); //StaticDataCenter.Instance.currentScore;
				
				RankFunctions rf = new RankFunctions();
				List<PlayerData> playersData = rf.LoadPlayerData();
				rf.WritePlayerData(rf.InsertPlayerData(currentPd, playersData));
				
				UIManager.Instance.HidePanel("RankInputPanel");
				UIManager.Instance.ShowPanel<RankShowPanel>("RankShowPanel");
				break;
			case "btnCancel":
				Debug.Log("btnCancel被点击");
				UIManager.Instance.HidePanel("RankInputPanel");
				UIManager.Instance.ShowPanel<RankShowPanel>("RankShowPanel");
				break;
		}
	}

	protected override void OnValueChanged(string toggleName, bool value)
	{
		//在这来根据名字判断 到底是那一个单选框或者多选框状态变化了 当前状态就是传入的value
	}


	public void InitInfo()
	{
		Debug.Log("初始化数据");
	}

	//点击开始按钮的处理(可以放到switch里)
	public void ClickStart()
	{
	}

	//点击开始按钮的处理
	public void ClickQuit()
	{
		Debug.Log("Quit Game");
	}
}
