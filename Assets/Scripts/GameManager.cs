using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("计时设置")]
    public float totalTime = 300f; 
    private float remainingTime;

    [Header("分数追踪")]
    public int workScore = 0;
    public int slackScore = 0;
    public bool hiddenEndingUnlocked = false;


    private bool endingTriggered = false;

    void Start()
    {
        remainingTime = totalTime;
    }

    void Update()
    {
        if (endingTriggered) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            TriggerEnding();
        }
    }

    public void UnlockHiddenEnding()
    {
        hiddenEndingUnlocked = true;
    }

    void TriggerEnding()
    {
        endingTriggered = true;

        if (hiddenEndingUnlocked)
        {
            Debug.Log("触发隐藏结局");
            SceneManager.LoadSceneAsync(4);
        }
        else if (workScore > slackScore)
        {
            Debug.Log("触发工作结局");
            SceneManager.LoadSceneAsync(2);
        }
        else
        {
            Debug.Log("触发摸鱼结局");
            SceneManager.LoadSceneAsync(3);
        }
    }
}