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
            Instantiate(ball, power.position, power.rotation);
    }
}
