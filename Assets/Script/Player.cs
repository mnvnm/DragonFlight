using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 플레이어 이동 속도
    [SerializeField] float moveSpeed = 1f;
    // 미사일 프리팹 리스트
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject bombPrefab;
    // 미사일 생성 위치
    [SerializeField] Transform missileSpawnPosition;
    private float shootInverval = 0.5f;
    private float bombInverval = 3f;

    // 마지막 발사 시간
    private float lastshotTime = 0f;
    private float bombLastshotTime = 0f;
    [HideInInspector] public int missileCount = 1;// 플레이어가 쏠 총알의 개수

    // 애니메이터 컴포넌트 참조
    private Animator animator;

    private int CurHP = 100;
    private int MaxHP = 100;
    private int bombCount = 3;
    private int maxBombCount = 3;

    private bool isInvincibility = false;

    [SerializeField] GameObject turretPrefab;
    int TurretMaxCount = 5;
    float circleR = 1.2f; // 돌 원의 반지름
    float deg; // 돌아가는 각도
    float TurretSpeed = 72f; // 오브젝트 스피드
                             // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<Turret> turrets = new List<Turret>();

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }
    public void Init()
    {
        foreach (var turret in turrets) Destroy(turret.gameObject);
        turrets.Clear();
        missileCount = 1;
        lastshotTime = Time.time;
        bombLastshotTime = Time.time; // 마지막 발사 시간 갱신
        CurHP = MaxHP;
        bombCount = maxBombCount;
        isInvincibility = false;
        transform.position = new Vector3(0, -3.5f, 0);
    }

    // 매 프레임마다 이동 및 발사 처리
    void Update()
    {
        if (!GameManager.Inst.IsGameBegin) return;
        Move();
        Shoot(); // 미사일 발사
        if (turrets.Count > 0) TurretRotate();
    }

    void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 moveTo = new Vector3(horizontalInput, 0, 0);
        transform.position += moveTo * moveSpeed * Time.deltaTime; // 좌우 이동

        // 애니메이션 상태 변경
        if (horizontalInput < 0)
        {
            animator.Play("Left"); // 왼쪽 이동 애니메이션
        }
        else if (horizontalInput > 0)
        {
            animator.Play("Right"); // 오른쪽 이동 애니메이션
        }
        else
        {
            animator.Play("Idle"); // 가운데(정지) 애니메이션
        }
    }

    // 미사일 발사 함수
    void Shoot()
    {
        if (Time.time - lastshotTime > shootInverval - ((float)LevelController.Inst.GetLevel() * 0.025) && Input.GetKey(KeyCode.Space))
        {
            float angleRange = missileCount > 2 ? 45f : 30; // 전체 퍼지는 각도 (예: 45도)
            float startAngle = -angleRange / 2f;
            float angleStep = missileCount > 1 ? angleRange / (missileCount - 1) : 0;

            for (int i = 0; i < missileCount; i++)
            {
                float angle = missileCount > 1 ? startAngle + angleStep * i : 0;
                Quaternion rot = Quaternion.Euler(0, 0, angle);
                Instantiate(missilePrefab, missileSpawnPosition.position, rot);
            }
            lastshotTime = Time.time; // 마지막 발사 시간 갱신
        }

        if (Time.time - bombLastshotTime > bombInverval && Input.GetKeyDown(KeyCode.G) && bombCount > 0)
        {
            bombCount--;
            Instantiate(bombPrefab, new Vector2(0, 0), Quaternion.identity);
            bombLastshotTime = Time.time; // 마지막 발사 시간 갱신
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            var item = collision.GetComponent<Item>();
            switch (item.ItemName)
            {
                case "Exp":
                    LevelController.Inst.AddExp(item.ItemValue);
                    break;
                case "Coin":
                    GameManager.Inst.Coin += item.ItemValue;
                    break;
                case "BombUp":
                    if (bombCount < maxBombCount) bombCount++;
                    break;
                case "TurretUp":
                    if (turrets.Count < TurretMaxCount)
                    {
                        var turretObj = Instantiate(turretPrefab, this.transform.position, Quaternion.identity, transform);
                        var turret = turretObj.GetComponent<Turret>();
                        turrets.Add(turret);
                    }
                    break;
                case "BulletUpgrade":
                    if (missileCount < 5) missileCount++;
                    break;
            }
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Enemy"))
        {
            DamagePlayer(25);
        }
    }
    public void DamagePlayer(int damage)
    {
        if (CurHP > 0 && !isInvincibility)
        {
            CurHP -= damage;
            StartCoroutine(Invincibility());
        }

        if (CurHP <= 0) GameManager.Inst.EndGame();
    }

    public float GetPlayerCurHP()
    {
        return CurHP;
    }
    public float GetPlayerMaxHP()
    {
        return MaxHP;
    }
    public float GetPlayerBombCount()
    {
        return bombCount;
    }

    IEnumerator Invincibility()
    {
        var spr = GetComponent<SpriteRenderer>();
        isInvincibility = true;
        spr.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        spr.color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.5f);
        spr.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        spr.color = new Color(1, 1, 1, 1f);
        yield return new WaitForSeconds(0.5f);
        isInvincibility = false;
        yield return null;
    }
    void TurretRotate()
    {
        deg += Time.deltaTime * TurretSpeed;
        if (deg < 360)
        {
            for (int i = 0; i < turrets.Count; i++)
            {
                var rad = Mathf.Deg2Rad * (deg + (i * (360 / turrets.Count)));
                var x = circleR * Mathf.Sin(rad);
                var y = circleR * Mathf.Cos(rad);
                turrets[i].gameObject.transform.position = transform.position + new Vector3(x, y);
                turrets[i].gameObject.transform.rotation = Quaternion.Euler(0, 0, (deg + (i * (360 / turrets.Count))) * -1);
            }
        }
        else
        {
            deg = 0;
        }
    }

    public float GetBombCurCooltime()
    {
        return Time.time - bombLastshotTime;
    }
    public float GetBombMaxCooltime()
    {
        return bombInverval;
    }
}
