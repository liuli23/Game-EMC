using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayItemDrop : ItemDrop
{

    [Header("玩家掉落")]
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
                //改动说明：循环过程中修改刘表大小，会出现某修bug。因此选择分离循环与修改逻辑
            }

        }
         
        for(int i= 0;i<itemToUnequip.Count;i++)
        {
            inventory.UnequipItem(itemToUnequip[i].data as ItemDataEquipment);
        }
    }

}
