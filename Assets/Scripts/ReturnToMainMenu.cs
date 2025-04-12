using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public float delay = 15f; // 延迟时间（秒）

    void Start()
    {
        // 启动延迟回到主菜单
        Invoke("ReturnToMenu", delay);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
