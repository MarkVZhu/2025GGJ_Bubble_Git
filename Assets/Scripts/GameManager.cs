using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarkFramework;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : SingletonMono<GameManager>
{
	public enum GameState
	{
		Start,
		Playing,
		Paused,
		Ended
	}

	// 当前游戏状态
	[SerializeField] private GameState currentState;
	public GameState CurrentState => currentState;
	public GameObject inGameCanvas;

	// 是否游戏正在运行
	public bool IsGameRunning => currentState == GameState.Playing;
	
	private int gameScore;
	
	private void Start()
	{	
		// 默认状态设置为游戏开始
		ChangeState(GameState.Start);
		SoundMgr.Instance.PlayBKMusic("BGM_BUB");
		UIManager.Instance.ShowPanel<MainPanel>("MainPanel");
	}

	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void ChangeState(GameState newState)
	{
		currentState = newState;
		HandleGameStateChange(newState);
	}
	
	/// <summary>
	/// 根据游戏状态处理逻辑
	/// </summary>
	/// <param name="state">当前状态</param>
	private void HandleGameStateChange(GameState state)
	{
		switch (state)
		{
			case GameState.Start:
				Debug.Log("Game Started");
				// 可以在这里初始化游戏
				break;
			case GameState.Playing:
				Debug.Log("Game is Playing");
				// 恢复游戏逻辑
				Time.timeScale = 1f;
				break;
			case GameState.Paused:
				Debug.Log("Game is Paused");
				// 暂停游戏逻辑
				Time.timeScale = 0f;
				break;
			case GameState.Ended:
				Debug.Log("Game Ended");
				// 结束游戏逻辑
				Time.timeScale = 0f;
				break;
		}
	}
	
	 /// <summary>
	/// 开始游戏
	/// </summary>
	public void StartGame()
	{
		ChangeState(GameState.Playing);
		if(inGameCanvas) inGameCanvas.SetActive(true);
		//EventCenter.Instance.EventTrigger(E_EventType.E_InGame_Cursor);
	}

	/// <summary>
	/// 暂停游戏
	/// </summary>
	public void PauseGame()
	{
		if (currentState == GameState.Playing)
		{
			ChangeState(GameState.Paused);
			UIManager.Instance.ShowPanel<PausePanel>("PausePanel");
			EventCenter.Instance.EventTrigger(E_EventType.E_Default_Cursor);
			SoundMgr.Instance.PauseBKMusic();
		}
	}

	/// <summary>
	/// 恢复游戏
	/// </summary>
	public void ResumeGame()
	{
		if (currentState == GameState.Paused)
		{
			ChangeState(GameState.Playing);
			EventCenter.Instance.EventTrigger(E_EventType.E_InGame_Cursor);
			SoundMgr.Instance.PlayBKMusic("PausePanel");
		}
	}
	
	public void ReplayGame()
	{
		ScenesMgr.Instance.ReloadCurrentScene();
	}

	/// <summary>
	/// 结束游戏
	/// </summary>
	public void EndGame()
	{
		SoundMgr.Instance.PlaySound("Explode");
		ChangeState(GameState.Ended);
		if(inGameCanvas) inGameCanvas.SetActive(false);
		
		ProgressTracker pt = GetComponent<ProgressTracker>();
		if(pt != null)
		{
			gameScore = pt.score;
		}
		else
		{
			Debug.LogError("Not found ProgressTracker!");
		}
		
		SoundMgr.Instance.StopBKMusic();
		UIManager.Instance.ShowPanel<ResultPanel>("ResultPanel"); //TODO:调出Rank榜单
	}
	
	public int GetScore()
	{
		return gameScore;
	}
}
