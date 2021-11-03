using Interfaces;
using UnityEngine;

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

    public bool CollideBottom(Vector3 collisionPoint)
    {
        Bounds selfBounds = GetComponent<Collider>().bounds;
        if (collisionPoint.y >= (selfBounds.center.y + selfBounds.size.y * 0.3f))
        {
            this.gameObject.GetComponent<HealthManager>().Kill();
            return true;
        }

        return false;
    }

    public void StopColliding()
    {
        //throw new System.NotImplementedException();
    }

    protected override void SpawnLoot()
    {
        if (m_SpawnLootOnDeath)
        {
            var it = Instantiate(loot, this.gameObject.transform.position + Vector3.up, Quaternion.identity);
            it.GetComponent<Rigidbody>()
                .AddForce((transform.forward + transform.up).normalized * 10f, ForceMode.VelocityChange);
        }
    }
}