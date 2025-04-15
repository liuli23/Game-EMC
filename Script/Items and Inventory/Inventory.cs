using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour,ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingEquipment;

    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDict;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDict;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDict;

    //public List<itemData> inventory = new List<itemData>();

    [Header("库存UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;



    private UIItemSlot[] inventoryItemSlot;
    private UIItemSlot[] stashItemSlot;
    private UIEquipmentSlot[] equipmentSlot;
    private UIStatSlot[] statSlot;


    [Header("药水冷却")]
    public float flaskCooldown;
    [SerializeField]private float lastTimeUseFlask;


    [Header("数据对照")]
    //public string[] assetNames;
    public List<ItemData> itemDataBase;
    public List<InventoryItem> loadedItems;
    public List<ItemDataEquipment> loadedEquipment;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }


    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDict = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDict = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDict = new Dictionary<ItemDataEquipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UIItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UIItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UIEquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UIStatSlot>();//注意GetComponents
        AddStartingItems();

        lastTimeUseFlask = -1000;

    }

    
    private void AddStartingItems()
    {
        foreach(ItemDataEquipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        if(loadedItems.Count > 0)
        {
            foreach(InventoryItem item in loadedItems)
            {
                for(int i = 0;i<item.stackSize;i++)
                {
                    AddItem(item.data);
                }
            }

            return;
        }


        for (int i = 0; i < startingEquipment.Count; i++)
        {
            if(startingEquipment[i] != null)
                AddItem(startingEquipment[i]);
        }
    }
    


    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDict)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDict.Add(newEquipment, newItem);

        newEquipment.AddModifiers();//数值修正

        RemoveItem(_item);

        UpdataSlotUI();
    }

    public void UnequipItem(ItemDataEquipment itemToRemove)
    {
        if (equipmentDict.TryGetValue(itemToRemove, out InventoryItem value))
        {
            //AddItem(itemToRemove);
            equipment.Remove(value);
            equipmentDict.Remove(itemToRemove);
            
            itemToRemove.RemoveModifiers();//数值修正
        }
    }

    public void UpdataSlotUI()
    {
        
        for(int i =0; i<equipmentSlot.Length;i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDict)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdataSlot(item.Value);
            }
        }
        

        for(int i =0;i<inventoryItemSlot.Length;i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }
        for(int i=0;i<stashItemSlot.Length;i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdataSlot(inventory[i]);

        }

        for(int i = 0;i < stash.Count; i++)
        {
            stashItemSlot[i].UpdataSlot(stash[i]);

        }

        for(int i=0;i<statSlot.Length;i++)
        {
            statSlot[i].UpdataStatValueUI();
        }


    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
            AddToInventory(_item);
        else if(_item.itemType == ItemType.Material)
            AddToStash(_item);


        UpdataSlotUI();

    }

    private void AddToStash(ItemData _item)
    {
        if (stashDict.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDict.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        //inventory.Add(_item); 
        if (inventoryDict.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDict.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDict.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value); 
                inventoryDict.Remove(_item);

            }
            else value.RemoveStack();
        }

        if(stashDict.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDict.Remove(_item);
            }
            else stashValue.RemoveStack();
        }



        UpdataSlotUI();
    }

    //背包空间判定
    public bool CanAddItem()
    {
        if(inventory.Count >= inventoryItemSlot.Length)
        {
            //Debug.Log("No more space");
            return false;
        }
        return true;
    }

    public bool CanCraft(ItemDataEquipment _itemToCraft, List<InventoryItem> _requireMaterials)
    {
        foreach (var requiredItem in _requireMaterials)
        {
            if (stashDict.TryGetValue(requiredItem.data, out InventoryItem stashItem))
            {
                if (stashItem.stackSize < requiredItem.stackSize)
                {
                    Debug.Log("材料不足" + requiredItem.data.name);
                    return false;
                }
            }
            else
            {
                Debug.Log("缺少" + requiredItem.data.name);
                return false;
            }
        }

        foreach (var requiredItem in _requireMaterials)
        {
            for(int i = 0; i < requiredItem.stackSize; i++)
            {
                RemoveItem(requiredItem.data);
            }
        }

        AddItem(_itemToCraft);
        Debug.Log("合成" +  _itemToCraft.name);
        return true;


    }



    public List<InventoryItem> GetEquipmentList() => equipment;


    public ItemDataEquipment GetEquipment(EquipmentType _type)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDict)
        {
            if (item.Key.equipmentType == _type)
                equipedItem = item.Key;
        }
        return equipedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null) return;

        bool canUseFlask = Time.time > lastTimeUseFlask + currentFlask.itemCooldown;

        if (canUseFlask)
        {
            currentFlask.Effect(null);
            lastTimeUseFlask = Time.time;
        }
        else Debug.Log("冷却");

        //Debug.Log(lastTimeUseFlask + currentFlask.itemCooldown);
    }

    public void LoadData(GameData _data)
    {
        //GetItemDataBase();
        // 清空已加载的数据列表，防止重复添加
        loadedItems.Clear();
        loadedEquipment.Clear();



        foreach (KeyValuePair<string,int> pair in _data.inventory)
        {
            foreach (var item in itemDataBase)
            {
                if(item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }
        foreach(string itemId in _data.equipmentId)
        {
            foreach(var item in itemDataBase)
            {
                if(item != null && itemId == item.itemId)
                {
                    loadedEquipment.Add(item as ItemDataEquipment);
                }
            }
        }

    }

    public void SaveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();
        foreach(KeyValuePair<ItemData,InventoryItem> pair in inventoryDict)
        {
            _data.inventory.Add(pair.Key.itemId,pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData,InventoryItem> pair in stashDict)
        {
            _data.inventory.Add(pair.Key.itemId,pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemDataEquipment,InventoryItem> pair in equipmentDict)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }

    }

#if UNITY_EDITOR
    [ContextMenu("填充物品数据库")]
    private void FillUpItemDataBase() => itemDataBase = new List<ItemData>(GetItemDataBase());
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Items/物体" });

        foreach (string assetName in assetNames)
        {
            var assetpath = AssetDatabase.GUIDToAssetPath(assetName);
            Debug.Log(assetName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(assetpath);
            itemDataBase.Add(itemData);
            Debug.Log(itemData);
        }
        Debug.Log("填充");
        return itemDataBase;
    }
#endif

}
