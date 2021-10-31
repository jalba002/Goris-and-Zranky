using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrankLoader : MonoBehaviour, IUpdateOnSceneLoad
{
    public GameObject frank;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadFrank(Vector3 pos)
    {
        frank.transform.position = pos;
        frank.SetActive(true);
    }

    public void UpdateOnSceneLoad()
    {
        DontDestroyOnLoad(frank);
        Debug.Log("BYE FRANK");
        frank.gameObject.SetActive(false);
    }
}
