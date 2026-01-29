using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress2 : MonoBehaviour, IProgress
{
    public static Progress2 Instance;

    public GameObject enemy;
    private int spawnEnemy = 20;

    public GameObject enemy1;
    private int spawnEnemy1 = 10;

    public Transform pointA, pointB;

    private int spawnIndex = 0;
    private int spawnIndex1 = 0;
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
        if (!ManagerGame.Instance.isGame) return;
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnWave());
    }
    //>= 30s
    private IEnumerator SpawnWave()
    {
        float x = pointA.position.x;
        float yMin = pointA.position.y;
        float yMax = pointB.position.y;
        while (spawnIndex < spawnEnemy || spawnIndex1 < spawnEnemy1)
        {
            // ⭐ Spawn 2 enemy1
            if (spawnIndex1 < spawnEnemy1)
            {
                Spawn(enemy1, x, yMin, yMax);
                spawnIndex1++;
                Spawn(enemy1, x, yMin, yMax);
                spawnIndex1++;
            }
            yield return new WaitForSeconds(1.5f);

            // ⭐ Spawn 4 enemy
            for (int i = 0; i < 4 && spawnIndex < spawnEnemy; i++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            yield return new WaitForSeconds(3f);
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
        return (spawnIndex >= spawnEnemy && spawnIndex1 >= spawnEnemy1 && ManagerGame.Instance.aliveEnemies.Count == 0);
    }
    public void ResetWave()
    {
        spawnIndex = 0;
        spawnIndex1 = 0;
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
