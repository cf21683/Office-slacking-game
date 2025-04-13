using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 Instance;

    [Header("Game Settings")]
    public float gameTime = 60f;
    public int targetScore = 1000;

    [Header("References")]
    public RectTransform gameCanvas;
    public GameObject enemyPrefab;
    public Transform enemyContainer;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public TMP_Text resultText;

    private int currentScore;
    private float remainingTime;
    private Vector2 canvasSize;
    public bool IsGameOver { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentScore = 0; // 当前局分数归零
        canvasSize = gameCanvas.rect.size;
        remainingTime = gameTime;
        StartCoroutine(SpawnEnemies());
        UpdateUI();
    }

    IEnumerator SpawnEnemies()
    {
        float spawnWidth = canvasSize.x * 0.25f;
        float spawnHeight = canvasSize.y * 0.25f;

        while (!IsGameOver)
        {
            RectTransform enemyRect = enemyPrefab.GetComponent<RectTransform>();
            float enemyWidth = enemyRect.rect.width * 0.5f;
            float enemyHeight = enemyRect.rect.height * 0.5f;

            Vector2 spawnPos = new Vector2(
                Random.Range(-spawnWidth + enemyWidth, spawnWidth - enemyWidth),
                Random.Range(-spawnHeight + enemyHeight, spawnHeight - enemyHeight)
            );

            GameObject newEnemy = Instantiate(enemyPrefab, enemyContainer);
            newEnemy.GetComponent<RectTransform>().anchoredPosition = spawnPos;

            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }
    }

    void Update()
    {
        if (!IsGameOver)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                EndGame();
            }
            UpdateUI();
        }
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateUI();

        // 累加全局分数
        int totalShooter = PlayerPrefs.GetInt(GameKeys.SHOOTER_B, 0) + amount;
        PlayerPrefs.SetInt(GameKeys.SHOOTER_B, totalShooter);
        PlayerPrefs.Save();
        TotalScoreManager.Instance?.UpdateAllScores();
    }

    void UpdateUI()
    {
        scoreText.text = $"Score: {currentScore}/{targetScore}";
        timerText.text = $"Time: {Mathf.CeilToInt(remainingTime)}s";
    }

    void EndGame()
    {
        IsGameOver = true;
    }
}