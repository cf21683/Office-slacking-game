// 修改后的CloseWindow.cs
using UnityEngine;

public class CloseWindow : MonoBehaviour
{
    public void Close()
    {
        // 找到预制体根对象（当前对象的父级的父级，根据实际层级调整）
        Destroy(transform.parent.parent.parent.gameObject);
        
        // 或者更安全的通用写法：
        // 找到最近的带有Canvas组件的父对象（假设你的弹窗预制体根对象有Canvas组件）
        // Transform root = GetComponentInParent<Canvas>().transform;
        // Destroy(root.gameObject);
    }
}