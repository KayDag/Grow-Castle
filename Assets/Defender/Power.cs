using UnityEngine;

public class Power : MonoBehaviour
{
    public int dmg = 2;
    public float speedMove = 5f;

    Transform target;

    void Start()
    {
        SetTarget();
    }

    void Update()
    {
        Move();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(dmg);
            Destroy(gameObject);
        }
    }
    void SetTarget()
    {
        float minDist = float.MaxValue;
        Enemy closest = null;

        foreach (var e in Progess1.Instance.aliveEnemies)
        {
            if (e == null) continue;

            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = e;
            }
        }

        if (closest != null)
            target = closest.transform;
    }
    void Move()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speedMove * Time.deltaTime);
    }
}
