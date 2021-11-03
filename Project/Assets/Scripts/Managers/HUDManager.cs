using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private static HUDManager _instance;
    public static HUDManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }

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

    public void ToggleFade(bool enable)
    {
        if (enable) 
        {
            FadeToBlack();
        }
        else
        {
            FadeToWhite();
        }
    }

    public float FadeToBlack()
    {
        animation.clip = fadeBlack;
        animation.Play();
        //Debug.Log("Fading to black.");
        return animation.clip.length;
    }

    public float FadeToWhite()
    {
        animation.clip = fadeWhite;
        animation.Play();
        //Debug.Log("Fading to white.");
        return animation.clip.length;
    }
}
