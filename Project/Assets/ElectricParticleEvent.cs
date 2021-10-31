using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricParticleEvent : MonoBehaviour
{
    //public ParticleSystem ElectricityBody;
    //public ParticleSystem NucleElectricity;
    //public ParticleSystem ThrowElectricity;

    //public float spawnTimeToNextParticle = 0;

    //public void StartParticle()
    //{
    //    spawnTimeToNextParticle = Time.timeSinceLevelLoad + ElectricityBody.duration;
    //}

    //public void Update()
    //{
    //    if (Time.timeSinceLevelLoad > spawnTimeToNextParticle)
    //    {
    //        NucleElectricity.Play();
    //        NucleElectricity.Stop();
    //    }
    //}
    public GameObject ElectricityBody;
    public GameObject NucleElectricity;
    public GameObject ThrowElectricity;

    public float ElectricityTime = 0f;

    public void Update()
    {
        ElectricityTime = ElectricityTime + Time.deltaTime;
        if (ElectricityTime > 5f)
        {
            ElectricityBody.SetActive(false);
            NucleElectricity.SetActive(true);

            if (ElectricityTime > 7f)
            {
                NucleElectricity.SetActive(false);
                ThrowElectricity.SetActive(true);         
            }            
        }
    }






}
