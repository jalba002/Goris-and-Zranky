using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class FrankensteinGameManager : MonoBehaviour
{
    //public bool startGame = false;
    public bool startMinigame = false;
    public AudioSource gameMusic;
    public bool skipMusic = false;
    
    public UnityEvent OnGameStart = new UnityEvent();
    public UnityEvent OnGameEnd = new UnityEvent();

    // private void Start()
    // {
    //     if (startGame)
    //     {
    //         // This will start playing a cinematic.
    //         StartGame();
    //     }
    // }

    private IEnumerator MinigameController;

    public void Awake()
    {
        
    }

    public void StartGame()
    {
        // Play music.
        // Play sounds?
        if (MinigameController != null) return;
        if (!startMinigame) return;
        OnGameStart.Invoke();
        PlayGame();
    }

    private void PlayGame()
    {
        MinigameController = MinigameManage();
        StartCoroutine(MinigameController);
    }

    [Button("End Minigame")]
    private void EndGame()
    {
        OnGameEnd.Invoke();
    }

    private IEnumerator MinigameManage()
    {
        gameMusic.Play();
        while (gameMusic.isPlaying && !skipMusic)
        {
            //
            Debug.Log("Waiting for music to end.");
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("MUSIC ENDED!");
        OnGameEnd.Invoke();
    }
}
