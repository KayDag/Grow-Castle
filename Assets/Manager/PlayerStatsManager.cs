[System.Serializable]
public class PlayerStatsManager
{
    public float defender;
    public float defenderMax = 7;
    public float attacker;
    public float attackerMax = 6;
    public float castle;
    public float castleMax = 7;
    public float booster;
    public float boosterMax = 5;
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
