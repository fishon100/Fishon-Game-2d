
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [Header("要傳送的另一個傳送點")]
    public Transform Tel;

    //玩家是否進入
    public bool PayerIn;

    public Transform Player;

    private void Teleport()
    {
        if (PayerIn && Input.GetKeyDown(KeyCode.Space))
        {
            Player.position = Tel.position + Vector3.up*1.5f;
        }
    }
    private void Awake()
    {
        Player = GameObject.Find("玩家").transform;
    }
    private void Update()
    {
        Teleport();
    }
    //進入碰撞器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果撞到物體.名稱 == "玩家"
        if (collision.name == "玩家")
        {
            PayerIn = true;
        }  
    }

    //離開碰撞器
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "玩家")
        {
            PayerIn = false;
        }
    }

}
