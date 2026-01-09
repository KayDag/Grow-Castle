using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress4 : MonoBehaviour
{
    public static Progress4 Instance;

    public GameObject enemy;
    public int spawnEnemy = 35;

    public GameObject enemy1;
    public int spawnEnemy1 = 2;

    public Transform pointA, pointB;

    private int spawnIndex = 0;
    private int spawnIndex1 = 0;
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

    //Time wave = 56s
    private IEnumerator SpawnWave()
    {
        float x = pointA.position.x;
        float yMin = pointA.position.y;
        float yMax = pointB.position.y;

        int totalWaves = 2; // 7 đợt spawn
        for (int wave = 0; wave < totalWaves; wave++)
        {
            for (int i = 0; i < 11; i++)
            {
                if (spawnIndex >= spawnEnemy) break;

                float y = Random.Range(yMin, yMax);
                GameObject e = Instantiate(enemy, new Vector3(x, y, 0f), Quaternion.identity);

                Enemy eComp = e.GetComponent<Enemy>();
                if (eComp != null)
                    ManagerGame.Instance.aliveEnemies.Add(eComp);

                spawnIndex++;
            }

            yield return new WaitForSeconds(4f);

            // Spawn 1 enemy1 nếu còn
            if (spawnIndex1 < spawnEnemy1)
            {
                float y1 = Random.Range(yMin, yMax);
                GameObject e1 = Instantiate(enemy1, new Vector3(x, y1, 0f), Quaternion.identity);

                Enemy e1Comp = e1.GetComponent<Enemy>();
                if (e1Comp != null)
                    ManagerGame.Instance.aliveEnemies.Add(e1Comp);

                spawnIndex++;
            }

            yield return new WaitForSeconds(2f);
        }

        for (int i = 0; i < 13; i++)
        {
            if (spawnIndex < spawnEnemy)
            {
                float y = Random.Range(yMin, yMax);
                GameObject e = Instantiate(enemy, new Vector3(x, y, 0f), Quaternion.identity);

                Enemy eComp = e.GetComponent<Enemy>();
                if (eComp != null)
                {
                    ManagerGame.Instance.aliveEnemies.Add(eComp);
                }
                spawnIndex++;
            }
        }
        spawnCoroutine = null;
    }
    public bool IsDone()
    {
        return (spawnIndex >= spawnEnemy && spawnIndex1 >= spawnEnemy1 && ManagerGame.Instance.aliveEnemies.Count == 0);
    }
    public void ResetWave()
    {
        spawnIndex = 0;
        spawnIndex1 = 0;
        spawnCoroutine = null;
    }
}
