using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftSlot : UIItemSlot
{
    protected override void Start()
    {
        base.Start();

    }

    public void SetupCraftSlot(ItemDataEquipment _data)
    {
        if(_data == null) 
            return;

        item.data = _data;
        //Debug.Log(_data.name);
        itemImage.sprite = _data.icon;
        itemText.text = _data.name;

    }



    private void OnEnable()
    {
        UpdataSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //base.OnPointerDown(eventData);
        /*
        ItemDataEquipment craftData = item.data as ItemDataEquipment;

        Inventory.instance.CanCraft(craftData,craftData.craftingMaterial);
        */

        ui.craftWindow.SetupCraftWindow(item.data as ItemDataEquipment);

    }

}
