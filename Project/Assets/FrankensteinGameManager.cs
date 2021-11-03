using System.Collections;
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

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        // Play music.
        // Play sounds?
        if (startMinigame)
            PlayGame();
    }

    private void PlayGame()
    {
        if (MinigameController != null) return;
        
        MinigameController = MinigameManage();
        StartCoroutine(MinigameController);
    }

    public void PauseGame()
    {
        gameMusic.Pause();
    }
    public void UnPause()
    {
        gameMusic.UnPause();
    }

    [Button("End Minigame")]
    private void EndGame()
    {
        OnGameEnd.Invoke();
    }

    private IEnumerator MinigameManage()
    {
        yield return new WaitForSeconds(1f);
        OnGameStart.Invoke();
        gameMusic.Play();
        while (gameMusic.isPlaying && !skipMusic)
        {
            //
            Debug.Log("Waiting for music to end.");
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("MUSIC ENDED!");
        OnGameEnd.Invoke();
        MinigameController = null;
    }
}
