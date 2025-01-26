using UnityEngine;

public class UIFloatAnimation : MonoBehaviour
{
    public float amplitude = 20f;  
    public float frequency = 3f;   

    private RectTransform rectTransform;
    private Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();  
        startPos = rectTransform.anchoredPosition;     
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude; 
        rectTransform.anchoredPosition = new Vector2(startPos.x, newY);         
    }
}
