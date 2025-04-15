using System.Collections;
using UnityEngine;

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


public class CharacterStats : MonoBehaviour
{
    [Header("主要属性")]
    public Stat strength;//力量，一攻击，一暴击效果
    public Stat agility;//敏捷，一闪避，一暴击率
    public Stat intelligence;//智力，一法伤，一法抗
    public Stat vitality;//活力，加血


    [Header("攻击属性")]
    public Stat damage;
    public Stat critChance;//暴击
    public Stat critPower;//爆伤,初始150%


    [Header("生存属性")]
    public Stat maxHp;//血限
    public Stat armor;//护甲
    public Stat evasion;//闪避
    public Stat magicResistance;//法抗


    [Header("法术属性")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;


    public bool isDead { get; private set; }
    public bool isWuDi {  get; private set; }
    public int currentHp;


    public System.Action onHpChanged;
    public EntityFX fx;

    protected virtual void Start()
    {
        //currentHp = maxHp.GetValue();
        critPower.SetDefaultValue(150);
        currentHp = GetMaxHpValue();
        fx = gameObject.GetComponent<EntityFX>();
    }

    public virtual void IncreaseStat(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));

    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }



    public virtual void DoDamage(CharacterStats _targetStats, float _multiplier = 1)
    {
        if (CanAvoidAttack(_targetStats)) return;

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);


        bool Crit = CanCrit();

        if (Crit)//暴击判断及伤害计算
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            //Debug.Log("暴击"+totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);


        _targetStats.MakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);

        //if(_targetStats.transform != null )Debug.Log(_targetStats.transform);
        fx.CreateHitFX(_targetStats.transform,Crit);

        

    }

    public virtual void MakeDamage(int _damage)
    {
        if(isWuDi)return;

        DecreaseHp(_damage);

        //Debug.Log(_damage);
        GetComponent<Entity>().DamageImpact();
        //fx.StartCoroutine("FlashFX");//移回去了

        if (currentHp <= 0 && !isDead)
        {
            Die();
        }

    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _firDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _firDamage + _iceDamage + _lightningDamage + intelligence.GetValue();
        //
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 2);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        //计算法抗
        _targetStats.MakeDamage(totalMagicDamage);


    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
            return;

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;

    }

    //计算并血限
    public int GetMaxHpValue()
    {
        return maxHp.GetValue() + vitality.GetValue() * 5;
    }


    public virtual void IncreaseHp(int _amount)
    {
        currentHp += _amount;

        if (currentHp > GetMaxHpValue())
            currentHp = GetMaxHpValue();
        if (onHpChanged != null)
            onHpChanged();

    }

    protected virtual void DecreaseHp(int _damage)
    {
        currentHp -= _damage;
        onHpChanged?.Invoke();
    }


    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    }

    public void MakeWuDi(bool _isWuDi) => isWuDi = _isWuDi;

    //闪避判断
    protected bool CanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        //总闪避率
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Miss");
            return true;//闪避机制，不想要就去掉，不要紧
        }
        return false;
    }

    //护甲结算，加减
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    //暴击判断
    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    //暴击伤害结算
    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }


    public Stat GetStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                return strength;
            case StatType.agility:
                return agility;
            case StatType.intelligence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.critChance:
                return critChance;
            case StatType.critPower:
                return critPower;
            case StatType.maxHp:
                return maxHp;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.magicResistance:
                return magicResistance;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightningDamage:
                return lightningDamage;
            default:
                Debug.LogError("BuffType not supported: " + _statType);
                return null;
        }
    }


}
