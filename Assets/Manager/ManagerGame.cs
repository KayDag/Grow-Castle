using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class ManagerGame : MonoBehaviour
{
    public static ManagerGame Instance;

    public PlayerStatsManager stats;

    public List<MonoBehaviour> waveObjects;
    private List<IProgress> waves = new List<IProgress>();
    public int currentWave = 0;

    public List<int> checkPoint = new List<int>() { 3, 4, 5, 7, 9, 12, 15 };
    public int count = 0;

    public bool isGame = false;
    private bool isWaveStarted = false;
    public List<Enemy> aliveEnemies;

    public bool isLose = false;
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

        foreach (var w in waveObjects)
        {
            IProgress p = w as IProgress;
            if (p != null)
                waves.Add(p);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        stats = new PlayerStatsManager();
        stats.castle = 1;
        stats.attacker = 1;
        stats.defender = 1;
        stats.booster = 1;
        stats.gold = 50;

        currentWave = 0;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGame) return;
        if (currentWave >= waves.Count) return;

        if (!isWaveStarted)
        {
            waves[currentWave].StartWave();
            isWaveStarted = true;
        }

        if (waves[currentWave].IsDone())
        {
            isGame = false;
            isWaveStarted = false;
            CheckWave();
        }

        if (isLose)
        {
            UIManager.Instance.LoseGame();
            aliveEnemies.Clear();
        }
    }
    public void CheckWave()
    {
        if (currentWave >= checkPoint.Count)
            return;

        if (count >= checkPoint[currentWave])
        {
            UIManager.Instance.WinGame();
            waves[currentWave].ResetWave();
            currentWave++;
            count = 0;
        }
        else
        {
            UIManager.Instance.LoseGame();
        }
    }
    public void UpdateStats()
    {
        Castle.Instance.health += Castle.Instance.baseHealth * 
            Mathf.Pow((int)(1 + (stats.castle - 1) * 0.2), (stats.castle - 1));
        Castle.Instance.stayHealth = Castle.Instance.health;
        AttackerManager.Instance.ApplyStatsAll(ManagerGame.Instance.stats);
        DefenderManager.Instance.ApplyStatsAll(ManagerGame.Instance.stats);
    }
    public void OnAttackerReachCheckpoint(Attacker att)
    {
        count++;
        UIManager.Instance.CheckPoint();
    }
}
