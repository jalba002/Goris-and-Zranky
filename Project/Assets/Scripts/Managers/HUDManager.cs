using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public Animation animation;
    
    public AnimationClip fadeBlack;
    public AnimationClip fadeWhite;

    private void Start()
    {
        animation = GetComponent<Animation>();
    }

    public void FadeToBlack()
    {
        animation.clip = fadeBlack;
        animation.Play();
    }

    public void FadeToWhite()
    {
        animation.clip = fadeWhite;
        animation.Play();
    }
}
