using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioCtrl Introduction;//prompts
    [SerializeField] private AudioCtrl prompts;
    [SerializeField] private AudioCtrl soundEffect;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource btnSE;
    public void SetIntroductionAudio(int index)
    {
        Introduction.PlaySound(index);
    }

    public void SetPromptsAudio(int index)
    {
        prompts.PlaySound(index);
    }

    public void ButtonClickSound()
    {
        btnSE.Play();
    }
}
