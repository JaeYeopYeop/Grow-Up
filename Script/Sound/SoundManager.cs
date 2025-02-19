using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public enum EBGM{
        BGM_Original,

    }
    public enum ESFX
    {
        SFX_Button,
        SFX_Buy,
        SFX_Clear,
        SFX_Fail,
        SFX_Grow,
        SFX_PauseIn,
        SFX_PauseOut,
        SFX_JellySell,
        SFX_JellyTouch,
        SFX_Unlock,
    }

    [SerializeField] private AudioClip[] bgms;
    [SerializeField] private AudioClip[] sfxs;

    [SerializeField] private AudioSource audioBgm;
    [SerializeField] private AudioSource audioSfx;


    private void Awake()
    {
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBgm(EBGM bgm)
    {
        audioBgm.clip=bgms[(int)bgm];
        audioBgm.Play();
    }
    public void StopBgm()
    {
        audioBgm.Stop(); 
    }

    public void PlaySfx(ESFX sfx)
    {
        audioSfx.PlayOneShot(sfxs[(int)sfx]);
    }

    public void MuteOnSfx()
    {
        audioSfx.mute = true;
    }
    public void MuteOffSfx()
    {
        audioSfx.mute = false;
    }


    public void SetBgmVolume(float value)
    {
        audioBgm.volume=value;
    }
    public void SetSfxVolume(float value)
    {
        audioSfx.volume=value;
    }
}
