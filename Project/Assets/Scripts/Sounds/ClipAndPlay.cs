using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipAndPlay : MonoBehaviour
{
    public Animation Animation;

    public void PlayClip(AnimationClip Clip)
    {
        Animation.clip = Clip;
        Animation.Play();
    }
}
