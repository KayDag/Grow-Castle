using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Progress3 : MonoBehaviour,  IProgress
{
    public static Progress3 Instance;

    public GameObject enemy;
    private int spawnEnemy = 25;

    public GameObject enemy1;
    private int spawnEnemy1 = 8;

    public GameObject enemy2;
    private int spawnEnemy2 = 3;

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
    //Time wave = 36s
    private IEnumerator SpawnWave()
    {
        float x = pointA.position.x;
        float yMin = pointA.position.y;
        float yMax = pointB.position.y;
        while (spawnIndex < spawnEnemy || spawnIndex1 < spawnEnemy1 || spawnIndex2 < spawnEnemy2)
        {
            //spawn 9 enemy
            for (int i = 0; i < 9; i++)
            {
                if (spawnIndex < spawnEnemy)
                {
                    Spawn(enemy, x, yMin, yMax);
                    spawnIndex++;
                }
            }
            yield return new WaitForSeconds(2.2f);
            //8 wave spawn 1 enemy1 và 2 enemy thường (cách nhau 2.s) đến wave thứ 4 thì spawn enemy2
            for (int i = 0; i < 8; i++)
            {
                if (spawnIndex1 < spawnEnemy1)
                {
                    Spawn(enemy1, x, yMin, yMax);
                    spawnIndex1++;
                }
                for (int j = 0; j < 2; j++)
                {
                    if (spawnIndex < spawnEnemy)
                    {
                        Spawn(enemy, x, yMin, yMax);
                        spawnIndex++;
                    }
                }
                if (i == 4)
                {
                    yield return new WaitForSeconds(1.2f);
                    Spawn(enemy2, x, yMin, yMax);
                    spawnIndex2++;
                    yield return new WaitForSeconds(1.5f);
                }
                else
                {
                    yield return new WaitForSeconds(1.8f);
                }
            }
            //khi kết thúc 8 wave quái thường thì xuất hiện cùng lúc 2 enemy2
            Spawn(enemy2, x, yMin, yMin);
            Spawn(enemy2, x, yMax, yMax);
            spawnIndex2 += 2;
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
