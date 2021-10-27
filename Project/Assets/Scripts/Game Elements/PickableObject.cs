using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

public class PickableObject : RestartableObject
{
    public ConfigurableJoint configJoint;
    public Rigidbody rb;

    [Range(1f, 100f)] public float force = 5f;
    
    public new void Awake()
    {
        base.Awake();
        this.rb = GetComponent<Rigidbody>();
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

        JointDrive genDrive =new JointDrive()
        {
            positionSpring = 50000,
            positionDamper = 250,
            maximumForce = 100000f
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

    public void Drop()
    {
        Disconnect();
    }
    
    public void Throw(Vector3 direction)
    {
        this.rb.velocity = direction * force;
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
        configJoint.connectedBody = rb;
    }

    public void Disconnect()
    {
        DestroyJoint();
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
    }

    public void AddRB()
    {
        rb = this.gameObject.AddComponent<Rigidbody>();
    }
}