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
        //改动说明，转移至CharacterStats
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

    //倍率伤害//改动说明，直接在原函数中加一个可选参数
    /*
    public void DoMulitiplierDamage(CharacterStats _targetStats, float _multiplier)
    {
        if (CanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        totalDamage = Mathf.RoundToInt(totalDamage*_multiplier);

        if (CanCrit())//暴击判断及伤害计算
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            //Debug.Log("暴击"+totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);


        _targetStats.MakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);

    }
    */


}
