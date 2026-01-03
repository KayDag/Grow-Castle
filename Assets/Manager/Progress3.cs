using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress3 : MonoBehaviour
{
    public static Progress3 Instance;

    public GameObject enemy;
    public int spawnEnemy = 18;

    public GameObject enemy1;
    public int spawnEnemy1 = 9;

    public Transform pointA, pointB;

    private int enemySpawned = 0;
    private int enemy1Spawned = 0;
    private Coroutine spawnCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void StartWave()
    {
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnWave());
    }
    //Time wave = 36s
    private IEnumerator SpawnWave()
    {
        float x = pointA.position.x;
        float yMin = pointA.position.y;
        float yMax = pointB.position.y;

        int totalWaves = 9; // 9 đợt spawn
        for (int wave = 0; wave < totalWaves; wave++)
        {
            // Spawn 1 enemy1 nếu còn
            if (enemy1Spawned < spawnEnemy1)
            {
                float y1 = Random.Range(yMin, yMax);
                GameObject e1 = Instantiate(enemy1, new Vector3(x, y1, 0f), Quaternion.identity);

                Enemy e1Comp = e1.GetComponent<Enemy>();
                if (e1Comp != null)
                    ManagerGame.Instance.aliveEnemies.Add(e1Comp);

                enemy1Spawned++;
            }

            // Spawn 2 enemy thường nếu còn
            for (int i = 0; i < 2; i++)
            {
                if (enemySpawned >= spawnEnemy) break;

                float y = Random.Range(yMin, yMax);
                GameObject e = Instantiate(enemy, new Vector3(x, y, 0f), Quaternion.identity);

                Enemy eComp = e.GetComponent<Enemy>();
                if (eComp != null)
                    ManagerGame.Instance.aliveEnemies.Add(eComp);

                enemySpawned++;
            }

            // Delay giữa các đợt
            yield return new WaitForSeconds(4f);
        }

        spawnCoroutine = null;
    }
}
