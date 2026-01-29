using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress4 : MonoBehaviour, IProgress
{
    public static Progress4 Instance;

    public GameObject enemy;
    private int spawnEnemy = 36;

    public GameObject enemy1;
    private int spawnEnemy1 = 16;

    public GameObject enemy2;
    private int spawnEnemy2 = 5;

    public Transform pointA, pointB;

    private int spawnIndex = 0;
    private int spawnIndex1 = 0;
    private int spawnIndex2 = 0;
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

        // spawn 2 enemy2 đầu tiên
        for (int i = 0; i < 2 && spawnIndex2 < spawnEnemy2; i++)
        {
            Spawn(enemy2, x, yMin, yMax);
            spawnIndex2++;
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 6 && spawnIndex < spawnEnemy; i++)
        {
            Spawn(enemy, x, yMin, yMax);
            spawnIndex++;
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 4 && spawnIndex1 < spawnEnemy1; i++)
        {
            Spawn(enemy1, x, yMin, yMax);
            spawnIndex1++;
        }
        yield return new WaitForSeconds(2f);

        //chạy 3 wave lớn
        for (int w = 0; w < 3; w++)
        {
            // spawn 10 enemy thường
            for (int i = 0; i < 10 && spawnIndex < spawnEnemy; i++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            yield return new WaitForSeconds(4f);

            // spawn 4 enemy1
            for (int i = 0; i < 4 && spawnIndex1 < spawnEnemy1; i++)
            {
                Spawn(enemy1, x, yMin, yMax);
                spawnIndex1++;
            }

            yield return new WaitForSeconds(3f);

            // spawn 1 enemy2
            if (spawnIndex2 < spawnEnemy2)
            {
                Spawn(enemy2, x, yMin, yMax);
                spawnIndex2++;
            }

            // cách wave 3s
            yield return new WaitForSeconds(4f);
        }
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
            spawnIndex2 >= spawnEnemy2 && ManagerGame.Instance.aliveEnemies.Count == 0);
    }
    public void ResetWave()
    {
        spawnIndex = 0;
        spawnIndex1 = 0;
        spawnIndex2 = 0;
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
