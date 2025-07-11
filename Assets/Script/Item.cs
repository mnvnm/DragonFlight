using UnityEngine;

public class Item : MonoBehaviour
{
    public string ItemName = "";
    // Rigidbody2D 컴포넌트 참조
    Rigidbody2D rb;
    public int ItemValue = 0;
    // 시작 시 호출됨
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 가져오기
        Jump(); // 코인 튀기기
        if (ItemName == "Exp")
        {
            ItemValue = Random.Range(2, 5);
        }
    }

    void Update()
    {
        MagneticActive();
        if (transform.position.y < -7f)
        {
            Destroy(gameObject);
        }
    }

    void MagneticActive()
    {
        float distance = Vector2.Distance(GameManager.Inst.player.gameObject.transform.position, transform.position);
        if (distance <= 1.6f)
        {
            rb.gravityScale = 0.2f;
            rb.linearVelocity = new Vector2(0, 0);
            Vector2 vector2 = Vector2.Lerp(transform.position, GameManager.Inst.player.gameObject.transform.position, 6.5f * Time.deltaTime);
            transform.position = vector2;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    // 코인을 위로 튀기는 함수
    void Jump()
    {
        // x축은 -1~1, y축은 3~5의 임의의 힘을 가함 (ForceMode2D.Impulse)
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(3f, 6f)), ForceMode2D.Impulse);
    }
}
