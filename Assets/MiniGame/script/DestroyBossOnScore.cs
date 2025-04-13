using UnityEngine;

public class DestroyBossOnScore : MonoBehaviour
{
    void Update()
    {
        // 获取 scoreA 的值
        int scoreA = PlayerPrefs.GetInt(GameKeys.TYPING_A, 0) 
                   + PlayerPrefs.GetInt(GameKeys.PUZZLE_A, 0);

        // 检查 scoreA 是否达到 500 分
        if (scoreA >= 500)
        {
            // 查找所有标签为 Bosstag 的 GameObject
            GameObject[] bossObjects = GameObject.FindGameObjectsWithTag("Bosstag");

            // 销毁所有找到的 GameObject
            foreach (GameObject boss in bossObjects)
            {
                Destroy(boss);
            }

            // 禁用该脚本，避免重复检查
            this.enabled = false;
        }
    }
}