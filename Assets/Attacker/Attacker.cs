using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public float damage = 5;
    public float baseDamage = 5;
    public float health = 140;
    public float baseHealth = 140;
    public float speed = 1.5f;
    public float baseSpeed = 1.5f;
    public float cooldown = 2f;

    private float timer;
    private Vector3 checkPoint;
    private bool isAttacking = false;
    private Enemy enemy;

    private bool reachedCheckpoint = false;
    private Animator animator;

    private int index = -1;

    void Awake()
    {
        animator = GetComponent<Animator>();
        ApplyStats(ManagerGame.Instance.stats);
    }

    void Start()
    {
        checkPoint = GetRandomCheckPoint();
    }

    void Update()
    {
        if (!ManagerGame.Instance.isGame) return;
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
            enemy.TakeDamage((int)damage);
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

        if (AttackerManager.Instance != null)
            AttackerManager.Instance.UnRegister(this);

        Destroy(gameObject, 0.5f);
    }

    void PlayAnim(string anim)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
            animator.Play(anim);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (reachedCheckpoint) return;
        timer = cooldown;
        if (other.CompareTag("Monster"))
        {
            isAttacking = true;
            enemy = other.GetComponent<Enemy>();
        }
        else if (other.CompareTag("CheckPoint"))
        {
            ReachCheckpoint();
        }
    }
    void ReachCheckpoint()
    {
        if (reachedCheckpoint) return;

        reachedCheckpoint = true;
        isAttacking = false;
        enemy = null;
        ManagerGame.Instance.OnAttackerReachCheckpoint(this);
        Die();
    }
    public void SetIndex(int i)
    {
        index = i;
    }
    public int GetIndex()
    {
        return index;
    }
    public void ResetState()
    {
        reachedCheckpoint = false;
        isAttacking = false;
        enemy = null;
        health = baseHealth; 
        checkPoint = GetRandomCheckPoint();
    }
    public void ApplyStats(PlayerStatsManager stats)
    {
        damage = baseDamage * (float)(1 + (stats.attacker - 1) * 0.15) ;
        speed = baseSpeed * (float)(1 + (stats.attacker - 1) * 0.2);
        health = baseHealth * (float)(1 + (stats.attacker - 1) * 0.35);
    }

}
