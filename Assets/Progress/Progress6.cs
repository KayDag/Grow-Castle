using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress6 : MonoBehaviour, IProgress
{
    public static Progress6 Instance;

    public GameObject enemy;
    private int spawnEnemy = 40;

    public GameObject enemy1;
    private int spawnEnemy1 = 18;

    public GameObject enemy2;
    private int spawnEnemy2 = 12;

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

        // Phase 1 – spam lính nhanh (20s) // 22
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 2 && spawnIndex < spawnEnemy; j++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            yield return new WaitForSeconds(0.5f);
        }

        // Phase 2 – elite & heavy dồn dập (30s) //48

        for (int i = 0; i < 6; i++) 
        {
            if (spawnIndex1 < spawnEnemy1)
            {
                for (int j = 0; j < 3; j++)
                {
                    Spawn(enemy, x, yMin, yMax);
                    spawnIndex++;
                    Spawn(enemy1, x, yMin, yMax);
                    spawnIndex1++;
                }
            }

            if (spawnIndex2 < spawnEnemy2)
            {
                for (int j = 0; j < 2; j++)
                {
                    Spawn(enemy2, x, yMin, yMax);
                    spawnIndex2++;
                }
            }

            yield return new WaitForSeconds(3f);
        }

        // Phase 3 – nghỉ rất ngắn
        yield return new WaitForSeconds(2f);

        // Phase 4 – Boss xuất hiện khi vẫn còn quái lẻ
        if (spawnIndexBoss < spawnBoss)
        {
            Spawn(boss, x, yMin, yMax);
            spawnIndexBoss++;
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
