using UnityEngine;
using TMPro;
using System;

public class TotalScoreManager : MonoBehaviour
{
    public static TotalScoreManager Instance;

    [Header("UI References")]
    // 确保三个UI组件都存在声明
    public TMP_Text scoreAText;
    public TMP_Text scoreBText;

    private int slackScore;
    private int workScore;
    public int panlty;

    public int SlackScore{get{return slackScore;}}
    public int WorkScore{get{return workScore;}}

    private int scoreA;

    private int scoreB;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 每次启动游戏时重置分数
            ResetAllScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        UpdateAllScores();
    }

    // 初始化分数（每次游戏启动时调用）
    public void UpdateAllScores()
    {
        // 直接读取全局累计值
        scoreA = PlayerPrefs.GetInt(GameKeys.TYPING_A, 0) 
                   + PlayerPrefs.GetInt(GameKeys.PUZZLE_A, 0);
        workScore = scoreA;
        scoreB = PlayerPrefs.GetInt(GameKeys.SHOOTER_B, 0);
        slackScore = scoreB;
        
        scoreAText.text = $"WorkScore: {scoreA}";
        scoreBText.text = $"SlackScore: {scoreB}";
    }

    // 重置分数
    public void ResetAllScores()
    {
        PlayerPrefs.SetInt(GameKeys.TYPING_A, 0);
        PlayerPrefs.SetInt(GameKeys.PUZZLE_A, 0);
        PlayerPrefs.SetInt(GameKeys.SHOOTER_B, 0);
        PlayerPrefs.Save(); // 确保修改保存到PlayerPrefs
        UpdateAllScores();
    }
    public void DecreaseSlackScore(){
        scoreB -= panlty;

        scoreB = Mathf.Max(0,scoreB);
        Debug.Log(scoreB);
    }
}

// GameKeys.cs
public static class GameKeys
{
    // A类得分（拼图和打字）
    public const string PUZZLE_A = "ScoreA_Puzzle";
    public const string TYPING_A = "ScoreA_Typing";
    
    // B类得分（射击）
    public const string SHOOTER_B = "ScoreB_Shooter";
}