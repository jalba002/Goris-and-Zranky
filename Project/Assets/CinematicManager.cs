using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    public GameObject Frankestein;
    public AnimationClip Animation;
   
    void Start()
    {
        FrankLoader f = FindObjectOfType<FrankLoader>();
        if (f == null) return;
        f.LoadFrank(Frankestein.transform.position);
        Destroy(Frankestein);
        f.gameObject.AddComponent<Animation>().clip = Animation;
    }
}
