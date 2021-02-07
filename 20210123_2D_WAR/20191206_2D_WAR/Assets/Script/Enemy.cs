
using UnityEngine;
using UnityEngine.UI;     //引用 UI API
using UnityEngine.Events; //引用 事件
using System.Collections; //引用 協同程式

//需第一次或重新套用腳本
//要求添加元件(類型(元件)，類型(元件)，.....
[RequireComponent(typeof(AudioSource),typeof(Rigidbody2D),typeof(CapsuleCollider2D))]
public class Enemy : MonoBehaviour
{
    [Header("移動速度"), Range(1, 1000)]
    public float speed = 10.5f;

    [Header("攻擊範圍"), Range(1, 1000)]
    public float RangeAtk = 10.5f;
    [Header("攻擊範圍位移")]
    public Vector3 attackoffset;
    [Header("攻擊範圍大小")]
    public Vector3 attackSize;

    [Header("攻擊力"), Range(1, 1000)]
    public float attack = 10;
    [Header("攻擊冷卻"), Range(1, 10)]
    public float attackCD = 3.5f;
    [Header("延遲玩家被攻擊扣寫的時間"), Range(1, 10)]
    public float attackDelayTime = 0.6f;


    [Header("攻擊音效")]
    public AudioClip AtkV;

    [Header("血量"), Range(1, 1000)]
    public float hp = 1000;

    [Header("血量圖片")]
    public Image Imahp ;
    [Header("血量文字")]
    public Text Texhp;

    [Header("死亡事件")]
    public UnityEvent onDead;

    private AudioSource aud;
    private Rigidbody2D rig;
    private Animator ani;

    private Player Player;

    private CameraCtrol2D Cam;

    /// <summary>
    /// 計時器
    /// </summary>
    private float timer;


    //最大血量值
    private float Maxhp;

    //粒子控制器
    private ParticleSystem PS;
    private bool isSecond;



    private void Start()
    {
        aud = GetComponent<AudioSource>();
        rig = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

        //透過類行尋找物件<類型>()    //透過 FindObjectOfType 追尋掛有這個角本 <腳本名稱>() 的物件  //只能適用一個物件
        Player = FindObjectOfType<Player>();
       Cam = FindObjectOfType<CameraCtrol2D>();

        //粒子名稱 = 物件.找到("Hierarchy粒子名稱").取得<粒子控制器>()
        PS = GameObject.Find("骷髏二階攻擊").GetComponent<ParticleSystem>();

        //最大血量 = hp
        Maxhp = hp;
        
    }

    private void Update()
    {
        //如果 死亡動畫被勾選 則 跳出以下程式的執行  
        if (ani.GetBool("開始死亡"))return;         //適用多個擁有此腳本的角色，可避免互相影響

        //呼叫
        Move();
    }

    //在UNITY繪製圖示
    private void OnDrawGizmos()
    {
        //圖示.顏色
        Gizmos.color =new Color(0,15,15,0.5f);
        //圖示.形狀方形(物體座標，x軸向* 攻擊範圍位移的x + y軸向* 攻擊範圍位移的y , 攻擊範圍大小 )
        Gizmos.DrawCube(transform.position + transform.right* attackoffset.x + transform.up *attackoffset.y, attackSize);

        //圖示.顏色
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        //圖示.形狀方形(物體座標，攻擊範圍 )
        Gizmos.DrawSphere(transform.position , RangeAtk);

    }

