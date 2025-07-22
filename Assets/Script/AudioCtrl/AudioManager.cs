using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioCtrl Introduction;//prompts
    [SerializeField] public AudioCtrl prompts;
    [SerializeField] public AudioCtrl soundEffect;
    [SerializeField] public AudioSource bgm;
    [SerializeField] public AudioSource btnSE;
    [SerializeField] public AudioSource hpSE;
    [SerializeField] public AudioSource cardSE;
    public void SetIntroductionAudio(int index)
    {
        Introduction.PlaySound(index);
    }

    public void SetPromptsAudio(int index)
    {
        prompts.PlaySound(index);
    }

    public void SetSEAudio(int index)
    {
        soundEffect.PlaySound(index);
    }

    public void ButtonClickSound()
    {
        btnSE.Play();
    }

    public void HPSound()
    {
        hpSE.Play();
    }

    public void cardSound()
    {
        cardSE.Play();
    }
}


