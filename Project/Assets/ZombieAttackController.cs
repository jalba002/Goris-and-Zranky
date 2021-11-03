using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackController : MonoBehaviour
{
    public Collider AttackCollider;
    
    public void ToggleAttack(int enable)
    {
        AttackCollider.enabled = (enable == 1);
    }
}
