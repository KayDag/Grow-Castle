using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveDatas
{
    public float gold;
    public int wave;
    public int scout;
    public int bomber;
    public int checkPoint;
    public float statsScout;
    public float statsBomber;
    public float statsCastle;
    public float statsBooster;

    public PlayerSaveDatas()
    {
        gold = 50;
        wave = 0;
        scout = 5;
        bomber = 1;
        checkPoint = 0;
        statsScout = 1;
        statsBomber = 1;
        statsCastle = 1;
        statsBooster = 1;
    }
    public void UpdateStats(int w, int sc, int bb, int cp, PlayerStatsManager stats)
    {
        gold = stats.gold;
        wave = w;
        scout = sc;
        bomber = bb;
        checkPoint = cp;
        statsScout = stats.attacker;
        statsBomber = stats.defender;
        statsCastle = stats.castle;
        statsBooster = stats.booster;
    }
}
