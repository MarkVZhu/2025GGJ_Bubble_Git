using System.Collections;
using System.Collections.Generic;
using MarkFramework;
using TMPro;
using UnityEngine;

public class VisualTimer : MonoBehaviour
{
	public bool addTimer = true;
	
	[Tooltip("Unit: millisecond (Valid when addTimer = false)")]
	[ConditionalHide("addTimer", hideOnFalse: false)]
	public int targetTime; 
	
	int currentTime;
	int timerNum;
	public TextMeshProUGUI timerText;
	
	void Start()
	{
		if(addTimer)
		{
			int countTime = 9999 * 1000;
			targetTime = 0;
			//创建一个正向计时器
			//第四个参数<=10时为毫秒*10计时器，第四个参数为1000时为秒计时器
			timerNum = TimerMgr.Instance.CreateTimer(false, countTime, null, 0, UpdateAddTimer);
			currentTime = targetTime;
		}
		else
		{
			int countTime = targetTime;
			timerNum = TimerMgr.Instance.CreateTimer(false, countTime, MinusTimerEnd, 0, UpdateMinusTimer);
			currentTime = targetTime;
			timerText.text = FormatTime(currentTime);
		}
		
		TimerMgr.Instance.StopTimer(timerNum);
		TimerMgr.Instance.Start();
	}

	void UpdateAddTimer()
	{
		Debug.Log(currentTime);
		currentTime += 10; //每次加10毫秒（毫秒*10计时器）//每次加10*秒（秒计时器）
		timerText.text = FormatTime(currentTime);
	}
	
	void UpdateMinusTimer()
	{
		Debug.Log(currentTime);
		currentTime -= 10; //每次加10毫秒（毫秒*10计时器）//每次加10*秒（秒计时器）
		timerText.text = FormatTime(currentTime);
	}
	
	public void PauseTimer()
	{
		TimerMgr.Instance.StopTimer(timerNum);
	}
	
	public void StartTimer()
	{
		TimerMgr.Instance.StartTimer(timerNum);
	}
	
	public void ResetTimer()
	{
		currentTime = targetTime;
		timerText.text = FormatTime(currentTime);
		TimerMgr.Instance.ResetTimer(timerNum);
		TimerMgr.Instance.StopTimer(timerNum);
	}
	
	public void MinusTimerEnd()
	{
		Debug.Log("Time Up, 倒计时结束");
	}

	public static string FormatTime(int milliseconds)
	{
		// 转换为时间单位
		int totalSeconds = milliseconds / 1000;                // 总秒数
		int minutes = totalSeconds / 60;                       // 分钟
		int seconds = totalSeconds % 60;                       // 秒
		int millisecondsTens = (milliseconds % 1000) / 10;     // 毫秒的十分位

		// 格式化为 "00:00:00"
		return $"{minutes:00}:{seconds:00}:{millisecondsTens:00}";
	}
}
