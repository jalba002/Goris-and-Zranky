using System;
using Interfaces;
using UnityEngine;

public class ObjectResetter : MonoBehaviour, IPlayerCollide
{
    private void OnTriggerEnter(Collider other)
    {
        //throw new NotImplementedException();
        try
        {
            other.gameObject.GetComponent<RestartableObject>().Restart();
        }
        catch(NullReferenceException)
        {
            // Its not restartable! OH!
        }
    }

    public bool Collide(GameObject self, Vector3 collisionPoint)
    {
        //throw new NotImplementedException();
        // self.gameObject.GetComponent<RestartableObject>().Restart();
        return true;
    }

    public void CollideTop()
    {
        //throw new NotImplementedException();
    }

    public bool CollideBottom(Vector3 pos)
    {
        //throw new NotImplementedException();
        return false;
    }
    

    public void StopColliding()
    {
        //throw new NotImplementedException();
    }
}