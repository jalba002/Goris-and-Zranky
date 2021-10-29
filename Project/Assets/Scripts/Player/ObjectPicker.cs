using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectPicker : MonoBehaviour
{
    public Collider pc;
    public PickableObject lastPickable;
    public Rigidbody connectedRB;
    [Required] public BoxCollider attachedCollider;
    private Vector3 halfSize;

    private void Awake()
    {
        //pickJoint = GetComponent<ConfigurableJoint>();
        halfSize = attachedCollider.size * 0.5f;
        pc = GetComponentInParent<Collider>(); // CHECK IF THERE IS PLAYER ATTACHED
    }

    public void LeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Left");
            if (lastPickable != null)
            {
                // SHOULD DROP BUT CAN'T!
                lastPickable.Throw(gameObject.transform.parent.forward);
                lastPickable = null;
                return;
            }

            PickableObject detectedItem = GetNearestItem();
            try
            {
                // If you have the strength to pick it up, proceed.

                lastPickable = detectedItem;
                lastPickable?.Connect(connectedRB);
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }

    public void RightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Right");
            // Maybe more like a throw
            lastPickable?.Drop();
            lastPickable = null;
        }
    }
    
    public void Drop()
    {
        lastPickable?.Disconnect();
        lastPickable = null;
    }

    private void CheckObjectDisconnection()
    {
        if (lastPickable != null && (!lastPickable.isEnabled() || lastPickable.GetConnectedRB() != connectedRB))
            lastPickable = null;
    }

    public PickableObject GetNearestItem()
    {
        Collider[] allColliders = new Collider[5];
        Physics.OverlapBoxNonAlloc(transform.position, halfSize, allColliders);
        foreach (Collider col in allColliders)
        {
            try
            {
                PickableObject item = col.gameObject.GetComponent<PickableObject>();
                if (item != null) return item;
            }
            catch (NullReferenceException)
            {
            }
        }

        return null;
    }
}