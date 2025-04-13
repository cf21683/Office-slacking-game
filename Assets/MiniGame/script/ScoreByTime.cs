using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreByTime : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText; // 用于显示当前得分的 Text 组件
    private int currentScore; // 当前得分
    private Coroutine scoreCoroutine; // 用于存储协程的引用

    private void OnEnable()
    {
        currentScore = 0; // 激活时重置当前得分
        UpdateScoreDisplay(); // 更新显示
        scoreCoroutine = StartCoroutine(AddScoreOverTime()); // 启动协程
    }

    private void OnDisable()
    {
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine); // 停用物体时停止协程
        }
    }

    private IEnumerator AddScoreOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // 等待 10 秒
            currentScore += 10; // 增加 10 分
            UpdateScoreDisplay(); // 更新显示

            // 累加到全局存储的总分 B
            int totalShooter = PlayerPrefs.GetInt(GameKeys.SHOOTER_B, 0) + 10;
            PlayerPrefs.SetInt(GameKeys.SHOOTER_B, totalShooter);
            PlayerPrefs.Save();
            TotalScoreManager.Instance?.UpdateAllScores();
        }
    }

    private void UpdateScoreDisplay()
    {
        scoreText.text = $"Score: {currentScore}"; // 更新 Text 组件显示的分数
    }
}