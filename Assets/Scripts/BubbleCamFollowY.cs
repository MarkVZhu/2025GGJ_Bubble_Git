using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleCamFollowY : MonoBehaviour
{
    public Transform bubble;
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = bubble.position.y;
        transform.position = pos;
    }
}
