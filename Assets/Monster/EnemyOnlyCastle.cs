using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnlyCastle : Enemy
{
    protected override void Attack()
    {
        if (isAttackingCastle)
        {
            PlayAnim(KeyAnimator.attack);
            Castle.Instance.TakeDamage(damage);
            Castle.Instance.CheckLose();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        timer = cooldown;

        if (other.CompareTag("Castle"))
        {
            isAttackingCastle = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Castle"))
        {
            isAttackingCastle = false;
        }
    }
}
