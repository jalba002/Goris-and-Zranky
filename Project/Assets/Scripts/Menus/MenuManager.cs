﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string sceneToLoad = "Jordi";
    
    public GameObject MainMenParent;
    public GameObject OptionsParent;
    public GameObject CreditsParent;
    
    public void ToggleCustomization()
    {
        MainMenParent.SetActive(CreditsParent.activeInHierarchy);
        CreditsParent.SetActive(!CreditsParent.activeInHierarchy);
    }
    
    public void ToggleOptions()
    {
        MainMenParent.SetActive(OptionsParent.activeInHierarchy);
        OptionsParent.SetActive(!OptionsParent.activeInHierarchy);
    }
        
    public void LoadGame()
    {
        // Load the game dumbass.
        //Debug.Log("Trying to load the game but it isn't programmed!");
        SceneManager.LoadScene(sceneToLoad);
    }
    
    public void ExitApplication()
    {
        if (Application.isPlaying)
        {
            Application.Quit();
        }
    }
}
