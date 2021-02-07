using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySecondAttack : MonoBehaviour
{
    [Header("第二階段攻擊")]
    public float Attack = 50f;

    //事件   On粒子(物件 
    private void OnParticleCollision(GameObject other)
    {
        //如果(.取得有<腳本>的物件
        if (other.GetComponent<Player>())
        {
            //取得有<腳本>().物件的受傷(二階段攻擊值)
            other.GetComponent<Player>().Hurt(Attack);
        }
    }
}
