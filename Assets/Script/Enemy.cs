using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 데미지 텍스트 프리팹
    public GameObject damageTextPrefab;

    // 스프라이트 렌더러 및 색상 관련 변수
    private SpriteRenderer spriteRenderer;
    public Color flashColor = Color.red; // 피격 시 깜빡일 색상
    public float flashDuration = 0.1f;   // 깜빡임 지속 시간
    private Color originalColor;         // 원래 색상 저장

    // 적의 체력
    public float enemyHp = 1;

    [SerializeField]
    public float moveSpeed = 1f; // 이동 속도

    // 코인 및 이펙트 프리팹
    public GameObject CoinPrefab;
    public GameObject ExpPrefab;
    public GameObject TurretUpPrefab;
    public GameObject BombUpPrefab;
    public GameObject BulletUpgradePrefab;
    public GameObject Effect;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 참조
        originalColor = spriteRenderer.color; // 원래 색상 저장
    }

    // 적이 피격 시 깜빡임 효과
    // public void Flash()
    // {
    //     StopAllCoroutines(); // 기존 코루틴 중지
    //     StartCoroutine(FlashRoutine());
    // }

    // // 피격 시 색상 변경 코루틴
    // private IEnumerator FlashRoutine()
    // {
    //     spriteRenderer.color = flashColor;
    //     yield return new WaitForSeconds(flashDuration);
    //     spriteRenderer.color = originalColor;
    // }

    // 이동 속도 설정
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    // 매 프레임마다 아래로 이동, 화면 밖으로 나가면 삭제
    void Update()
    {
        if (!GameManager.Inst.IsGameBegin) return;
        Cheat();
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        if (transform.position.y < -7f)
        {
            GameManager.Inst.enemys.RemoveEnemy(this);
        }
    }

    void SpawnItem()
    {
        int randItemSpawnAverrage = Random.Range(0, 1000);
        if (randItemSpawnAverrage > 800) Instantiate(CoinPrefab, transform.position, Quaternion.identity); // 코인 생성
        else if (randItemSpawnAverrage > 0 && randItemSpawnAverrage <= 15) Instantiate(BombUpPrefab, transform.position, Quaternion.identity);
        else if (randItemSpawnAverrage > 15 && randItemSpawnAverrage <= 30) Instantiate(TurretUpPrefab, transform.position, Quaternion.identity);
        else if (randItemSpawnAverrage > 30 && randItemSpawnAverrage <= 45) Instantiate(BulletUpgradePrefab, transform.position, Quaternion.identity);
    }
    void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Instantiate(CoinPrefab, transform.position, Quaternion.identity);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Instantiate(TurretUpPrefab, transform.position, Quaternion.identity);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Instantiate(BulletUpgradePrefab, transform.position, Quaternion.identity);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Instantiate(BombUpPrefab, transform.position, Quaternion.identity);
    }

    // 미사일과 충돌 시 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Missile")
        {
            Missile missile = collision.GetComponent<Missile>();
            StopAllCoroutines(); // 기존 코루틴 중지
            StartCoroutine("HitColor"); // 피격 색상 코루틴 실행
            // Flash(); // 대체 가능

            enemyHp = enemyHp - missile.missileDamege; // 체력 감소
            TakeDamage(missile.missileDamege); // 데미지 팝업 표시
        }
        if (collision.tag == "Bomb")
        {
            StopAllCoroutines(); // 기존 코루틴 중지
            Destroy(Instantiate(Effect, transform.position, Quaternion.identity), 1f); // 이펙트 생성 후 1초 뒤 바로 삭제
            TakeDamage(100);
        }
    }

    // 피격 시 색상 변경 코루틴
    IEnumerator HitColor()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = Color.white;
    }

    // 데미지 팝업 표시 함수
    void TakeDamage(int damage)
    {
        if (enemyHp > 0) enemyHp -= damage;
        if (enemyHp <= 0)
        {
            Instantiate(ExpPrefab, transform.position, Quaternion.identity); // 경험치 생성
            SpawnItem();
            Destroy(Instantiate(Effect, transform.position, Quaternion.identity), 1f); // 이펙트 생성 후 1초 뒤 바로 삭제
            GameManager.Inst.enemys.RemoveEnemy(this);
        }
    }
}
