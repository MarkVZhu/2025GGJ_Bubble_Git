using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    private void Start()
    {
        // 设置窗口分辨率为 720 x 1240
        Screen.SetResolution(720, 1240, false);
    }
}
