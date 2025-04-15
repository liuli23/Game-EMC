using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask

}

[CreateAssetMenu(fileName = "����Ʒ��Ϣ", menuName = "����/װ��")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("����Ч��")]
    public float itemCooldown;
    public ItemEffect[] itemEffect;
    //[TextArea]
    //public string itemEffectDescription;



    [Header("��Ҫ����")]
    public int strength;//������һ������һ����Ч��
    public int agility;//���ݣ�һ���ܣ�һ������
    public int intellgence;//������һ���ˣ�һ����
    public int vitality;//��������Ѫ


    [Header("��������")]
    public int damage;
    public int critChance;//����
    public int critPower;//����,��ʼ150%


    [Header("��������")]
    public int maxHp;//Ѫ��
    public int armor;//����
    public int evasion;//����
    public int magicResistance;//����


    [Header("��������")]
    public int firDamage;
    public int iceDamage;
    public int lightningDamage;


    [Header("��������")]
    public List<InventoryItem> craftingMaterial;


    private int descriptionLines;

    public void Effect(Transform _enemyPosition)
    {
        foreach (var item in itemEffect)
        {
            item.ExecuteEffect(_enemyPosition);
        }


    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intellgence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHp.AddModifier(maxHp);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(firDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);

    }

    public void RemoveModifiers() 
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intellgence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHp.RemoveModifier(maxHp);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(firDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLines = 0;
        // ��Ҫ����
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intellgence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        // ��������
        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit Chance");
        AddItemDescription(critPower, "Crit Power");

        // ��������
        AddItemDescription(maxHp, "Max HP");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Resistance");

        // ��������
        AddItemDescription(firDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightningDamage, "Lightning Damage");


        for(int i = 0;i<itemEffect.Length;i++)
        {
            if(itemEffect[i].effectDescription.Length >0)
            {
                sb.AppendLine();
                sb.AppendLine("Ч����" + itemEffect[i].effectDescription);
                descriptionLines++;
            }

        }


        if(descriptionLines < 5)
        {
            for(int i = descriptionLines;i<5;i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }
        /*

        if (itemEffectDescription.Length > 0)
        {
            sb.AppendLine();
            sb.Append(itemEffectDescription);
        }

        *///�Ķ�˵������Ӧ��Ч��


        return sb.ToString();
    }

    private void AddItemDescription(int _value , string _name)
    {
        if(_value!=0)
        {
            if (sb.Length > 0) sb.AppendLine();

            if(_value > 0) sb.Append( "+" + _value + ""+ _name);

            descriptionLines++;
        }

    }


}
