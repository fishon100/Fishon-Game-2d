

using UnityEngine;
using System.Collections; //引用 協同程式


public class CameraCtrol2D : MonoBehaviour
{
    [Header("追蹤物件")]
    public Transform Target;

    [Header("追蹤速度"), Range(0, 100)]
    public float   Speed = 3.5f;

    [Header("晃動間隔"), Range(0, 1)]
    public float ShackInterval = 0.05f;
    [Header("晃動數值"), Range(0, 5)]
    public float ShackValue = 3.5f;
    [Header("晃動次數"), Range(0, 5)]
    public float ShackCount = 3;




    /// <summary>
    /// 追蹤
    /// </summary>
    private void Track()
    {
        Vector3 PosA = Target.position;     //取得玩家座標
        Vector3 PosB = transform.position;  //取得攝相機座標 transform(小寫)使用跟腳本同一元件
        PosA.z = -10;                       //攝相機 z軸改為-10

        PosB = Vector3.Lerp(PosB, PosA, 0.5f * Speed * Time.deltaTime);   //攝相機差值  //Time.deltaTime 一偵的時間
        transform.position = PosB;                                        //更新攝相機座標
    }

    private void LateUpdate()              //在Update執行完後，執行 適用追蹤系統
    {
        Track();
    }

    /// <summary>
    /// 攝相機晃動
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShackCamera()
    {
       //迴圈(初始值 i =0 ;i < 次數 ; 迭代器 i++
        for(int i =0; i< ShackCount; i++ )
        {
            //自身座標 累加 Y軸向*晃動值
            transform.position += Vector3.up * ShackValue;
            //等待延遲時間(間隔時間)
            yield return new WaitForSeconds(ShackInterval);

            transform.position -= Vector3.up * ShackValue;
            yield return new WaitForSeconds(ShackInterval);
        }
    }
       

}
