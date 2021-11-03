using System;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;
    public Animation animation;
    
    public AnimationClip fadeBlack;
    public AnimationClip fadeWhite;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        animation = GetComponent<Animation>();
    }

    public float FadeToBlack()
    {
        animation.clip = fadeBlack;
        animation.Play();
        return animation.clip.length;
    }

    public float FadeToWhite()
    {
        animation.clip = fadeWhite;
        animation.Play();
        return animation.clip.length;
    }
}
