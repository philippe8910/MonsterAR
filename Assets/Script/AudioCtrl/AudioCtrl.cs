using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCtrl : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource;
    [Header("¤¶²Ð»y­µ")]
    [SerializeField] public AudioClip[] audioClips;
    public void PlaySound(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
