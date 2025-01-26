using MarkFramework;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
	// 光标对象引用
	public RectTransform cursorImage; // UI Image 或 RectTransform（UI 光标）
	public Sprite defaultCursor;
	public Sprite inGameCursor;    // 游戏内光标
	public Sprite openMicCursor;   // 开麦光标
	public Sprite inflateCursor;   // 吹大光标

	private Image cursorRenderer;  // 显示光标的 Image 组件
	
	public Transform bubble;
private bool isPointingToBubble = false;

	void Start()
	{
		// 隐藏系统默认光标
		Cursor.visible = false;

		// 获取光标渲染器（Image）
		cursorRenderer = cursorImage.GetComponent<Image>();

		// 事件监听
		EventCenter.Instance.AddEventListener(E_EventType.E_InGame_Cursor, SetInGameCursor);
		EventCenter.Instance.AddEventListener(E_EventType.E_Open_Mic, SetOpenMicCursor);
		EventCenter.Instance.AddEventListener(E_EventType.E_On_Bubble, SetInflateCursor);
		EventCenter.Instance.AddEventListener(E_EventType.E_Default_Cursor, SetDefaultCursor);

		// 设置默认光标
		SetCursor(defaultCursor);
	}

	void Update()
	{
		// 实时更新光标位置
		Vector2 mousePosition = Input.mousePosition;
		cursorImage.position = mousePosition;
		
		// 如果需要指向 bubble
		if (isPointingToBubble && bubble != null)
		{
			Vector3 directionToBubble = (bubble.position - Camera.main.ScreenToWorldPoint(mousePosition));
			directionToBubble.z = 0; // 忽略 Z 轴

			float angle = Mathf.Atan2(directionToBubble.y, directionToBubble.x) * Mathf.Rad2Deg - 90f;
			cursorImage.rotation = Quaternion.Euler(0, 0, angle);
		}
	}

	void SetInGameCursor()
	{
		SetCursor(inGameCursor);
		isPointingToBubble = true;
	}

	void SetOpenMicCursor()
	{
		SetCursor(openMicCursor);
		isPointingToBubble = true; // 启用指向功能
	}

	void SetInflateCursor()
	{
		SetCursor(inflateCursor);
		isPointingToBubble = false;
		cursorImage.rotation = Quaternion.Euler(0, 0, 0);
	}

	void SetDefaultCursor()
	{
		SetCursor(defaultCursor);
		isPointingToBubble = false;
		cursorImage.rotation = Quaternion.Euler(0, 0, 0);
	}

	void OnDestroy()
	{
		// 移除事件监听
		EventCenter.Instance.RemoveEventListener(E_EventType.E_InGame_Cursor, SetInGameCursor);
		EventCenter.Instance.RemoveEventListener(E_EventType.E_Open_Mic, SetOpenMicCursor);
		EventCenter.Instance.RemoveEventListener(E_EventType.E_On_Bubble, SetInflateCursor);
		EventCenter.Instance.RemoveEventListener(E_EventType.E_Default_Cursor, SetDefaultCursor);
	}

	// 设置光标的方法
	private void SetCursor(Sprite cursorSprite)
	{
		if (cursorSprite != null && cursorRenderer != null)
		{
			cursorRenderer.sprite = cursorSprite;
		}
	}
}
