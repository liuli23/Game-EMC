using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayItemDrop : ItemDrop
{

    [Header("��ҵ���")]
    [SerializeField] private float chanceToLoseItems;

    public override void GenerateDrop()
    {
        //base.GenerateDrop();
        Inventory inventory = Inventory.instance;

        List<InventoryItem> currentEquipment = inventory.GetEquipmentList();
        List<InventoryItem> itemToUnequip = new List<InventoryItem>();

        for (int i = 0; i < currentEquipment.Count; i++)
        {
            InventoryItem item = currentEquipment[i];
            if (Random.Range(0,100) <= chanceToLoseItems)
            {
                DropItem(item.data);
                itemToUnequip.Add(item);
                //inventory.UnequipItem(item.data as ItemDataEquipment);
                //�Ķ�˵����ѭ���������޸������С�������ĳ��bug�����ѡ�����ѭ�����޸��߼�
            }

        }
         
        for(int i= 0;i<itemToUnequip.Count;i++)
        {
            inventory.UnequipItem(itemToUnequip[i].data as ItemDataEquipment);
        }
    }

}
