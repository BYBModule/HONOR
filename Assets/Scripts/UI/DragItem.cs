using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Timeline;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;      
    private Transform previousParent;  
    private RectTransform rect;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = FindAnyObjectByType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent; // 드레그 직전에 소속되어 있던 부모 Transform 정보 저장

        transform.SetParent(canvas); // 부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling(); // 가장 앞에 보이도록 마지막 자식으로 설정

        canvasGroup.alpha = 0.6f; // 드래그 가능한 오브젝트가 하나가 아닌 자식들을 가지고 있을 수도 있기 때문에
        canvasGroup.blocksRaycasts = false; // 알파값을 설정하고, 광선 충돌처리가 되지 않도록 한다
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position; // UI가 마우스를 쫓아다니도록 설정
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponent<RectTransform>().position;
            // 마지막에 소속되어있었던 previousParent의 자식으로 설정, 해당 위치로 설정
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        // 알파값을 1로 설정하고, 충돌처리가 되도록 한다.
    }
    // 각 인터페이스를 상속받을 경우 해당 메소드를 정의해야 함함


    // public Image image;
    // [HideInInspector] public Transform parentAfterDrag;
    // public void OnBeginDrag(PointerEventData eventData)
    // {
    //     Debug.Log("Begin Darg");
    //     parentAfterDrag = transform.parent;
    //     transform.SetParent(transform.root);
    //     transform.SetAsLastSibling();
    //     image.raycastTarget = false; 
    // }    

    // public void OnDrag(PointerEventData eventData)
    // {
    //     Debug.Log("Dragging");
    //     transform.position = Input.mousePosition;
    // }

    // public void OnEndDrag(PointerEventData eventData)
    // {
    //     Debug.Log("End Drag");
    //     transform.SetParent(parentAfterDrag);
    //     image.raycastTarget = true;
    // }


}
