using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    private Image image;
    private RectTransform rect;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }
    
    // 각 인터페이스를 상속받을 경우 해당 메소드를 정의해야 함
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.yellow; // 아이템 슬롯의 색상을 노란색으로 변경
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white; // 아이템 슬롯의 색상을 하얀색으로 변경
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
            // 드래그하고 있는 대상의 부모를 현재 오브젝트로 설정, 위치를 현재 오브젝트 위치와 동일하게 설정정
        }
    }
}
