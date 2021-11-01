using UnityEngine;

namespace Interfaces
{
    public interface IPlayerCollide
    {
        bool Collide(GameObject self, Vector3 collisionPoint);
        void CollideTop();
        bool CollideBottom(float speed);
        void StopColliding();
    }
}
