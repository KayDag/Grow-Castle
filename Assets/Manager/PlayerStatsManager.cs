using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatsManager
{
    public float defender;
    public float attacker;
    public float castle;
    public float booster;
    public float gold;

    public PlayerStatsManager Clone()
    {
        return new PlayerStatsManager
        {
            defender = defender,
            attacker = attacker,
            castle = castle,
            booster = booster,
            gold = gold
        };
    }
}
