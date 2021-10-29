using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    public Collider pc;
    public PickableObject lastPickable;
    public Rigidbody connectedRB;
    [Required] public BoxCollider attachedCollider;
    private Vector3 halfSize;
    
    public T GetNearestItem<T>() where T : InteractableObject
    {
        Collider[] allColliders = new Collider[5];
        Physics.OverlapBoxNonAlloc(transform.position, halfSize, allColliders);
        foreach (Collider col in allColliders)
        {
            try
            {
                T item = col.gameObject.GetComponent<T>();
                if (item != null) return item;
            }
            catch (NullReferenceException)
            {
            }
        }

        return null;
    }
}
