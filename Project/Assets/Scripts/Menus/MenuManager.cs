using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string sceneToLoad = "Jordi";
    
    public GameObject MainMenParent;
    public GameObject ControlsParent;
    public GameObject OptionsParent;
    public GameObject CreditsParent;
    
    public Animation Animation;
   
    public void ToggleOptions()
    {
        MainMenParent.SetActive(OptionsParent.activeInHierarchy);
        OptionsParent.SetActive(!OptionsParent.activeInHierarchy);
    }

    public void ToggleControls()
    {
        MainMenParent.SetActive(ControlsParent.activeInHierarchy);
        ControlsParent.SetActive(!ControlsParent.activeInHierarchy);
    }

    public void ToggleCredits()
    {
        MainMenParent.SetActive(CreditsParent.activeInHierarchy);
        CreditsParent.SetActive(!CreditsParent.activeInHierarchy);
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

    public void Start()
    {
        Animation.Play();
    }
}