    /// <summary>
    /// 移動,追蹤玩家
    /// </summary>
    public void Move()
    {      
        //動畫狀態信息.信息 = 動畫控制器.取得當前動畫狀態信息(layer動畫圖層 為0)
        AnimatorStateInfo info = ani.GetCurrentAnimatorStateInfo(0);
        //如果動畫執行在 (信息.名稱("骷髏-攻擊") 或 信息.名稱("骷髏-受傷")) 則 跳出以下走動程式(不走動)
        if (info.IsName("骷髏-攻擊") || info.IsName("骷髏-受傷")) return;

        /** 方法一
        //如果自身座標X 大於> 玩家座標X
        if (transform.position.x >Player.transform.position.x)
        {
            //座標.水平軸 = (0.180.0)
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        */

        //方法二  三元運算值
        //布林值? 是的結果 : 否的結果
        float y =transform.position.x > Player.transform.position.x ? 180 : 0;
        transform.eulerAngles = new Vector3(0, y, 0);
              
        //距離 = 二維.距離(A座標，B座標)
        float dis = Vector2.Distance(transform.position, Player.transform.position);  //感應雙方距離

        //如果(距離>攻擊範圍)
        if (dis > RangeAtk)
        {
            //往前走
            //缸體.移動座標(自身座標 + 移動軸向 * 一禎 * 速度)   //軸向名稱 紅:right  綠:up   藍:front
            rig.MovePosition(transform.position + transform.right * Time.deltaTime * speed);
        }
        else
        {
            //呼叫攻擊
            Hit();
        }

        //動畫.布林值("動畫開關".鋼體.加速度.浮點數值 > 0 )  
        ani.SetBool("開始走", rig.velocity.magnitude > 0);    //當 加速度數值大於0 開始走路

    }
    
    /// <summary>
    /// 攻擊
    /// </summary>
    public void Hit()
    {
        //鋼體.加速度 = 停止
        rig.velocity = Vector3.zero;
        

        if (timer < attackCD)            //如果( 計時器 < 攻擊冷卻)
        {
            timer += Time.deltaTime;     //累加計時器
        }
        else                             //否則 計時器 >= 攻擊冷卻
        {
            ani.SetTrigger("攻擊觸發");  //攻擊動畫 
            timer = 0;                   //計時器歸0

            //啟動協同程式(協同程式名稱)        
            StartCoroutine(DelaySendDamage());
        }
    
            
    }

    /// <summary>
    /// 延遲傳送玩家傷害時間
    /// </summary>   
    public IEnumerator DelaySendDamage()   //協同程式IEnumerator
    {
        //等待延遲時間
        yield return new WaitForSeconds(attackDelayTime);

        //碰撞     攻擊hit =      物理.盒型覆蓋區域(物體座標，x軸向* 攻擊範圍位移的x + y軸向* 攻擊範圍位移的y , 攻擊範圍大小 , 旋轉0 , layer圖層10)
        Collider2D hit = Physics2D.OverlapBox(transform.position + transform.right * attackoffset.x + transform.up * attackoffset.y, attackSize, 0, 1 << 10);

        //如果(打到)存在玩家腳本.受傷(攻擊力)
        if (hit) Player.Hurt(attack);

        //啟動協同程式(腳本裡的.協同程式名稱)        
        StartCoroutine(Cam.ShackCamera());

        //如果是第二階段 就播放特效
        if (isSecond) PS.Play();
       

    }


    /// <summary>
    /// 傷害
    /// </summary>
    /// <param name="damage">接收傷害直</param>
    public void Damage(float damage)
    {
        hp -= damage; //遞減
        ani.SetTrigger("受傷觸發");      //受傷動畫
        Texhp.text = hp.ToString();      //血量文字.文字 = 血量.轉字串();
        Imahp.fillAmount = hp / Maxhp;   //血量圖片.填滿長度 = 目前血量/最大血量

        //如果(血量 小於等於 最大血量的7成) 攻擊範圍 = 20 
        if (hp <= Maxhp * 0.7)
        {
            isSecond = true;      //進入第二檔攻擊
            RangeAtk = 20;        //二階攻擊力
        }              

        //如果(hp <= 0 呼叫Dead          //呼叫方法 名稱();
        if (hp <= 0) Dead();
        

    }

    

    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        //觸發 死亡 事件
        onDead.Invoke();

        hp = 0;
        Texhp.text = 0.ToString();
        ani.SetBool("開始死亡", true);

        //取得元件<膠囊碰撞汽>().啟動 = 關閉
        GetComponent<CapsuleCollider2D>().enabled = false;

        
        rig.Sleep();                                             //鋼體.沉睡不使用()
        rig.constraints = RigidbodyConstraints2D.FreezeAll;      //鋼體.約束 = 約束.凍結全部

        
    }


}
