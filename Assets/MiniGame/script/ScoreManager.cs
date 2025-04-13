// ScoreManager.cs
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    private int _score = 0;
    public int Score
    {
        get => _score;
        private set
        {
            _score = value;
            // 如果有UI文本需要更新，可以在这里调用
            Debug.Log($"当前分数: {_score}");
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持跨场景存在
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        Score += amount;
    }
}