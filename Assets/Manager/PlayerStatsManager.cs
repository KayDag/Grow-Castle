using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatsManager
{
    public float defender;
    public float defenderMax = 6;
    public float attacker;
    public float attackerMax = 5;
    public float castle;
    public float castleMax = 6;
    public float booster;
    public float boosterMax = 4;
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
