using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // 배경이 이동하는 속도 (초당 단위)
    [SerializeField]
    private float moveSpeed = 1f;
 
    // 매 프레임마다 호출됨
    void Update()
    {
        if (!GameManager.Inst.IsGameBegin) return;
        // 배경을 아래로 이동
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;

        if (transform.position.y < -12f)
        {
            transform.position += new Vector3(0, 24f, 0);
        }

    }
}
