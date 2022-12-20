using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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


    private AudioSource myAudio = null;

    public AudioClip[] sound;

    public AudioSource[] source;

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void PlayerAttackSound(int num)
    {
        myAudio.PlayOneShot(sound[num]);
    }

    public void ButtonClick()
    {
        source[0].Play();
    }
}
