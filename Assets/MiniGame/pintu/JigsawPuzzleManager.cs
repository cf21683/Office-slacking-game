// JigsawPuzzleManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// 定义一个委托类型，用于拼图完成事件
public delegate void PuzzleCompletedEventHandler();

public class JigsawPuzzleManager : MonoBehaviour
{
    [Header("Settings")]
    public Texture2D[] sourceTextures;
    public int gridSize = 3;
    public float pieceSpacing = 10f;
    public float snapDistance = 50f;
    public float scrambleRange = 800f;

    [Header("References")]
    public Canvas canvas;
    public Image referenceImage;
    public GameObject piecePrefab;
    public GameObject puzzleContainer;
    public Text successText;

    [Header("Scoring")]
    [SerializeField] private TMP_Text scoreText;
    private int score;

    private List<JigsawPiece> pieces = new List<JigsawPiece>();
    private bool isCompleted;


    private void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {score}";
    }

    void Start()
    {
        score = 0; // 当前局分数归零
        UpdateScoreDisplay();
        successText.gameObject.SetActive(false);
        InitializePuzzleWithRandomImage();
        ScramblePieces();
    }

    void InitializePuzzleWithRandomImage()
    {
        if (sourceTextures.Length == 0)
        {
            Debug.LogError("No source textures assigned!");
            return;
        }
        Texture2D selectedTexture = sourceTextures[Random.Range(0, sourceTextures.Length)];

        ClearPieces();

        referenceImage.sprite = Sprite.Create(selectedTexture,
            new Rect(0, 0, selectedTexture.width, selectedTexture.height),
            new Vector2(0.5f, 0.5f));
        referenceImage.color = new Color(1, 1, 1, 0.5f);

        CreatePieces(selectedTexture);
    }

    void ClearPieces()
    {
        foreach (JigsawPiece piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();
    }

    void CreatePieces(Texture2D texture)
    {
        int pieceSize = texture.width / gridSize;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject newPiece = Instantiate(piecePrefab, puzzleContainer.transform);
                Image img = newPiece.GetComponent<Image>();
                RectTransform rt = newPiece.GetComponent<RectTransform>();

                Sprite pieceSprite = Sprite.Create(texture,
                    new Rect(x * pieceSize, y * pieceSize, pieceSize, pieceSize),
                    new Vector2(0.5f, 0.5f));

                img.sprite = pieceSprite;
                rt.sizeDelta = new Vector2(pieceSize, pieceSize);

                Vector2 correctPos = CalculateCorrectPosition(x, y, pieceSize);

                JigsawPiece jigsawPiece = newPiece.GetComponent<JigsawPiece>();
                jigsawPiece.Initialize(correctPos, snapDistance, canvas, this);
                pieces.Add(jigsawPiece);
            }
        }
    }

    Vector2 CalculateCorrectPosition(int x, int y, int pieceSize)
    {
        float totalWidth = gridSize * (pieceSize + pieceSpacing) - pieceSpacing;
        float startX = -totalWidth / 2f + pieceSize / 2f;
        float startY = -totalWidth / 2f + pieceSize / 2f;

        float posX = startX + x * (pieceSize + pieceSpacing);
        float posY = startY + y * (pieceSize + pieceSpacing);

        return new Vector2(posX, posY);
    }

    void ScramblePieces()
    {
        foreach (JigsawPiece piece in pieces)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(-scrambleRange, scrambleRange),
                Random.Range(-scrambleRange, scrambleRange));

            piece.GetComponent<RectTransform>().anchoredPosition = randomPos;
        }
    }

    public void CheckPuzzleComplete()
    {
        foreach (JigsawPiece piece in pieces)
        {
            if (!piece.IsCorrectlyPlaced) return;
        }
        if (!isCompleted)
        {
            isCompleted = true;
            score += 500;
            UpdateScoreDisplay();


            // 累加全局分数
            int totalPuzzle = PlayerPrefs.GetInt(GameKeys.PUZZLE_A, 0) + 500;
            PlayerPrefs.SetInt(GameKeys.PUZZLE_A, totalPuzzle);
            PlayerPrefs.Save();
            TotalScoreManager.Instance?.UpdateAllScores();

            // 显示成功文本
            successText.gameObject.SetActive(true);
            StartCoroutine(ScaleTextAnimation());

            // 延迟5秒后销毁对象
            StartCoroutine(DestroyPuzzleAfterDelay(5f));


        }
    }

    IEnumerator DestroyPuzzleAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(transform.parent.parent.gameObject);
    }

    IEnumerator ScaleTextAnimation()
    {
        float duration = 1f;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            successText.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / duration);
            yield return null;
        }
    }
}