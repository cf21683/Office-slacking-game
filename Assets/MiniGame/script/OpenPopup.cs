using UnityEngine;

public class OpenPopup : MonoBehaviour
{
    public GameObject popupPrefab;
    public Vector2 spawnPosition = new Vector2(100, -100);
    public int maxOpenCount = 10; // 定义可在 Unity 中修改的最大打开次数

    private GameObject currentPopup;
    private int openCount = 0; // 新增计数器变量

    public void OnButtonClick()
    {
        if (openCount >= maxOpenCount)
        {
            return; // 如果达到最大打开次数，不执行后续操作
        }

        // 检查场景中是否存在带有 TargetPrefab 标签的预制体
        GameObject[] existingWindows = GameObject.FindGameObjectsWithTag("TargetPrefab");
        if (existingWindows.Length > 0)
        {
            return; // 如果存在，不创建新的预制体
        }

        // 创建新实例
        Canvas targetCanvas = FindObjectOfType<Canvas>();
        currentPopup = Instantiate(popupPrefab, targetCanvas.transform);

        // 设置位置并确保标签正确
        RectTransform rt = currentPopup.GetComponent<RectTransform>();
        rt.anchoredPosition = spawnPosition;
        currentPopup.tag = "TargetPrefab"; // 如果预制体未设置标签时需要这行

        openCount++; // 打开次数加1
    }
}    