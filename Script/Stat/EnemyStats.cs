using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop dropSystem;
    public Stat moneyDropAmount;


    [Header("�ȼ�����")]
    [SerializeField] private int level;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier;


    protected override void Start()
    {
        moneyDropAmount.SetDefaultValue(100);
        ApplyLevelModifiers();//�����������˺�����

        base.Start();
        dropSystem = GetComponent<ItemDrop>();
        enemy = GetComponent<Enemy>();



    }

    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHp);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);

        Modify(moneyDropAmount);


    }

    private void Modify(Stat _stat)
    {
        for(int i=1;i<level;i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }

    }


    public override void MakeDamage(int _damage)
    { 
        base.MakeDamage(_damage);

        //enemy.DamageEffect();//�ܻ��Ӿ�Ч��
        //�Ķ�˵����ת����CharacterStats
    }


    protected override void Die()
    {
        base.Die();
        enemy.Die();

        PlayerManager.instance.currency += moneyDropAmount.GetValue();
        dropSystem.GenerateDrop();

        Destroy(gameObject, 5f);
    }

}
