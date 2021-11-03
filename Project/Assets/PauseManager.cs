using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject MenuPause;
    public FrankensteinGameManager FrankensteinMiniGame;


    public void PauseOn()
    {
        MenuPause.SetActive(!MenuPause.activeInHierarchy);
        if (MenuPause.activeInHierarchy)
        {
            Time.timeScale = 0;
            if (FrankensteinMiniGame == null) return;
            FrankensteinMiniGame.PauseGame();
        }
        else
        {
            Time.timeScale = 1;
            if (FrankensteinMiniGame == null) return;
            FrankensteinMiniGame.UnPause();
        }     
    }    
}
