using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress7 : MonoBehaviour, IProgress
{
    public static Progress7 Instance;

    public GameObject enemy;
    private int spawnEnemy = 45;

    public GameObject enemy1;
    private int spawnEnemy1 = 16;

    public GameObject enemy2;
    private int spawnEnemy2 = 10;

    public GameObject boss;
    private int spawnBoss = 2;

    public Transform pointA, pointB;

    private int spawnIndex = 0;
    private int spawnIndex1 = 0;
    private int spawnIndex2 = 0;
    private int spawnIndexBoss = 0;
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
        if (!ManagerGame.Instance.isGame) return;
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnWave());
    }

    //Time wave = 56s
    private IEnumerator SpawnWave()
    {
        float x = pointA.position.x;
        float yMin = pointA.position.y;
        float yMax = pointB.position.y;

        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 5 && spawnIndex < spawnEnemy; j++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            if (i % 3 == 0 && spawnIndex1 < spawnEnemy1)
            {
                Spawn(enemy1, x, yMin, yMax);
                spawnIndex1++;
            }

            yield return new WaitForSeconds(1.5f);
        }

        if (spawnIndexBoss < spawnBoss)
        {
            Spawn(boss, x, yMin, yMax);
            spawnIndexBoss++;
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3 && spawnIndex < spawnEnemy; j++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            if (spawnIndex2 < spawnEnemy2)
            {
                Spawn(enemy2, x, yMin, yMax);
                spawnIndex2++;
            }

            yield return new WaitForSeconds(2f);
        }

        yield return new WaitForSeconds(3f);

        for (int i = 0; i < 2 && spawnIndexBoss < spawnBoss; i++)
        {
            Spawn(boss, x, yMin, yMax);
            spawnIndexBoss++;
        }

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 4 && spawnIndex < spawnEnemy; j++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            for (int j = 0; j < 2 && spawnIndex1 < spawnEnemy1; j++)
            {
                Spawn(enemy1, x, yMin, yMax);
                spawnIndex1++;
            }

            if (spawnIndex2 < spawnEnemy2)
            {
                Spawn(enemy2, x, yMin, yMax);
                spawnIndex2++;
            }

            yield return new WaitForSeconds(2.5f);
        }

        spawnCoroutine = null;
    }
    public void StopWave()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
    public bool IsDone()
    {
        return (spawnIndex >= spawnEnemy && spawnIndex1 >= spawnEnemy1 &&
            spawnIndex2 >= spawnEnemy2 && spawnIndexBoss >= spawnBoss && ManagerGame.Instance.aliveEnemies.Count == 0);
    }
    public void ResetWave()
    {
        spawnIndex = 0;
        spawnIndex1 = 0;
        spawnIndex2 = 0;
        spawnIndexBoss = 0;
        spawnCoroutine = null;
    }
    public void Spawn(GameObject obj, float x, float yMin, float yMax)
    {
        float y = Random.Range(yMin, yMax);
        GameObject e = Instantiate(obj, new Vector3(x, y, 0f), Quaternion.identity);

        Enemy c = e.GetComponent<Enemy>();
        if (c != null)
            ManagerGame.Instance.aliveEnemies.Add(c);
    }
}
