using UnityEngine;

/*//�Ķ�˵����ͬ���������ʹ��
public enum StatType
{
    strength, // ������Ӱ�칥��������Ч��
    agility, // ���ݣ�Ӱ�����ܣ�������
    intelligence, // ������Ӱ�취�ˣ�����
    vitality, // ��������Ѫ
    damage,
    critChance, // ������
    critPower, // �����˺�����ʼ150%
    maxHp, // Ѫ������
    armor, // ����
    evasion, // ����
    magicResistance, // ����
    fireDamage, // �����˺�
    iceDamage, // ��˪�˺�
    lightningDamage // ����˺�
}
*/

[CreateAssetMenu(fileName = "BuffЧ��", menuName = "����/Ч��/Buff")]
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
        //�Ķ�˵��������·��Ķ�ʹ��
        stats.IncreaseStat(buffAmount, buffDuration, stats.GetStat(buffType));
    }


    /*//�Ķ�˵��������CharacterStats
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