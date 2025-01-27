using UnityEngine;

public class CameraAspectController : MonoBehaviour
{
    public float targetAspect = 1080f / 1920f; // 目标宽高比

    void Start()
    {
        // 当前窗口的宽高比
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // 计算宽高比差异
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = Camera.main;

        if (scaleHeight < 1.0f) // 如果窗口更高，增加黑边
        {
            Rect rect = camera.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            camera.rect = rect;
        }
        else // 如果窗口更宽，增加黑边
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = camera.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            camera.rect = rect;
        }
    }
}
