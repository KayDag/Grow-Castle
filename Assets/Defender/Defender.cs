using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    public GameObject ball;
    public Transform power;
    [SerializeField] float timer = 0;
    [SerializeField] int cooldown = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            Fire();
            timer = 0f;
        }
    }
    void Fire()
    {
        Instantiate(ball, power.position, power.rotation);
    }
}
