using UnityEngine;

public class Power : MonoBehaviour
{
    public float baseDamage = 2;
    public float baseSpeed = 5f;

    private float damage;
    private float speed;
    private Enemy target;

    void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
            FindTarget();

        Move();
    }

    void FindTarget()
    {
        float minDist = float.MaxValue;
        Enemy closest = null;

        foreach (var e in ManagerGame.Instance.aliveEnemies)
        {
            if (e == null || !e.gameObject.activeInHierarchy) continue;

            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = e;
            }
        }

        target = closest;
    }

    void Move()
    {
        if (target == null)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            return;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;
        transform.right = dir; // xoay đầu đạn theo hướng bay
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e != null && e == target)
        {
            e.TakeDamage((int)damage);
            Destroy(gameObject);
        }
    }

    public void ApplyStats(PlayerStatsManager stats)
    {
        damage = baseDamage * (1 + (stats.defender - 1) * 0.3f);
        speed = baseSpeed * (1 + (stats.defender - 1) * 0.25f);
    }
}
