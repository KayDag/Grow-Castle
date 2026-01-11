using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progess1 : MonoBehaviour, IProgress
{
    public static Progess1 Instance;

    public GameObject enemy;
    private int spawnEnemy = 15;

    public Transform pointA, pointB;          

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
        spawnCoroutine = StartCoroutine(SpawnWave());
    }
    //>= 9s
    IEnumerator SpawnWave()
    {
        float x = pointA.position.x;
        float yMin = pointA.position.y;
        float yMax = pointB.position.y;

        while (spawnIndex < spawnEnemy)
        {
            int batchCount = Mathf.Min(5, spawnEnemy - spawnIndex);

            for (int i = 0; i < batchCount; i++)
            {
                Spawn(enemy, x, yMin, yMax);
                spawnIndex++;
            }

            yield return new WaitForSeconds(5f); // ⭐ 3s sau mới spawn lượt tiếp
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
    public void Spawn(GameObject obj, float x, float yMin, float yMax)
    {
        float y = Random.Range(yMin, yMax);
        GameObject e = Instantiate(obj, new Vector3(x, y, 0f), Quaternion.identity);

        Enemy c = e.GetComponent<Enemy>();
        if (c != null)
            ManagerGame.Instance.aliveEnemies.Add(c);
    }
}
