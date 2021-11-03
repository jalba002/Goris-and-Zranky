using UnityEngine;
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
}
