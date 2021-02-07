using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    /// <summary>
    /// 子彈攻擊力
    /// </summary>
    public float attack;

    /// <summary>
    /// 碰撞事件:兩個物件都沒有勾選IS Trigger 使用
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //如果碰撞物件有 Enemy 腳本
        if (collision.gameObject.GetComponent<Enemy>())
        {
            //對 Enemy 呼叫. Damage(攻擊力)
            collision.gameObject.GetComponent<Enemy>().Damage(attack);
        }

        //消除物件
        Destroy(gameObject);
    }
}
