using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Power : MonoBehaviour
{
    public int dmg = 2;
    public float speedMove = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.health -= dmg;
            Destroy(gameObject);
        }
    }
    public Transform target;

    public void SetTarget(Transform t)
    {
        target = t;
        Move(); 
    }

    void Move()
    {
        if (target == null) return;
        float distance = Vector3.Distance(transform.position, target.position);
        float duration = distance / speedMove;
        transform.DOMove(target.position, duration).SetEase(Ease.Linear).SetTarget(target);
    }

}
