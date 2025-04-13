// AutoScoreIncrementer.cs
using UnityEngine;
using System.Collections;

public class AutoScoreIncrementer : MonoBehaviour
{
    private Coroutine _scoreCoroutine;

    private void OnEnable()
    {
        // 启动计分协程
        _scoreCoroutine = StartCoroutine(AddScoreRoutine());
    }

    private void OnDisable()
    {
        // 停止计分协程
        if (_scoreCoroutine != null)
        {
            StopCoroutine(_scoreCoroutine);
        }
    }

    private IEnumerator AddScoreRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            ScoreManager.Instance.AddScore(10);
        }
    }
}