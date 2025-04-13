// JigsawPiece.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class JigsawPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Vector2 CorrectPosition { get; private set; }
    public bool IsCorrectlyPlaced { get; private set; }

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private JigsawPuzzleManager manager;
    private float snapDistance;
    private Vector2 startPos;

    public void Initialize(Vector2 correctPos, float snapDist, Canvas c, JigsawPuzzleManager m)
    {
        CorrectPosition = correctPos;
        snapDistance = snapDist;
        canvas = c;
        manager = m;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsCorrectlyPlaced) return;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        startPos = rectTransform.anchoredPosition;
        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsCorrectlyPlaced) return;

        // 获取Canvas的RectTransform
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // 将屏幕坐标转换为Canvas的本地坐标
        Vector2 localMousePos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, canvas.worldCamera, out localMousePos))
        {
            // 计算相对于puzzleContainer的偏移
            RectTransform containerRect = manager.puzzleContainer.GetComponent<RectTransform>();
            Vector2 containerOffset = containerRect.anchoredPosition;

            // 修正位置计算
            Vector2 newPosition = localMousePos - containerOffset;

            // 设置碎片的位置
            rectTransform.anchoredPosition = newPosition;

            // 限制区域的x方向-524单位（保留之前的需求修改）
            Vector2 minPos = canvasRect.rect.min;
            Vector2 maxPos = canvasRect.rect.max;
            Vector2 pieceSize = rectTransform.sizeDelta;
            minPos += pieceSize / 2;
            maxPos -= pieceSize / 2;
            minPos.x -= 524;
            maxPos.x -= 524;
            rectTransform.anchoredPosition = new Vector2(
                Mathf.Clamp(rectTransform.anchoredPosition.x, minPos.x, maxPos.x),
                Mathf.Clamp(rectTransform.anchoredPosition.y, minPos.y, maxPos.y));
        }

        CheckAutoSnap();
    }

    private void CheckAutoSnap()
    {
        float distance = Vector2.Distance(rectTransform.anchoredPosition, CorrectPosition);
        if (distance <= snapDistance)
        {
            SnapToCorrectPosition();
        }
    }

    private void SnapToCorrectPosition()
    {
        rectTransform.anchoredPosition = CorrectPosition;
        IsCorrectlyPlaced = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        GetComponent<Image>().raycastTarget = false;
        manager.CheckPuzzleComplete();
        rectTransform.SetAsFirstSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsCorrectlyPlaced) return;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (Vector2.Distance(rectTransform.anchoredPosition, CorrectPosition) <= snapDistance)
        {
            SnapToCorrectPosition();
        }
        else
        {
            GetComponent<Image>().raycastTarget = true;
        }
    }

    IEnumerator SmoothReturn(Vector2 targetPos)
    {
        float duration = 0.3f;
        float timer = 0;
        Vector2 startPos = rectTransform.anchoredPosition;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, timer / duration);
            yield return null;
        }
    }
}