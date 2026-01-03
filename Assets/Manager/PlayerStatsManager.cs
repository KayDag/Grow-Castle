using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatsManager
{
    public float damageDefender;
    public float speedAttacker;
    public float healthCastle;
    public float cooldownBooster;
    public float gold;

    public PlayerStatsManager Clone()
    {
        return new PlayerStatsManager
        {
            damageDefender = damageDefender,
            speedAttacker = speedAttacker,
            healthCastle = healthCastle,
            cooldownBooster = cooldownBooster,
            gold = gold
        };
    }
}
