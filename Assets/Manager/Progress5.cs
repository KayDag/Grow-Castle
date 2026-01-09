using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Progress5 : MonoBehaviour
{
    public static Progress5 Instance;

    public GameObject enemy;

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
        GameObject e = Instantiate(enemy, new Vector3(9.15f, -2.0f, 0), Quaternion.identity);
        Enemy eComp = e.GetComponent<Enemy>();
        if (eComp != null)
        {
            ManagerGame.Instance.aliveEnemies.Add(eComp);
        }
        yield return null;
    }
    public bool IsDone()
    {
        return false;
    }
    public void ResetWave()
    {
    }
}
