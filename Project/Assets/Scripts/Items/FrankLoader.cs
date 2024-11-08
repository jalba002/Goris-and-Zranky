﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrankLoader : MonoBehaviour, IUpdateOnSceneLoad
{
    public GameObject frank;
    public AnimationClip wakeUpAnim;

    public static FrankLoader Instance;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Instance.Cleanup();
        }
        Instance = this;
        this.transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadFrank(Vector3 pos)
    {
        frank.transform.position = pos;
        frank.SetActive(true);
        var anim = frank.AddComponent<Animation>();
        anim.AddClip(wakeUpAnim, wakeUpAnim.name);
        anim.clip = wakeUpAnim;
        anim.Play();
    }

    public void UpdateOnSceneLoad()
    {
        DontDestroyOnLoad(frank);
        //Debug.Log("BYE FRANK");
        var list = frank.GetComponentsInChildren<ParticleSystem>();
        foreach (var item in list)
        {
            Destroy(item.gameObject);
        }
        frank.gameObject.SetActive(false);
    }

    private void Cleanup()
    {
        Destroy(frank);        
    }
}
