using Sirenix.OdinInspector;

[GUIColor(0.22f, 0.72f, 0.55f, 1)]
public enum ETalentType
{
    None = 0,
    BasicAttribute = 1,
    Special = 2,
}

[GUIColor(1, 0.55f, 0, 1)]
public enum ESpecialTalent
{
    None = 0,
    BattleReset = 1,
    AutoComplete = 2,
    AutoCompleteZombieRushDungeons = 3,
    AutoCompleteBossRushDungeons = 4,
    TimelineSkip = 5,
}

[GUIColor(0.88f, 0.21f, 0.12f, 1)]
public enum EBuffType {
    None = 0,
    AllDamageUnit = 1,
    AllHealthUnit = 2,
    CoinGained = 3,
    IncreaseFoodProduction = 4,
    ChanceToDropDoubleCoin = 5,
    CriticalChance = 6,
    CriticalDamage = 10,
    TowerHealth = 7,
    IncreaseBlockChance = 8,
    ExtraCoin = 9,
    ExtraFood = 11,
    AdBoost = 12,
}