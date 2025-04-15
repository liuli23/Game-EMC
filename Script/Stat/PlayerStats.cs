using UnityEngine;

public class PlayerStats : CharacterStats
{

    protected override void Start()
    {
        base.Start();

    }

    public override void MakeDamage(int _damage)
    {
        base.MakeDamage(_damage);

        //PlayerManager.instance.player.DamageEffect();
        //�Ķ�˵����ת����CharacterStats
    }

    protected override void Die()
    {
        base.Die();

        PlayerManager.instance.player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;
        
        GetComponent<PlayItemDrop>()?.GenerateDrop();
        //GetComponent<Inventory>()?.UpdataSlotUI();
        //GetComponent<UIItemSlot>()?.CleanUpSlot();
    }

    //�����˺�//�Ķ�˵����ֱ����ԭ�����м�һ����ѡ����
    /*
    public void DoMulitiplierDamage(CharacterStats _targetStats, float _multiplier)
    {
        if (CanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = Mathf.RoundToInt(totalDamage*_multiplier);

        if (CanCrit())//�����жϼ��˺�����
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            //Debug.Log("����"+totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);


        _targetStats.MakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);

    }
    */


}
