using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Defender : MonoBehaviour
{
    public GameObject ball;
    public Transform power;

    public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Fire()
    {
            animator.Play(KeyAnimator.attack);
        GameObject b = Instantiate(ball, power.position, power.rotation);

        Power p = b.GetComponent<Power>();
        if (p != null)
        {
            if (DefenderManager.Instance.isUseBooster)
            {
                p.ApplyStatsBooster(ManagerGame.Instance.stats);
            }
            else
            {
                p.ApplyStats(ManagerGame.Instance.stats);
            }
        }
        DefenderManager.Instance.ball.Add(b);
            
    }
}
