using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIItemSlot : MonoBehaviour, IPointerDownHandler ,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }



    public void UpdataSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            //Debug.Log("图像");
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;//改动说明：标注出物品槽
        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;

        if(Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);

        }

        /*
        if(item.data.itemType == ItemType.Equipment)
            Debug.Log(item.data.itemName);
        */
        if(item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(item.data);
        }

        ui.itemToolTip.HideToolTip();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        //throw new System.NotImplementedException();
        //Debug.Log("展示信息");

        //动态偏移
        /*
        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 600)
            xOffset = -75;
        else xOffset = 75;
        if (mousePosition.y > 320)
            yOffset = -75;
        else yOffset = 75;
        */

        ui.itemToolTip.ShowToolTip(item.data as ItemDataEquipment);

        //ui.itemToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);

        //改动说明：移至基类中


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null) return;
        //throw new System.NotImplementedException();
        //Debug.Log("隐藏信息");
        ui.itemToolTip.HideToolTip();

    }
}
