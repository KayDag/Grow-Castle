using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public int health = 10;
    public float speedMove = 5f;
    public float cooldown = 1f;
    public float timer;
    public bool isAttacking = false;

    public Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Move();
    }

    private void Update()
    {
        if (isAttacking)
        {
            timer += Time.deltaTime;
            if (timer >= cooldown)
            {
                Attack();
                timer = 0;
            }
        }
    }
    void Move()
    {
        if (health <= 0) return;
        animator.Play(KeyAnimator.walk);
        float distance = Vector3.Distance(transform.position, Castle.Instance.door.position);
        float duration = distance / speedMove;
        transform.DOMove(Castle.Instance.door.position, duration).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Castle") && !isAttacking)
        {
            isAttacking = true;
            Attack();
            transform.DOKill(); 
        }
    }

    void Attack()
    {
        animator.Play(KeyAnimator.attack);
        DealDamage();
    }
    void DealDamage()
    {
        Castle castle = Castle.Instance;
        castle.TakeDamage(damage);
        castle.CheckLose();
    }
    public void TakeDamage(int amount)
    {
        if (health <= 0) return;
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Progess1.Instance.aliveEnemies.Remove(this);
        transform.DOKill();
        animator.Play(KeyAnimator.die);
        Destroy(gameObject, 0.5f);
    }
}
