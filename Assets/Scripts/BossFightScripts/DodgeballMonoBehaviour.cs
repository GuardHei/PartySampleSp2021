using UnityEngine;

public class DodgeballMonoBehaviour : MonoBehaviour
{
    public int damage = 1;
    public float movespeed = 10.0f;

    void Update()
    {
        Transform trns = transform;
        trns.position += Time.deltaTime * movespeed * trns.up;
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.CompareTag("Player"))
        {
            if (target.TryGetComponent(out Health health))
            {
                health.Hit(damage);
            }
            Destroy(gameObject);
        }
        else if (!target.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}