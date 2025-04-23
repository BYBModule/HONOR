using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragItem dragItem = dropped.GetComponent<DragItem>();
        dragItem.parentAfterDrag = transform;
    }
}
