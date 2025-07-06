using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MarkFramework;
using Unity.VisualScripting;

public class SettingPanel : BasePanel 
{
    public Slider volumeSlider; // 在 Inspector 里赋值或者动态查找
    private BubbleMovement bubbleMovement;
    private float originalSen;

    protected override void Awake()
    {
        base.Awake();
        
        // 查找 Slider 组件（如果未在 Inspector 中赋值）
        if (volumeSlider == null)
        {
            volumeSlider = transform.Find("VolumeSlider")?.GetComponent<Slider>();
        }

        if (volumeSlider != null)
        {
            // 监听 Slider 值变化
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogWarning("VolumeSlider not found!");
        }
    }

    void Start() 
    {
        Invoke("TestM", 2f);
		bubbleMovement = GameObject.FindWithTag("Player").GetComponent<BubbleMovement>();
		originalSen = 35; //bubbleMovement.loudnessSensibility
		
		volumeSlider.value = bubbleMovement.loudnessSensibility/7;
    }

    private void Drag(BaseEventData data)
    {
        // 拖拽逻辑
    }

    private void PointerDown(BaseEventData data)
    {
        // PointerDown 逻辑
    }

    void Update() { }

    public override void ShowMe()
    {
        base.ShowMe();
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        
        switch(btnName)
        {
            case "btnBack":
                Debug.Log("btnBack被点击");
                UIManager.Instance.HidePanel("SettingPanel");
                UIManager.Instance.ShowPanel<MainPanel>("MainPanel");
                break;
        }
    }
    
    protected override void OnValueChanged(string toggleName, bool value) { }

    public void InitInfo()
    {
        Debug.Log("初始化数据");
    }

    public void ClickStart() { }

    public void ClickQuit()
    {
        Debug.Log("Quit Game");
    }

    // **处理 Slider 值变化的函数**
    private void OnSliderValueChanged(float value)
    {
        Debug.Log("Sensitivity Value: " + value);
        bubbleMovement.loudnessSensibility = value/5 * originalSen;
        CurrentSen.Instance.currentSen = bubbleMovement.loudnessSensibility;
    }
}
