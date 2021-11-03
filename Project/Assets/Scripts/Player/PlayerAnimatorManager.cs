using Player;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    private PlayerController pc;
    private ObjectPicker _pickupZone;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        pc = GetComponent<PlayerController>();
        _pickupZone = GetComponentInChildren<ObjectPicker>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc != null)
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

            vel.y = 0f;
            animator.SetFloat("Movement", vel.magnitude);
        }

        if (_pickupZone.lastPickable != null)
        {
            animator.SetBool("Carrying", true);
        }
        else
        {
            animator.SetBool("Carrying", false);
        }
    }

    public void Kill()
    {
        animator.SetTrigger("Death");
    }

    public void Restart()
    {
        animator.Rebind();
        animator.Update(0f);
    }
}