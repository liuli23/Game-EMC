using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropProfab;
    //[SerializeField] private ItemData item;//≤‚ ‘”√¿˝

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0,100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        for(int i = 0;i<possibleItemDrop; i++)
        {
            if (dropList.Count == 0) return;

            ItemData randomItem = dropList[Random.Range(0,dropList.Count-1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }

    }


    protected void DropItem(ItemData _itemData)
    {
        GameObject newdrop = Instantiate(dropProfab, transform.position,Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-3, 3), Random.Range(3, 7));

        newdrop.GetComponent<ItemObject>().SetupItem(_itemData,randomVelocity);

    }

}
