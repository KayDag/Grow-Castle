using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress2 : MonoBehaviour
{
    public static Progress2 Instance;

    public GameObject enemy;
    public int spawnEnemy = 14;

    public GameObject enemy1;
    public int spawnEnemy1 = 4;

    public Transform pointA, pointB;

    private int enemySpawned = 0;
    private int enemy1Spawned = 0;
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartWave()
    {
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnWave());
    }
    //Time wave = 20s
    private IEnumerator SpawnWave()
    {
        float x = pointA.position.x;
        float yMin = pointA.position.y;
        float yMax = pointB.position.y;

        // lặp cho đến khi spawn hết enemy1 và enemy
        while (enemy1Spawned < spawnEnemy1 || enemySpawned < spawnEnemy)
        {
            // Spawn 1 enemy1 nếu chưa đủ
            if (enemy1Spawned < spawnEnemy1)
            {
                float y1 = Random.Range(yMin, yMax);
                GameObject e1 = Instantiate(enemy1, new Vector3(x, y1, 0f), Quaternion.identity);

                Enemy e1Comp = e1.GetComponent<Enemy>();
                if (e1Comp != null)
                    ManagerGame.Instance.aliveEnemies.Add(e1Comp);
                enemy1Spawned++;
            }

            yield return new WaitForSeconds(2f); // delay 1s sau enemy1

            // Spawn 3 enemy thường nếu còn
            int spawnCount = Mathf.Min(3, spawnEnemy - enemySpawned);
            for (int i = 0; i < spawnCount; i++)
            {
                float y = Random.Range(yMin, yMax);
                GameObject e = Instantiate(enemy, new Vector3(x, y, 0f), Quaternion.identity);

                Enemy eComp = e.GetComponent<Enemy>();
                if (eComp != null)
                    ManagerGame.Instance.aliveEnemies.Add(eComp);
                enemySpawned++;
            }

            yield return new WaitForSeconds(3f); // delay 1.5s sau 3 enemy
        }

        spawnCoroutine = null;
    }
}
