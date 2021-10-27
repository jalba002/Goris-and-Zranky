using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public void SoundPlayer(Object clip)
    {
        audioSource.clip = clip as AudioClip;
        audioSource.Play();
        Debug.Log("Playing Jump");
    }
}
