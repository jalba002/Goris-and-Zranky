using Sirenix.OdinInspector.Editor.Drawers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class CinematicPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public UnityEvent OnVideoEnd = new UnityEvent();
    private void Awake()
    {
        //transform.parent = null;
        //DontDestroyOnLoad(this.gameObject);
        videoPlayer.loopPointReached += VideoEnded;
    }
    
    private void VideoEnded(VideoPlayer vp)
    {
        // When the video ends the Event triggers.
        OnVideoEnd.Invoke();
        Debug.Log("Video ended!");
        OnVideoEnd.RemoveAllListeners();
    }

}
