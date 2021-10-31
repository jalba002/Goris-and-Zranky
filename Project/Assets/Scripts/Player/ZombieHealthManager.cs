using Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ZombieHealthManager : HealthManager, IPlayerCollide
{
    public bool Collide(GameObject self, Vector3 collisionPoint)
    {
        //
        return false;
    }

    public void CollideTop()
    {
        //throw new System.NotImplementedException();
    }

    public bool CollideBottom()
    {
        this.gameObject.GetComponent<HealthManager>().Kill();
        return true;
    }

    public void StopColliding()
    {
        //throw new System.NotImplementedException();
    }
}