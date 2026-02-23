using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public int health = 10;
    public float speedMove = 3f;
    public float cooldown = 3f;
    public int goldDrop = 5;

    protected float timer;
    protected bool isAttackingCastle;
    protected bool isAttackingAttacker;

    protected Attacker targetAttacker;
    protected Animator animator;

    public GameObject vfxDie;
    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected void Update()
    {
        if (!ManagerGame.Instance.isGame) return;
        Move();
        HandleAttack();
    }

    protected void Move()
    {
        if (health <= 0) return;
        if (isAttackingCastle || isAttackingAttacker) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            Castle.Instance.door.position,
            speedMove * Time.deltaTime
        );
    }

    protected void HandleAttack()
    {
        if (!isAttackingCastle && !isAttackingAttacker)
        {
            timer = 0;
            return;
        }
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            Attack();
            timer = 0;
        }
    }

    protected virtual void Attack()
    {
        animator.SetBool(KeyAnimator.attacking, true);
        animator.SetTrigger(KeyAnimator.attack);

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
        if (other.CompareTag("Castle"))
        {
            isAttackingCastle = true;
            animator.SetBool(KeyAnimator.attacking, true);
        }
        else if (other.CompareTag("Attacker"))
        {
            isAttackingAttacker = true;
            targetAttacker = other.GetComponent<Attacker>();
            animator.SetBool(KeyAnimator.attacking, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Castle"))
        {
            isAttackingCastle = false;
            StopAttack();
        }
        else if (other.CompareTag("Attacker"))
        {
            StopAttackAttacker();
            StopAttack();
        }
    }
    public virtual void StopAttack()
    {
        if (!isAttackingCastle && !isAttackingAttacker)
        {
            animator.SetBool(KeyAnimator.attacking, false);
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
        AudioManager.Instance.PlayCollectGold();
        if (vfxDie != null)
        {
            Instantiate(vfxDie, transform.position, Quaternion.identity);
        }
        animator.SetBool(KeyAnimator.die, true);
        this.enabled = false; // TẮT Enemy script
        GetComponent<Collider2D>().enabled = false;

        if (Progess1.Instance != null)
            Progess1.Instance.RemoveEnemy(this);
        ManagerGame.Instance.stats.gold += goldDrop;
        Destroy(gameObject, 1f);
    }

    protected void PlayAnim(string anim)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
            animator.Play(anim);
    }
}
