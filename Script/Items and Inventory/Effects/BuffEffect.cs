using UnityEngine;

/*//改动说明：同理配合下文使用
public enum StatType
{
    strength, // 力量，影响攻击，暴击效果
    agility, // 敏捷，影响闪避，暴击率
    intelligence, // 智力，影响法伤，法抗
    vitality, // 活力，加血
    damage,
    critChance, // 暴击率
    critPower, // 暴击伤害，初始150%
    maxHp, // 血量上限
    armor, // 护甲
    evasion, // 闪避
    magicResistance, // 法抗
    fireDamage, // 火焰伤害
    iceDamage, // 冰霜伤害
    lightningDamage // 电击伤害
}
*/

[CreateAssetMenu(fileName = "Buff效果", menuName = "数据/效果/Buff")]
public class BuffEffect : ItemEffect
{
    private PlayerStats stats;

    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;
    [SerializeField] private StatType buffType;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        //stats.IncreaseStat(buffAmount, buffDuration, StatToModify());
        //改动说明：配合下方改动使用
        stats.IncreaseStat(buffAmount, buffDuration, stats.GetStat(buffType));
    }


    /*//改动说明，移至CharacterStats
    private Stat StatToModify()
    {
        switch (buffType)
        {
            case StatType.strength:
                return stats.strength;
            case StatType.agility:
                return stats.agility;
            case StatType.intelligence:
                return stats.intelligence;
            case StatType.vitality:
                return stats.vitality;
            case StatType.damage:
                return stats.damage;
            case StatType.critChance:
                return stats.critChance;
            case StatType.critPower:
                return stats.critPower;
            case StatType.maxHp:
                return stats.maxHp;
            case StatType.armor:
                return stats.armor;
            case StatType.evasion:
                return stats.evasion;
            case StatType.magicResistance:
                return stats.magicResistance;
            case StatType.fireDamage:
                return stats.fireDamage;
            case StatType.iceDamage:
                return stats.iceDamage;
            case StatType.lightningDamage:
                return stats.lightningDamage;
            default:
                Debug.LogError("BuffType not supported: " + buffType);
                return null;
        }
    }

    */
}