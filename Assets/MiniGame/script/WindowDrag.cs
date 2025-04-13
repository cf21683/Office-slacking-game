using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private RectTransform dragRectTransform;
    private Canvas canvas;

    void Awake()
    {
        dragRectTransform = GetComponentInParent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransform.SetAsLastSibling(); // 确保窗口显示在最前面
    }
}