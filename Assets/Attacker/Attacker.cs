using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public int damage = 10;
    public int health = 20;
    public float speed = 2f;
    public float cooldown = 1f;

    private float timer;
    private Vector3 checkPoint;
    private bool isAttacking = false;
    private Enemy enemy;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        checkPoint = GetRandomCheckPoint();
    }

    void Update()
    {
        if (health <= 0) return;

        if (isAttacking && enemy != null)
        {
            AttackEnemy();
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        PlayAnim(KeyAnimator.walk);

        transform.position = Vector3.MoveTowards(
            transform.position,
            checkPoint,
            speed * Time.deltaTime
        );
    }

    void AttackEnemy()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            PlayAnim(KeyAnimator.attack);
            enemy.TakeDamage(damage);
            timer = 0f;
        }
    }

    Vector3 GetRandomCheckPoint()
    {
        Camera cam = Camera.main;
        float rightX = cam.ViewportToWorldPoint(
            new Vector3(1f, 0.5f, cam.nearClipPlane)
        ).x;

        float randomY = Random.Range(-2.0f, 1f);
        return new Vector3(rightX, randomY, 0f);
    }

    public void TakeDamage(int amount, Enemy enemy)
    {
        if (health <= 0) return;

        health -= amount;
        if (health <= 0)
        {
            Die();
            enemy.StopAttackAttacker();
        }
    }

    void Die()
    {
        PlayAnim(KeyAnimator.die);
        Destroy(gameObject, 0.5f);
    }

    void PlayAnim(string anim)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
            animator.Play(anim);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        timer = cooldown;
        if (other.CompareTag("Monster"))
        {
            isAttacking = true;
            enemy = other.GetComponent<Enemy>();
        }
        else if (other.CompareTag("CheckPoint"))
        {
            Destroy(gameObject);
        }
    }
}
