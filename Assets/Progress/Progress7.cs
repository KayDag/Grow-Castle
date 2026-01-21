using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress7 : MonoBehaviour, IProgress
{
    public static Progress7 Instance;

    public GameObject enemy;
    private int spawnEnemy = 45;

    public GameObject enemy1;
    private int spawnEnemy1 = 22;

    public GameObject enemy2;
    private int spawnEnemy2 = 12;

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

        // Phase 1 – spam quân ồ ạt (20s) // 45
        for (int i = 0; i < 9; i++) 
        {
            for (int j = 0; j < 5 && spawnIndex < spawnEnemy; j++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            yield return new WaitForSeconds(2.2f);
        }

        for (int i = 0; i < 11; i++) //44 + 1 boss
        {
            if (spawnIndex1 < spawnEnemy1)
            {
                Spawn(enemy1, x, yMin, yMax);
                Spawn(enemy1, x, yMin, yMax);
                spawnIndex1 += 2;
            }

            if (spawnIndex2 < spawnEnemy2)
            {
                Spawn(enemy2, x, yMin, yMax);
                Spawn(enemy2, x, yMin, yMax);
                spawnIndex2 += 2;
            }

            // Boss 1 xuất hiện ở lượt thứ 4
            if (i == 3 && spawnIndexBoss < spawnBoss)
            {
                while (ManagerGame.Instance.aliveEnemies.Count > 0)
                    yield return null;

                Spawn(boss, x, 0f, 0f);
                spawnIndexBoss++;
            }

            yield return new WaitForSeconds(3f);
        }

        // Phase 3 – boss cuối kết liễu
        while (ManagerGame.Instance.aliveEnemies.Count > 0)
            yield return null;

        if (spawnIndexBoss < spawnBoss)
        {
            Spawn(boss, x, 0f, 0f);
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
