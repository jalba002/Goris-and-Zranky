using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class DamagingObject : PickableObject
{
    [Title("Damage")]
    public bool velocityDeterminesDamage = false;
    [DisableIf("velocityDeterminesDamage")] public int damage;
    [Title("Speed Settings")]
    [Tooltip("Collisions below this speed are ignored.")]
    public float minimumSpeedToCollide = 0.2f;
    
    [Title("Events")] [Tooltip("Triggers when any collision above the required speed happens.")]
    public UnityEvent OnObjectCollision = new UnityEvent();

    private void OnCollisionEnter(Collision other)
    {
        if (!IsThrown) return;
        if (rb.velocity.magnitude >= minimumSpeedToCollide)
        {
            Debug.Log(other.gameObject.name);
            OnObjectCollision.Invoke();
            IsThrown = false;
            
            var hm = other.gameObject.GetComponent<HealthManager>();
            if (hm != null)
            {
                Debug.Log("DEALING DAMAGE");
                hm.DealDamage(velocityDeterminesDamage ? (rb.velocity.magnitude * rb.mass) : damage);
            }
        }
    }
    
    public override void Throw(Vector3 direction)
    {
        this.rb.AddForce(direction * force, ForceMode.VelocityChange);
        if (rb.velocity.magnitude >= minimumSpeedToCollide)
            IsThrown = true;
        Disconnect();
    }
    
    public override void Drop()
    {
        IsThrown = true;
        Disconnect();
    }

}
