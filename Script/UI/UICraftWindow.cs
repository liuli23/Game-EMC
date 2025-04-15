using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UICraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemDataEquipment _data)
    {
        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0;i < _data.craftingMaterial.Count;i++)
        {
            if (_data.craftingMaterial.Count > materialImage.Length)
                Debug.LogWarning("≥¨∑∂Œß¡À");

            materialImage[i].sprite = _data.craftingMaterial[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _data.craftingMaterial[i].stackSize.ToString();
            materialSlotText.color = Color.white;

        }


        itemIcon.sprite = _data.icon;
        itemName.text = _data.name;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.RemoveAllListeners();

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data,_data.craftingMaterial));

    }


}
