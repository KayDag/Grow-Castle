using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public int health = 10;
    public float speedMove = 3f;
    public float cooldown = 3f;

    protected float timer = 0;
    protected bool isAttackingCastle;
    protected bool isAttackingAttacker;

    protected Attacker targetAttacker;
    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        Move();
        HandleAttack();
    }

    protected void Move()
    {
        if (health <= 0) return;
        if (isAttackingCastle || isAttackingAttacker) return;

        PlayAnim(KeyAnimator.walk);

        transform.position = Vector3.MoveTowards(
            transform.position,
            Castle.Instance.door.position,
            speedMove * Time.deltaTime
        );
    }

    protected void HandleAttack()
    {
        if (!isAttackingCastle && !isAttackingAttacker) return;

        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            Attack();
            timer = 0f;
        }
    }

    protected virtual void Attack()
    {
        PlayAnim(KeyAnimator.attack);

        if (isAttackingCastle)
        {
            Castle.Instance.TakeDamage(damage);
            Castle.Instance.CheckLose();
        }
        else if (isAttackingAttacker && targetAttacker != null)
        {
            targetAttacker.TakeDamage(damage, this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        timer = cooldown;
        if (other.CompareTag("Castle"))
        {
            isAttackingCastle = true;
        }
        else if (other.CompareTag("Attacker"))
        {
            isAttackingAttacker = true;
            targetAttacker = other.GetComponent<Attacker>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Castle"))
        {
            isAttackingCastle = false;
        }
        else if (other.CompareTag("Attacker"))
        {
            StopAttackAttacker();
        }
    }

    public void StopAttackAttacker()
    {
        isAttackingAttacker = false;
        targetAttacker = null;
    }

    public void TakeDamage(int amount)
    {
        if (health <= 0) return;

        health -= amount;
        if (health <= 0)
            Die();
    }

    protected void Die()
    {
        PlayAnim(KeyAnimator.die);
        Destroy(gameObject, 0.5f);
    }

    protected void PlayAnim(string anim)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
            animator.Play(anim);
    }
}
