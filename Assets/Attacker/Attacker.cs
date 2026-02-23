using System.Collections;
using System.Collections.Generic;
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
    public GameObject vfxReachCheckPoint;

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

        if (isAttacking && enemy != null && enemy.health > 0)
        {
            AttackEnemy();
        }
        else
        {
            isAttacking = false;
            animator.SetBool(KeyAnimator.attacking, false);
            Move();
            timer = 0;
        }
    }

    void Move()
    {
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
            animator.SetTrigger(KeyAnimator.attack);
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
        animator.SetBool(KeyAnimator.die, true);
        this.enabled = false; // TẮT Enemy script
        GetComponent<Collider2D>().enabled = false;

        if (AttackerManager.Instance != null)
            AttackerManager.Instance.UnRegister(this);

        Destroy(gameObject, 1f);
    }

    void PlayAnim(string anim)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(anim))
            animator.Play(anim);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (reachedCheckpoint) return;
        if (other.CompareTag("Monster"))
        {
            isAttacking = true;
            enemy = other.GetComponent<Enemy>();
            animator.SetBool(KeyAnimator.attacking, true);
            timer = 0f;
        }
        else if (other.CompareTag("CheckPoint"))
        {
            ReachCheckpoint();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            isAttacking = false;
            enemy = null;
            animator.SetBool(KeyAnimator.attacking, false);
            timer = cooldown;
        }
    }
    void ReachCheckpoint()
    {
        if (reachedCheckpoint) return;

        AudioManager.Instance.PlayReachCheckPoint();
        if (vfxReachCheckPoint != null)
        {
            Instantiate(vfxReachCheckPoint, transform.position, Quaternion.identity);
        }

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
