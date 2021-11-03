using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject MenuPause;
    public FrankensteinGameManager FrankensteinMiniGame;

    public void TogglePause()
    {
        MenuPause.SetActive(!MenuPause.activeInHierarchy);
        if (MenuPause.activeInHierarchy)
        {
            Time.timeScale = 0;

            if (FrankensteinMiniGame != null)
                FrankensteinMiniGame.PauseGame();
        }
        else
        {
            Time.timeScale = 1;

            if (FrankensteinMiniGame != null)
                FrankensteinMiniGame.UnPause();
        }
    }

    public void PauseOn()
    {
        if (Time.timeScale < 1f) return;
        
        MenuPause.SetActive(true);

        Time.timeScale = 0f;

        if (FrankensteinMiniGame != null)
            FrankensteinMiniGame.PauseGame();
    }
}