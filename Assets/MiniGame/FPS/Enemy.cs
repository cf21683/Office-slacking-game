using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image), typeof(CanvasGroup))]
public class Enemy : MonoBehaviour, IPointerDownHandler
{
    public int scoreValue = 10;
    private GameManager1 gameManager;

    void Start()
    {
        gameManager = GameManager1.Instance;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!gameManager.IsGameOver)
        {
            gameManager.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}