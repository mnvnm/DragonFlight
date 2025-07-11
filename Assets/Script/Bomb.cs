using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    SpriteRenderer spr;
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Inst.IsGameBegin) return;
        Explode();
    }

    void Explode()
    {
        transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 11, 5 * Time.deltaTime), Mathf.Lerp(transform.localScale.y, 11, 5 * Time.deltaTime), 0);
        if (transform.localScale.x > 10.99f || transform.localScale.y > 10.99f) transform.localScale = new Vector3(11, 11, 0);

        spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, Mathf.Lerp(spr.color.a, 0, 4 * Time.deltaTime));

        if (spr.color.a <= 0) Destroy(gameObject);
    }
}
