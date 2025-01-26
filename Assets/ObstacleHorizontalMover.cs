using UnityEngine;
using DG.Tweening; // 别忘了引用 DOTween 命名空间

/// <summary>
/// 如果检测到在同一水平范围内没有其他障碍物，则让此障碍物做X轴往返运动。
/// </summary>
[RequireComponent(typeof(Collider2D))] // 如果你的障碍物需要碰撞检测
public class ObstacleHorizontalMover : MonoBehaviour
{
    [Header("Detection Settings")] [Tooltip("检测附近障碍物的圆形半径")]
    public float checkWidth = 3f;

    public float checkHeight = 1f;

    [Tooltip("指定障碍物所在的LayerMask，用于检测")] public LayerMask obstacleMask;

    [Header("Movement Settings")] [Tooltip("左右移动的幅度（到右侧或左侧的距离）")]
    public float amplitude = 2f;

    [Tooltip("一次从左到右(或右到左)所需的时间")] public float duration = 2f;

    [Tooltip("运动曲线")] public Ease movementEase = Ease.InOutSine;

    private bool isMoving = false;

    void Start()
    {
        // 在Start时检测同一水平范围内有没有其它障碍物
        bool hasOtherObstacle = CheckForOtherObstacles();

        if (!hasOtherObstacle)
        {
            // 没有同类障碍物 => 启动左右摆动
            StartHorizontalMove();
        }
    }

    /// <summary>
    /// 检测同一水平范围内是否有其它障碍物
    /// </summary>
    /// <returns>若有其它障碍物则true，否则false</returns>
    private bool CheckForOtherObstacles()
    {
        // 以自身位置为圆心，用 checkRadius 做Overlap检测
        // obstacleMask 是LayerMask，一定要设对
        // Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, new Vector2(checkRadius, checkRadius), obstacleMask);
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            transform.position,
            new Vector2(checkWidth, checkHeight),
            0f, // 不旋转，可视需求设置
            obstacleMask
        );
        foreach (Collider2D col in hits)
        {
            // 如果找到的碰撞体不是自己，就说明范围内有别的障碍物
            if (col.gameObject != this.gameObject)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 启动X轴上的往返移动
    /// </summary>
    private void StartHorizontalMove()
    {
        if (isMoving) return;
        isMoving = true;

        float startX = transform.position.x;
        float targetX = startX + amplitude; // 往右移 amplitude

        // 用DOTween做一个无限循环往返
        // 先从startX到targetX，再从targetX返回startX
        transform.DOMoveX(targetX, duration)
            .SetEase(movementEase)
            .SetLoops(-1, LoopType.Yoyo);
    }

    // 如果需要在Scene里可视化检测范围（调试用）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // 与 OverlapBoxAll 相同的中心、大小、旋转
        Vector2 boxCenter = transform.position;
        Vector2 boxSize = new Vector2(checkWidth, checkHeight);
        // 画线时需要用 Matrix 处理旋转，这里角度=0就直接画
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}