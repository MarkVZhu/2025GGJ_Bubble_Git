using UnityEngine;

public class MoveImageUp : MonoBehaviour
{
    public float speed = 5f;  // 控制移动速度

    void Update()
    {
        // 简单地增加y坐标
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}