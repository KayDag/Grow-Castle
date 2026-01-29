using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemyOnlyCastle : Enemy
{
    protected override void Attack()
    {
        if (isAttackingCastle)
        {
            animator.SetBool(KeyAnimator.attacking, true);
            animator.SetTrigger(KeyAnimator.attack);

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
    }

    public override void StopAttack()
    {
        if (!isAttackingCastle)
        {
            animator.SetBool(KeyAnimator.attacking, false);
        }
    }
}
