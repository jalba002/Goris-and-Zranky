using Player;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    private PlayerController pc;
    private PickupZone _pickupZone;
    private Animator animator;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        pc = GetComponent<PlayerController>();
        _pickupZone = GetComponentInChildren<PickupZone>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel = pc.GetVelocity();
        if (vel.y > 1f)
        {
            animator.ResetTrigger("Land");   
            animator.SetBool("Jump", true);
        }
        if (pc.Controller.isGrounded && animator.GetBool("Jump"))
        {
            animator.SetTrigger("Land");
            animator.SetBool("Jump", false);
        }

        if (_pickupZone.lastPickable != null)
        {
            animator.SetBool("Carrying", true);
        }
        else
        {
           animator.SetBool("Carrying", false); 
        }
        
        vel.y = 0f;
        animator.SetFloat("Movement", vel.magnitude);
    }
}
