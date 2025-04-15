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

[CreateAssetMenu(fileName = "新物品信息", menuName = "数据/装备")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("特殊效果")]
    public float itemCooldown;
    public ItemEffect[] itemEffect;
    //[TextArea]
    //public string itemEffectDescription;



    [Header("主要属性")]
    public int strength;//力量，一攻击，一暴击效果
    public int agility;//敏捷，一闪避，一暴击率
    public int intellgence;//智力，一法伤，一法抗
    public int vitality;//活力，加血


    [Header("攻击属性")]
    public int damage;
    public int critChance;//暴击
    public int critPower;//爆伤,初始150%


    [Header("生存属性")]
    public int maxHp;//血限
    public int armor;//护甲
    public int evasion;//闪避
    public int magicResistance;//法抗


    [Header("法术属性")]
    public int firDamage;
    public int iceDamage;
    public int lightningDamage;


    [Header("制作材料")]
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
        // 主要属性
        AddItemDescription(strength, "Strength");
        AddItemDescription(agility, "Agility");
        AddItemDescription(intellgence, "Intelligence");
        AddItemDescription(vitality, "Vitality");

        // 攻击属性
        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit Chance");
        AddItemDescription(critPower, "Crit Power");

        // 生存属性
        AddItemDescription(maxHp, "Max HP");
        AddItemDescription(armor, "Armor");
        AddItemDescription(evasion, "Evasion");
        AddItemDescription(magicResistance, "Magic Resistance");

        // 法术属性
        AddItemDescription(firDamage, "Fire Damage");
        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(lightningDamage, "Lightning Damage");


        for(int i = 0;i<itemEffect.Length;i++)
        {
            if(itemEffect[i].effectDescription.Length >0)
            {
                sb.AppendLine();
                sb.AppendLine("效果：" + itemEffect[i].effectDescription);
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

        *///改动说明：适应多效果


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
