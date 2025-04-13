// Scripts/Minigames/TypingGame.cs
using TMPro;
using UnityEngine;

public class TypingGame : MonoBehaviour
{
    [SerializeField] private TMP_Text targetWordText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float timeLimit = 60f;

    private string[] wordPool = { "set goals", "make a schedule", "prioritize tasks", "carry out a project", "implement a plan","meet deadlines","work overtime","put in extra hours","work as a team","hold a team meeting","share ideas","improve professional skills","send an email","deal with stress","take a break","manage workload","change jobs" };
    private string currentWord;
    private int score;
    private float timer;

    void Start()
    {
        score = 0; // 每次游戏实例初始化时重置当前分数
        GenerateNewWord();
        timer = timeLimit;
        UpdateScoreDisplay(); // 更新显示
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) EndGame();

        if (Input.GetKeyDown(KeyCode.Return))
            CheckAnswer();
    }

    void GenerateNewWord()
    {
        currentWord = wordPool[Random.Range(0, wordPool.Length)];
        targetWordText.text = currentWord;
        inputField.text = "";
    }

    void CheckAnswer()
    {
        if (inputField.text.Equals(currentWord))
        {
            score += 100;
            UpdateScoreDisplay();
            GenerateNewWord();

            // 累加全局分数
            int totalTyping = PlayerPrefs.GetInt(GameKeys.TYPING_A, 0) + 100;
            PlayerPrefs.SetInt(GameKeys.TYPING_A, totalTyping);
            PlayerPrefs.Save();
            TotalScoreManager.Instance?.UpdateAllScores();
        }
    }

    void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {score}";
    }

    void EndGame()
    {
        gameObject.SetActive(false);
    }
}