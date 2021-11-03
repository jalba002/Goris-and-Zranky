using System;
using System.Collections;
using System.Linq;
using Interfaces;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class SceneLoadAsyncWithFade : MonoBehaviour
{
    public string sceneToLoad = "03_C_Final";

    private IEnumerator Coroutine;

    public void Awake()
    {
        //this.transform.parent = null;
        //DontDestroyOnLoad(this.gameObject);
    }

    [Button("Trigger Load")]
    public void LoadScene()
    {
        // Play video animation and while that, load the scene async...
        if (Coroutine != null) return;

        Coroutine = SceneLoadAsyncWithVideo();
        StartCoroutine(Coroutine);
    }


    private void UpdateAllNeeded()
    {
        var list = FindObjectsOfType<MonoBehaviour>().OfType<IUpdateOnSceneLoad>();
        foreach (var item in list)
        {
            item.UpdateOnSceneLoad();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>())
            LoadScene();
    }

    IEnumerator SceneLoadAsyncWithVideo()
    {
        string m_Text = "";
        
        yield return new WaitForSeconds(HUDManager.Instance.FadeToBlack());
        UpdateAllNeeded();
        var task = SceneManager.LoadSceneAsync(sceneToLoad);
        task.allowSceneActivation = false;

        while (!task.isDone)
        {
            //Output the current progress
            m_Text = "Loading progress: " + (task.progress * 100) + "%";

            // Check if the load has finished
            if (task.progress >= 0.9f)
            {
                m_Text = "Loaded!";
                task.allowSceneActivation = true;
            }

            Debug.Log(m_Text);
            yield return new WaitForSeconds(0.1f);
        }
        
        //Debug.Log("ENDED!");
        task.allowSceneActivation = true;
        Coroutine = null;
        //Destroy(this.gameObject);
    }
}