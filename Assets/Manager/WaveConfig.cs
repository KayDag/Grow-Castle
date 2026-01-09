using System;
using UnityEngine;

[Serializable]
public class WaveConfig
{
    [Header("Wave Info")]
    public int waveIndex;

    [Header("Checkpoint")]
    public int checkpointRequired;

    [Header("Enemy Count")]
    public int normalCount;
    public int runnerCount;
    public int tankCount;
    public bool hasBoss;

    [Header("Spawn Control")]
    public int spawnPerBatch;
    public float spawnInterval;

    [Header("Reward")]
    public int rewardGold;

    public int TotalEnemies()
    {
        int total = normalCount + runnerCount + tankCount;
        if (hasBoss) total += 1;
        return total;
    }
}
