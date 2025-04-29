using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public Transform rootSlot;
    public Store store;

    private List<Slot> slots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slots = new List<Slot>();

        int slotCount = rootSlot.childCount;

        for(int i = 0; i < slotCount; i++)
        {
            var slot = rootSlot.GetChild(i).GetComponent<Slot>();

            slots.Add(slot);
        }
        store.onSlotClick += BuyItem;
        // += 기존에 연결돼있던 함수를 파괴하지 않고 쌓는 것
    }

    void BuyItem(ItemProperty item)
    {
        var emptySlot = slots.Find(t => {return t.item == null || t.item.name == string.Empty; });

        if(emptySlot != null)
        {
            emptySlot.SetItem(item);
        } 
    }
}
