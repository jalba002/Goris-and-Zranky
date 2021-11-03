using System.Collections;
using System.Linq;
using Interfaces;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneLoadAsyncWithCinematic : MonoBehaviour
{
    public string sceneToLoad = "Jordi";
    public VideoClip clip;
    public VideoPlayer videoPlayer;

    private bool videoEnded = false;

    private IEnumerator Coroutine;

    public void Awake()
    {
        this.transform.parent = null;
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene()
    {
        // Play video animation and while that, load the scene async...
        if (Coroutine != null) return;

        Coroutine = SceneLoadAsyncWithVideo();
        StartCoroutine(Coroutine);

        videoPlayer.loopPointReached += VideoEnded;
    }

    private void VideoEnded(VideoPlayer vp)
    {
        videoEnded = true;
    }

    private void UpdateAllNeeded()
    {
        var list = FindObjectsOfType<MonoBehaviour>().OfType<IUpdateOnSceneLoad>();
        foreach (var item in list)
        {
            item.UpdateOnSceneLoad();
        }
    }

    IEnumerator SceneLoadAsyncWithVideo()
    {
        string m_Text = "";
        
        videoPlayer.clip = this.clip;
        videoPlayer.enabled = true;
        videoPlayer.Play();
        UpdateAllNeeded();
        var task = SceneManager.LoadSceneAsync(sceneToLoad);
        task.allowSceneActivation = false;
        
        yield return new WaitForSeconds((float)videoPlayer.clip.length);

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

        Debug.Log("ENDED!");
        task.allowSceneActivation = true;
        Destroy(this.gameObject);
    }
}