using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class CinematicPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public UnityEvent OnVideoEnd = new UnityEvent();
    private void Awake()
    {
        videoPlayer.loopPointReached += VideoEnded;
    }
    
    private void VideoEnded(VideoPlayer vp)
    {
        // When the video ends the Event triggers.
        OnVideoEnd.Invoke();
        Debug.Log("Video ended!");
    }
}
