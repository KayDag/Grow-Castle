using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progess1 : MonoBehaviour
{
    public static Progess1 Instance;
    public List<GameObject> enemies;
    public List<Enemy> aliveEnemies = new List<Enemy>();
    public Transform pointA, pointB;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnEnemies()
    {
        float distance = Mathf.Abs(pointA.position.y - pointB.position.y) / enemies.Count;
        float x = pointA.position.x;
        float y = pointB.position.y;
        float delta = 0;
        foreach (var e in enemies)
        {
            GameObject enemy = Instantiate(e, new Vector3(x, y + delta, 0f), Quaternion.identity);
            aliveEnemies.Add(enemy.GetComponent<Enemy>());
            delta += distance;
        }
    }
    public bool Done()
    {
        if (aliveEnemies == null || aliveEnemies.Count == 0)
        {
            return true;
        }
        return false;
    }

}
