using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress5 : MonoBehaviour,  IProgress
{
    public static Progress5 Instance;

    public GameObject enemy;
    private int spawnEnemy = 35;

    public GameObject enemy1;
    private int spawnEnemy1 = 15;

    public GameObject enemy2;
    private int spawnEnemy2 = 8;

    public GameObject boss;
    private int spawnBoss = 1;

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
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnWave());
    }

    //Time wave = 56s
    private IEnumerator SpawnWave()
    {
        float x = pointA.position.x;
        float yMin = pointA.position.y;
        float yMax = pointB.position.y;

        // dồn lính (20s)
        for (int i = 0; i < 7; i++)
        {
            // 5 enemy thường
            for (int j = 0; j < 5 && spawnIndex < spawnEnemy; j++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            yield return new WaitForSeconds(3f);
        }

        // enemy1 xuất hiện (24s)
        for (int i = 0; i < 6; i++)
        {
            if (spawnIndex1 < spawnEnemy1)
            {
                Spawn(enemy1, x, yMin, yMax);
                spawnIndex1++;
            }

            if (spawnIndex2 < spawnEnemy2)
            {
                Spawn(enemy2, x, yMin, yMax);
                spawnIndex2++;
            }

            // thêm 2 enemy thường
            for (int j = 0; j < 2 && spawnIndex < spawnEnemy; j++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            yield return new WaitForSeconds(4f);
        }
        while (ManagerGame.Instance.aliveEnemies.Count > 0)
            yield return null;

        //Boss xuất hiện
        if (spawnIndexBoss < spawnBoss)
        {
            GameObject b = Instantiate(boss, new Vector3(x, 0f, 0f), Quaternion.identity);
            Enemy c = b.GetComponent<Enemy>();
            if (c != null)
                ManagerGame.Instance.aliveEnemies.Add(c);

            spawnIndexBoss++;
        }

        spawnCoroutine = null;
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
