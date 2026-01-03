using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class ManagerGame : MonoBehaviour
{
    public static ManagerGame Instance;

    public PlayerStatsManager stats;

    public List<int> checkPointWave = new List<int>() { 1, 3, 6, 10, 15};
    public int countWave;
    private List<bool> isWave = new List<bool>() { false, false, false, false, false };
    public float wave = 0;
    public List<Enemy> aliveEnemies = new List<Enemy>();

    public TextMeshProUGUI progress;

    public bool isGame = false;

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
    // Start is called before the first frame update
    void Start()
    {
        stats = new PlayerStatsManager();
        stats.healthCastle = 1;
        stats.speedAttacker = 1;
        stats.damageDefender = 1;
        stats.cooldownBooster = 1;
        stats.gold = 50;

        wave = 0;
        Progress4.Instance.StartWave();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
