using Assets.FantasyMonsters.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public static Castle Instance;
    public float health = 50;
    public float healthStay = 50;
    public Transform door;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(health);
    }
    public void CheckLose()
    {
        if (health <= 0)
        {
            Debug.Log("Lose game");
            Time.timeScale = 0;
        }
    }
}
