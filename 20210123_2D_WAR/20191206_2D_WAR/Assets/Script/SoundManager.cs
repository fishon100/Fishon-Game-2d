using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;  //引用 音效API

public class SoundManager : MonoBehaviour
{
    [Header("音效管理")]
    public AudioMixer Mix;

    /// <summary>
    /// 背景音量
    /// </summary>
    /// <param 值="v"></param>
    public void VolumeBGM(float v)
    {
        //音效管理名稱.設置浮點數("曝光名稱" ，值)
        Mix.SetFloat("VolumeBGM", v);
    }

    /// <summary>
    /// 音效音量
    /// </summary>
    /// <param 值="v"></param>
    public void VolumeSFX(float v)
    {
        //音效管理名稱.設置浮點數("曝光名稱" ，值)
        Mix.SetFloat("VolumeSFX", v);
    }
}
