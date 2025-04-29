using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public ItemBuffer itemBuffer;
    public Transform slotRoot;

    private List<Slot> slots;

    public System.Action<ItemProperty>onSlotClick;
    // Use this for initialization
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slots = new List<Slot>();

        int slotCount = slotRoot.childCount;

        for(int i = 0; i < slotCount; i++)
        {
            var slot = slotRoot.GetChild(i).GetComponent<Slot>();

            if(i < itemBuffer.items.Count)
            {
                slot.SetItem(itemBuffer.items[i]);
            }
            else
            {
                // 아이템 없음 오시라
                slot.GetComponent<UnityEngine.UI.Button>().interactable = false;
            }

            slots.Add(slot);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickSlot(Slot slot)
    {
        if(onSlotClick != null)
        {
            onSlotClick(slot.item);
        }
    }
}
