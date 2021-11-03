using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class SoundPlayer : MonoBehaviour
{
    public Transform startRay;
    public LayerMask LayerMask;

    [System.Serializable]
    public struct SoundMat
    {
        public string associatedMaterial;
        public List<AudioClip> sounds;

        public AudioClip GetSound()
        {
            int i = Random.Range(0, sounds.Count + 1);
            return sounds[i];
        }
    }

    public void PlaySound(Object clip)
    {
        AudioManager.Instance.PlaySound(clip as AudioClip);
    }

    public List<SoundMat> sounds;

    public void PlaySoundAt()
    {
        try
        {

            Ray ray = new Ray(startRay.position, -startRay.up);
            if (Physics.Raycast(ray, out RaycastHit info, 10f, LayerMask))
            {
                var i = info.collider.material;
                var a = sounds.Find(x => i.name.Contains(x.associatedMaterial));
                AudioManager.Instance.PlaySoundAt(startRay.position, a.GetSound());
            }
        }
        catch (Exception ex)
        {
            // Audios...
            //Debug.Log();
        }
    }
}