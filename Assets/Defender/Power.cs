using UnityEngine;

public class Power : MonoBehaviour
{
    public float baseDamage = 5;
    public float baseSpeed = 3f;
    public float deltaDamage = 1.7f;

    private float damage;
    private float speed;

    private Vector3 moveDir;
    private bool isLocked = false;

    void Start()
    {
        LockTargetDirection();
    }

    void Update()
    {
        transform.position += moveDir * speed * Time.deltaTime;

        // tự hủy khi ra khỏi màn hình
        Vector3 view = Camera.main.WorldToViewportPoint(transform.position);
        if (view.x < -0.1f || view.x > 1.1f || view.y < -0.1f || view.y > 1.1f)
            Destroy(gameObject);
    }

    void LockTargetDirection()
    {
        Enemy closest = null;
        float minDist = float.MaxValue;

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

        if (closest != null)
            moveDir = (closest.transform.position - transform.position).normalized;
        else
            moveDir = Vector3.right; // không có mục tiêu thì bắn thẳng

        transform.right = moveDir;
        isLocked = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage((int)damage);
            Destroy(gameObject);
        }
    }

    public void ApplyStats(PlayerStatsManager stats)
    {
        damage = baseDamage * (1 + (stats.defender - 1) * 0.3f);
        speed = baseSpeed * (1 + (stats.defender - 1) * 0.1f);
    }

    public void ApplyStatsBooster(PlayerStatsManager stats)
    {
        damage = baseDamage * (1 + (stats.defender - 1) * 0.3f)
                 + deltaDamage + 0.3f * (stats.defender - 1);
        speed = baseSpeed * (1 + (stats.defender - 1) * 0.1f);
    }
}
