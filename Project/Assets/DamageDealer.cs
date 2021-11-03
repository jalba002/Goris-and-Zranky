using System;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something here!" + other.gameObject.name);
        var hpman = other.gameObject.GetComponent<HealthManager>();
        try
        {
            hpman.DealDamage(damage);
        }
        catch (NullReferenceException)
        {
        }
    }
}