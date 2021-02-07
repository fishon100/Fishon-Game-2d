
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    [Header("魔王")]
    public GameObject Boss;
    [Header("魔王血條")]
    public GameObject BossHp;

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "玩家")
        {
            Boss.SetActive(true);
            BossHp.SetActive(true);

        }
    }
}
