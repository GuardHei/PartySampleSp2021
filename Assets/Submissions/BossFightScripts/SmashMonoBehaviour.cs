using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashMonoBehaviour : MonoBehaviour
{
    
    public float countdownTimer = 1.0f;
    public int damage = 3;
    
    protected SpriteRenderer spriteRenderer;
    protected Collider2D collider;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownTimer < 0.0f)
        {
            Collider2D[] touching = new Collider2D[5];
            collider.OverlapCollider(new ContactFilter2D(), touching);
            foreach (Collider2D otherCollider in touching)
            {
                if (otherCollider != null)
                {
                    if (otherCollider.CompareTag("Player"))
                    {
                        otherCollider.GetComponent<Health>().Hit(damage);
                    }
                }
            }
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        spriteRenderer.color = spriteRenderer.color + new Color(30 * Time.deltaTime, 0, 0);
        countdownTimer -= Time.deltaTime;
    }
}
