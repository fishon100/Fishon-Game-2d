using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;

    public void Awake()
    {
        //角色.找到其中擁有的腳本<Player>():
        player = FindObjectOfType<Player>();
    }

    /// <summary>
    /// 設置鈕按下去遊戲會暫停，玩家會暫停
    /// </summary>
    public void PuaseGame()
    {
        //時間.時間快慢 = 暫停    //  *2 = 加快  *1 = 恢復繼續  *0.5f = 慢速
        Time.timeScale = 0;

        //玩家.啟動 = 取消
        player.enabled = false;

    }

    public void ContinuGame()
    {
        //時間.時間快慢 = 繼續    //  *2 = 加快  *1 = 恢復繼續  *0.5f = 慢速
        Time.timeScale = 1;

        //玩家.啟動 = 開啟
        player.enabled = true;

    }
}
