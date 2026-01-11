using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class ManagerGame : MonoBehaviour
{
    public static ManagerGame Instance;

    public PlayerStatsManager stats;
    private PlayerStatsManager waveStartStats;
    private float waveStartGold;
    private int waveStartCount;
    private int waveStartWave;

    public List<MonoBehaviour> waveObjects;
    private List<IProgress> waves = new List<IProgress>();
    public int currentWave = 0;

    public List<int> checkPoint = new List<int>() { 3, 4, 5, 7, 9, 12, 15 };
    public int count = 0;

    public List<int> scoreAdd = new List<int>() { 2, 3, 3, 4, 4, 5 };

    public bool isGame = false;
    private bool isWaveStarted = false;
    public List<Enemy> aliveEnemies;
    public bool waitingForPlayer = false;

    public int aliveScouts = 0;
    public int completedScouts = 0;

    public bool isEndWave = false;
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
            EnterWave();
        }

        if ((waves[currentWave].IsDone() && aliveScouts == 0) || Castle.Instance.health <= 0)
        {
            isGame = false;
            isWaveStarted = false;
            waitingForPlayer = true;
            DefenderManager.Instance.DestroyBall();
            foreach (var e in aliveEnemies)
                if (e != null) Destroy(e.gameObject);
            aliveEnemies.Clear();
            CheckWave();
        }
        if (isEndWave)
        {
            AttackerManager.Instance.NewWave();
            isEndWave = false;
        }
    }
    //Go Wave
    public void EnterWave()
    {
        waveStartStats = stats.Clone();
        waveStartGold = stats.gold;
        waveStartCount = count;
        waveStartWave = currentWave;

        isEndWave = false;
        waves[currentWave].ResetWave();
        isWaveStarted = true;
        UpdateStats();
        waves[currentWave].StartWave();
    }
    public void CheckWave()
    {
        if (currentWave >= checkPoint.Count)
            return;
        isEndWave = true;
        waves[currentWave].StopWave();
        //Thắng
        if (count >= checkPoint[currentWave] && Castle.Instance.health > 0)
        {
            UIManager.Instance.WinGame();
            currentWave++;
            count = 0;
            stats.gold += 50 + currentWave * 25;
        }
        //Thua
        else if (Castle.Instance.health <= 0)
        {
            count = 0;
            UIManager.Instance.LoseGame();
        }
        //Thua nhưng vẫn được giữ checkpoint
        else
        {
            UIManager.Instance.LoseGame();
        }
    }
    public void ResetCurrentWave()
    {
        waves[currentWave].StopWave();
        // reset stats
        stats.castle = waveStartStats.castle;
        stats.attacker = waveStartStats.attacker;
        stats.defender = waveStartStats.defender;
        stats.booster = waveStartStats.booster;
        stats.gold = waveStartGold;

        count = waveStartCount;
        currentWave = waveStartWave;

        // destroy enemy còn sống
        DestroyEnemy();

        // destroy đạn
        DefenderManager.Instance.DestroyBall();

        // reset castle
        Castle.Instance.health = Castle.Instance.stayHealth;
        AttackerManager.Instance.ResetScouts();
        waves[currentWave].ResetWave();

        isGame = false;
        isWaveStarted = false;
    }

    public void UpdateStats()
    {
        Castle.Instance.health = Castle.Instance.baseHealth * (float)(1 + (stats.castle - 1) * 0.2);
        Castle.Instance.stayHealth = Castle.Instance.health;
        AttackerManager.Instance.ApplyStatsAll(ManagerGame.Instance.stats);
        DefenderManager.Instance.ApplyStatsAll(ManagerGame.Instance.stats);
    }
    public void OnAttackerReachCheckpoint(Attacker att)
    {
        count++;
        completedScouts++;
        stats.gold += stats.attacker * 10;
        UIManager.Instance.CheckPoint();
    }
    void DestroyEnemy()
    {
        foreach (var e in aliveEnemies)
            if (e != null) Destroy(e.gameObject);
        aliveEnemies.Clear();
    }
}
