using System.Collections;
using UnityEngine;
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

    IEnumerator SceneLoadAsyncWithVideo()
    {
        videoPlayer.clip = this.clip;
        videoPlayer.enabled = true;
        videoPlayer.Play();
        
        var task = SceneManager.LoadSceneAsync(sceneToLoad);
        
        while (!videoEnded || !task.isDone)
        {
            Debug.Log("LOADING");
            yield return new WaitForSeconds(0.2f);
        }
        Debug.Log("ENDED!");
        task.allowSceneActivation = true;
        Destroy(this.gameObject);
    }
}
