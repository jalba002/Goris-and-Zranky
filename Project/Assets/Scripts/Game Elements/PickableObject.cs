using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

public class PickableObject : RestartableObject
{
    protected ConfigurableJoint configJoint;
    protected Rigidbody rb;
    protected bool IsPicked = false;
    protected bool IsThrown = false;

    private float oldMass;

    [Range(1f, 100f)] public float force = 5f;
    
    public new void Awake()
    {
        base.Awake();
        this.rb = GetComponent<Rigidbody>();
        oldMass = this.rb.mass;
    }

    // public new void Start()
    // {
    //     base.Start();
    // }

    #region Joints
    
    public void CreateJoint()
    {
        configJoint = this.gameObject.AddComponent<ConfigurableJoint>();

        configJoint.xMotion = ConfigurableJointMotion.Free;
        configJoint.yMotion = ConfigurableJointMotion.Free;
        configJoint.zMotion = ConfigurableJointMotion.Free;

        configJoint.angularXMotion = ConfigurableJointMotion.Free;
        configJoint.angularYMotion = ConfigurableJointMotion.Free;
        configJoint.angularZMotion = ConfigurableJointMotion.Free;

        var mass = rb.mass;
        JointDrive genDrive =new JointDrive()
        {
            positionSpring = 8000 * mass,
            positionDamper = 5 * mass,
            maximumForce = mass * 5000f
        };
        configJoint.xDrive = genDrive;
        configJoint.yDrive = genDrive;
        configJoint.zDrive = genDrive;
        configJoint.angularXDrive = genDrive;
        configJoint.angularYZDrive = genDrive;

        configJoint.targetRotation = rb.gameObject.transform.rotation;

        configJoint.autoConfigureConnectedAnchor = false;
        //configJoint.connectedAnchor = Vector3.zero;
        //configJoint.axis = Vector3.up;
        //configJoint.connectedAnchor = new Vector3(0f, 0f, 0f);
        configJoint.anchor = rb.centerOfMass;
        configJoint.connectedAnchor = Vector3.zero;
    }

    void DestroyJoint()
    {
        if (configJoint == null) return;
        configJoint.connectedBody = null;
        Destroy(configJoint);
    }
    #endregion

    public virtual void Drop()
    {
        Disconnect();
    }
    
    public virtual void Throw(Vector3 direction)
    {
        this.rb.velocity = direction * force;
        IsThrown = true;
        Disconnect();
    }

    public virtual void Deactivate()
    {
        meshRenderer.enabled = false;
        Disconnect();
        this.gameObject.SetActive(false);
    }

    public void Connect(Rigidbody rb)
    {
        if(configJoint == null)
            CreateJoint();
        IsPicked = true;
        this.rb.mass = 1f;
        configJoint.connectedBody = rb;
    }

    public void Disconnect()
    {
        IsPicked = false;
        DestroyJoint();
        rb.mass = oldMass;
    }
    
    public bool isEnabled()
    {
        return this.gameObject.activeInHierarchy;
    }

    public Rigidbody GetConnectedRB()
    {
        return configJoint.connectedBody;
    }

    public override void Restart()
    {
        base.Restart();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.mass = oldMass;
    }

    public void AddRB()
    {
        rb = this.gameObject.AddComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        IsThrown = false;
    }
}