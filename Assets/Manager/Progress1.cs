using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progess1 : MonoBehaviour, IProgress
{
    public static Progess1 Instance;

    public GameObject enemy;
    public int spawnEnemy = 5;

    public Transform pointA, pointB;

    public float spawnInterval = 2f;          
    public int spawnPerTime = 2;             

    private int spawnIndex = 0;
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
        // tránh gọi spawn nhiều lần
        if (spawnCoroutine != null) return;

        spawnIndex = 0;
        spawnCoroutine = StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        float x = pointA.position.x;
        float yMin = pointA.position.y;
        float yMax = pointB.position.y;

        while (spawnIndex < spawnEnemy)
        {
            // tạo list chứa Y cho mỗi con trong lần spawn
            List<float> spawnYs = new List<float>();

            for (int i = 0; i < spawnPerTime; i++)
            {
                if (spawnIndex + i >= spawnEnemy) break;

                // chia đều đoạn và random trong phần nhỏ
                float segmentHeight = (yMax - yMin) / spawnPerTime;
                float y = yMin + segmentHeight * i + Random.Range(0f, segmentHeight);

                spawnYs.Add(y);
            }

            // spawn từng con
            for (int i = 0; i < spawnYs.Count; i++)
            {
                GameObject enemy1 = Instantiate(enemy, new Vector3(x, spawnYs[i], 0f), Quaternion.identity);

                Enemy enemyComp = enemy1.GetComponent<Enemy>();
                if (enemyComp != null)
                    ManagerGame.Instance.aliveEnemies.Add(enemyComp);

                spawnIndex++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        spawnCoroutine = null;
    }

    public void RemoveEnemy(Enemy enemy)
    {
        if (ManagerGame.Instance.aliveEnemies.Contains(enemy))
            ManagerGame.Instance.aliveEnemies.Remove(enemy);
    }

    public bool IsDone()
    {
        return (spawnIndex >= spawnEnemy && ManagerGame.Instance.aliveEnemies.Count == 0);
    }
    public void ResetWave()
    {
        spawnIndex = 0;
        spawnCoroutine = null;
    }

}
