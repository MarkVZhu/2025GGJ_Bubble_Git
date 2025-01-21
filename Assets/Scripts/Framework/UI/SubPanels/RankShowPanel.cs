using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;
using TMPro;

public class RankShowPanel : BasePanel {

	public GameObject playerItemPrefab; // 列表项预制件
    public Transform contentTransform;  // Scroll View 的 Content 对象
	
	protected override void Awake()
	{
		//一定不能少 因为需要执行父类的awake来初始化一些信息 比如找控件 加事件监听
		base.Awake();
		//在下面处理自己的一些初始化逻辑
	}

	// Use this for initialization
	void Start () 
	{
		RankFunctions rf = new RankFunctions();
		List<PlayerData> playerDataList = rf.LoadPlayerData();
		PopulateScrollView(playerDataList);
	}
	
	private void PopulateScrollView(List<PlayerData> playerDataList)
	{
		foreach (var playerData in playerDataList)
		{
			// 实例化预制件
			GameObject newItem = Instantiate(playerItemPrefab, contentTransform);

			// 设置排名、名字和分数
			newItem.transform.Find("RankText").GetComponent<TextMeshProUGUI>().text = playerData.rankNum.ToString();
			newItem.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = playerData.playerName;
			newItem.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = playerData.score.ToString();
		}

		// 更新 Content 的高度
		//UpdateContentHeight(playerDataList.Count);
	}
	
	private void UpdateContentHeight(int listCount)
	{
		RectTransform contentRect = contentTransform.GetComponent<RectTransform>();
		float itemHeight = playerItemPrefab.GetComponent<RectTransform>().rect.height;
		contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, itemHeight * listCount);
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
		switch(btnName)
		{
			case "btnSubmit":
				
				break;
			case "btnCancel":
				
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
