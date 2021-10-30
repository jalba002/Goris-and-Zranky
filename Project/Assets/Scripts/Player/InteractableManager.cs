using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// ReSharper disable InconsistentNaming

namespace Player
{
    public class InteractableManager : MonoBehaviour
    {
        BoxCollider attachedCollider;
        private Vector3 halfSize;
    
        public UnityEvent OnInteractSuccess = new UnityEvent();

        private void Awake()
        {
            attachedCollider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            halfSize = attachedCollider.size * 0.5f;
        }

        public void Interact(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            
            var item = GetNearestItem<InteractableObject>();
            if (item.Interact())
            {
                OnInteractSuccess.Invoke(); // This could mean, play sounds or play animations.
            }
        }

        private T GetNearestItem<T>() where T : InteractableObject
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
}
