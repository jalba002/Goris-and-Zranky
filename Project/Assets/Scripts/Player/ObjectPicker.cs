using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public Collider pc;
    public IPickable lastPickable;
    public Rigidbody connectedRB;
    [Required] public BoxCollider attachedCollider;
    private Vector3 halfSize;

    public bool ignoreStrength = false;
    public int pickupStrength = 1;

    private void Awake()
    {
        //pickJoint = GetComponent<ConfigurableJoint>();
        halfSize = attachedCollider.size * 0.5f;
        pc=GetComponentInParent<Collider>(); // CHECK IF THERE IS PLAYER ATTACHED
        
    }

    public void LeftClick()
    {
        //Debug.Log("Left");
        if (lastPickable != null)
        {
            // SHOULD DROP BUT CAN'T!
            return;
        }

        IPickable detectedItem = GetNearestItem();
        try
        {
            // If you have the strength to pick it up, proceed.
            if (ignoreStrength || pickupStrength >= detectedItem?.GetStrengthRequirement())
            {
                lastPickable = detectedItem;
                lastPickable?.Connect(connectedRB);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void RightClick()
    {
        //Debug.Log("Right");
        // Maybe more like a throw
        lastPickable?.Throw(this.gameObject.transform.parent.forward);
        lastPickable = null;
    }

    public void Drop()
    {
        lastPickable?.Disconnect();
        lastPickable = null;
    }


    public void Update()
    {
        CheckObjectDisconnection();
    }

    private void CheckObjectDisconnection()
    {
        if (lastPickable != null && (!lastPickable.isEnabled() || lastPickable.GetConnectedRB() != connectedRB))
            lastPickable = null;
    }

    public IPickable GetNearestItem()
    {
        Collider[] allColliders = new Collider[5];
        Physics.OverlapBoxNonAlloc(transform.position, halfSize, allColliders);
        foreach (Collider col in allColliders)
        {
            try
            {
                IPickable item = col.gameObject.GetComponent<IPickable>();
                if (item != null) return item;
            }
            catch (NullReferenceException)
            {
            }
        }

        return null;
    }
}