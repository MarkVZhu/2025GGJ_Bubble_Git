using MarkFramework;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
	// 光标图片引用
	public Texture2D defaultCursor;
	public Texture2D inGameCursor;   // 游戏内光标
	public Texture2D openMicCursor;    // 开麦光标
	public Texture2D inflateCursor;    // 吹大光标

	// 光标的热点（光标的实际点击点）
	public Vector2 hotspot = Vector2.zero;

	// 光标模式
	public CursorMode cursorMode = CursorMode.Auto;
	
	void Start()
	{
		EventCenter.Instance.AddEventListener(E_EventType.E_InGame_Cursor, SetInGameCursor);
		EventCenter.Instance.AddEventListener(E_EventType.E_Open_Mic, SetOpenMicCursor);
		EventCenter.Instance.AddEventListener(E_EventType.E_On_Bubble, SetInflateCursor);
		EventCenter.Instance.AddEventListener(E_EventType.E_Default_Cursor, SetDefaultCursor);
		// 设置默认光标
		SetCursor(defaultCursor);
	}
	
	void SetInGameCursor()
	{
		SetCursor(inGameCursor);
	}
	
	void SetOpenMicCursor()
	{
		SetCursor(openMicCursor);
	}
	
	void SetInflateCursor()
	{
		SetCursor(inflateCursor);
	}
	
	void SetDefaultCursor()
	{
		SetCursor(defaultCursor);
	}


	void OnDestroy() 
	{
		EventCenter.Instance.RemoveEventListener(E_EventType.E_InGame_Cursor, SetInGameCursor);
		EventCenter.Instance.RemoveEventListener(E_EventType.E_Open_Mic, SetOpenMicCursor);
		EventCenter.Instance.RemoveEventListener(E_EventType.E_On_Bubble, SetInflateCursor);
		EventCenter.Instance.RemoveEventListener(E_EventType.E_Default_Cursor, SetDefaultCursor);
	}
	
	// 设置光标的方法
	private void SetCursor(Texture2D cursorTexture)
	{
		Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
	}
}
