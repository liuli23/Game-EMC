using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;


    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => ++stackSize;
    public void RemoveStack() => --stackSize;

    public void AddStack(int value)
    {
        stackSize += value;
    }
    public void RemoveStack(int value)
    {
        stackSize -= value;
    }


}
