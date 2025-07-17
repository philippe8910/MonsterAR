using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    [Header("介紹語音")]
    [SerializeField] public AudioClip[] audioClips;
    public void PlaySound(int index)
    {

        audioSource.clip = audioClips[index];
        audioSource.Play();
        Debug.Log($"播放音效為：{audioClips[index].name}");
    }
}
