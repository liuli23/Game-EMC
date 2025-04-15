using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipmentSlot : UIItemSlot
{
    public EquipmentType slotType;
    private PlayItemDrop playItemDrop;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot -" + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;//改动说明：不知道有没有必要，我记得我在别的地方也做了这个

        //base.OnPointerDown(eventData);
        Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
        Inventory.instance.AddItem(item.data as ItemDataEquipment);

        ui.itemToolTip.HideToolTip();

        CleanUpSlot();



    }


}
