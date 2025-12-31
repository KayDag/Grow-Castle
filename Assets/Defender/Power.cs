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
        Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Mathf.Abs(Camera.main.transform.position.z)));
        if (target == null)
        {
            transform.position = Vector3.MoveTowards(transform.position,center,speedMove * Time.deltaTime);
            if (Vector3.Distance(transform.position, center) < 0.1f)
            {
                Destroy(gameObject);
            }
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speedMove * Time.deltaTime);
    }
}
