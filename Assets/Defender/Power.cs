using UnityEngine;

public class Power : MonoBehaviour
{
    public float baseDamage = 5;
    public float baseSpeed = 3f;
    public float deltaDamage = 1.7f;

    private float damage;
    private float speed;

    private Enemy target;
    private Vector3 moveDir;
    private bool hadTarget = false; 

    void Start()
    {
        LockTargetOrForward();
    }

    void Update()
    {
        // Đã từng có target
        if (hadTarget)
        {
            // Target chết giữa chừng 
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                Destroy(gameObject);
                return;
            }

            // Target còn sống
            moveDir = (target.transform.position - transform.position).normalized;
            transform.position += moveDir * speed * Time.deltaTime;
            transform.right = moveDir;
        }
        else
        {
            // Chưa từng có target 
            transform.position += moveDir * speed * Time.deltaTime;

            Vector3 view = Camera.main.WorldToViewportPoint(transform.position);
            if (view.x < -0.1f || view.x > 1.1f || view.y < -0.1f || view.y > 1.1f)
                Destroy(gameObject);
        }
    }

    void LockTargetOrForward()
    {
        float minDist = float.MaxValue;

        foreach (var e in ManagerGame.Instance.aliveEnemies)
        {
            if (e == null || !e.gameObject.activeInHierarchy) continue;

            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < minDist)
            {
                minDist = d;
                target = e;
            }
        }

        if (target != null)
        {
            hadTarget = true;
            moveDir = (target.transform.position - transform.position).normalized;
        }
        else
        {
            moveDir = Vector3.right; 
        }

        transform.right = moveDir;
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
