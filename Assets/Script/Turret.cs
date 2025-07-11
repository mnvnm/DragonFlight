using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject missilePrefab;
    // 미사일 생성 위치
    [SerializeField] Transform missileSpawnPosition;

    // 미사일 발사 간격(초)
    [SerializeField]
    private float shootInverval = 1f;

    // 마지막 발사 시간
    private float lastshotTime = 0f;
    public void Init()
    {

    }
    void Update()
    {
        if (!GameManager.Inst.IsGameBegin) return;
        Shoot();
    }

    // 미사일 발사 함수
    void Shoot()
    {
        if (Time.time - lastshotTime > shootInverval)
        {
            Instantiate(missilePrefab, missileSpawnPosition.position, Quaternion.identity);
            lastshotTime = Time.time; // 마지막 발사 시간 갱신
        }
    }
}
