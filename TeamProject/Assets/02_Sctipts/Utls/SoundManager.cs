using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    #region SingleTon

    private static SoundManager _instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("SoundManager").AddComponent<SoundManager>();
                }
            }
            return _instance;
        }
    }

    #endregion


    public List<AudioSource> vfxAudioSource = new List<AudioSource>();
    public AudioSource bgmAudioSource = null;

    [Space(50)]
    public Slider bgmSlider = null;
    public Slider vfxSlider = null;

    public float bgmVol = 1f;
    public float vfxVol = 1f;


    private void Start()
    {
        bgmVol = PlayerPrefs.GetFloat(ConstantManager.BACK_VOL, 1f);
        bgmSlider.value = bgmVol;
        bgmAudioSource.volume = bgmSlider.value;

        vfxVol = PlayerPrefs.GetFloat(ConstantManager.VFX_VOL, 1f);
        vfxSlider.value = vfxVol;
    }
    public void SoundAudio(int _num)
    {
        vfxAudioSource[_num].volume = vfxVol;

        vfxAudioSource[_num].Play();
    }

    public void BGMSlider()
    {
        bgmAudioSource.volume = bgmSlider.value;

        bgmVol = bgmSlider.value;
        PlayerPrefs.SetFloat(ConstantManager.BACK_VOL, bgmVol);
    }

    public void VFXSlider()
    {
        //vfxAudioSource.volume = vfxSlider.value;
        vfxVol = vfxSlider.value;
        PlayerPrefs.SetFloat(ConstantManager.VFX_VOL, vfxVol);
    }
}
