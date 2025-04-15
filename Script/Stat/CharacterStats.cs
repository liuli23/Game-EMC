using System.Collections;
using UnityEngine;

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


public class CharacterStats : MonoBehaviour
{
    [Header("��Ҫ����")]
    public Stat strength;//������һ������һ����Ч��
    public Stat agility;//���ݣ�һ���ܣ�һ������
    public Stat intelligence;//������һ���ˣ�һ����
    public Stat vitality;//��������Ѫ


    [Header("��������")]
    public Stat damage;
    public Stat critChance;//����
    public Stat critPower;//����,��ʼ150%


    [Header("��������")]
    public Stat maxHp;//Ѫ��
    public Stat armor;//����
    public Stat evasion;//����
    public Stat magicResistance;//����


    [Header("��������")]
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

        if (Crit)//�����жϼ��˺�����
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            //Debug.Log("����"+totalDamage);
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
        //fx.StartCoroutine("FlashFX");//�ƻ�ȥ��

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
        //���㷨��
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

    //���㲢Ѫ��
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

    //�����ж�
    protected bool CanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        //��������
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Miss");
            return true;//���ܻ��ƣ�����Ҫ��ȥ������Ҫ��
        }
        return false;
    }

    //���׽��㣬�Ӽ�
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    //�����ж�
    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    //�����˺�����
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
