using UnityEngine;

public class DestroyPrefabOnClick : MonoBehaviour
{
    public void DestroyTargetPrefabs()
    {
        // 查找所有带有 "TargetPrefab" 标签的游戏对象
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag("TargetPrefab");
        
        foreach (GameObject prefab in prefabs)
        {
            Destroy(prefab);
        }
        
        Debug.Log($"已销毁 {prefabs.Length} 个预制体实例。");
    }
}