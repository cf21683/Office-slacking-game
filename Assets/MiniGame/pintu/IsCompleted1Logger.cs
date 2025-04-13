using UnityEngine;

public class IsCompleted1Logger : MonoBehaviour
{
    public string prefabName; // 预制体实例的名称
    private JigsawPuzzleManager1 yourScript;

    void Start()
    {
        // 开始协程来持续查找游戏对象
        StartCoroutine(FindPrefabInstance());
    }

    System.Collections.IEnumerator FindPrefabInstance()
    {
        while (true)
        {
            // 查找带有指定名称的游戏对象
            GameObject prefabInstance = GameObject.Find(prefabName);
            if (prefabInstance != null)
            {
                // 查找预制体中的 Canvas 物体
                GameObject canvas = prefabInstance.GetComponentInChildren<Canvas>(true)?.gameObject;
                if (canvas != null)
                {
                    // 查找 Canvas 中的 PuzzleManager 物体
                    GameObject puzzleManager = canvas.transform.Find("PuzzleManager")?.gameObject;
                    if (puzzleManager != null)
                    {
                        // 获取脚本组件
                        yourScript = puzzleManager.GetComponent<JigsawPuzzleManager1>();
                        if (yourScript != null)
                        {
                            // 开始持续检测 isCompleted1 的值
                            yield return StartCoroutine(CheckIsCompleted1());
                            // 检测完成后停止协程
                            yield break;
                        }
                    }
                }
            }
            // 等待一段时间后再次查找
            yield return new WaitForSeconds(0.1f);
        }
    }

    System.Collections.IEnumerator CheckIsCompleted1()
    {
        while (!yourScript.isCompleted1)
        {
            // 等待一段时间后再次检测
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("isCompleted1 的值为 true");
    }
}

// 包含isCompleted1变量的脚本
public class YourScript : MonoBehaviour
{
    public bool isCompleted1;
}