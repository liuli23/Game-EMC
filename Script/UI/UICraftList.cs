using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICraftList : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;


    [SerializeField] private List<ItemDataEquipment> craftEquipment;
    //[SerializeField] private List<UICraftSlot> craftSlots;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //AssingCraftSlots();
        transform.parent.GetChild(0).GetComponent<UICraftList>().SetupCraftList();
        SetupDefaultCraftWindow();
    }

    /*
    private void AssingCraftSlots()
    {
        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            craftSlots.Add(craftSlotParent.GetChild(i).GetComponent<UICraftSlot>());
        }
    }
    *///不再需要

    public void SetupCraftList()
    {
        for (int i = 0; i < craftSlotParent.childCount;i++)
        {
            //Destroy(craftSlots[i].gameObject);
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        //craftSlots = new List<UICraftSlot>();


        for(int i = 0;i < craftEquipment.Count;i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab,craftSlotParent);
            //Debug.Log("设置");
            newSlot.GetComponent<UICraftSlot>().SetupCraftSlot(craftEquipment[i]);

        }
    }



    // Update is called once per frame


    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0]!=null) 
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]); 

    }

}
