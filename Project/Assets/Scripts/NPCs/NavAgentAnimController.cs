using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentAnimController : MonoBehaviour
{
    private NavMeshAgent pc;
    private ObjectPicker _pickupZone;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        pc = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vel = pc.velocity;
        // if (vel.y > 1f)
        // {
        //     animator.ResetTrigger("Land");
        //     animator.SetBool("Jump", true);
        // }

        // if (pc.Controller.isGrounded && animator.GetBool("Jump"))
        // {
        //     animator.SetTrigger("Land");
        //     animator.SetBool("Jump", false);
        // }

        // if (_pickupZone.lastPickable != null)
        // {
        //     animator.SetBool("Carrying", true);
        // }
        // else
        // {
        //     animator.SetBool("Carrying", false);
        // }

        vel.y = 0f;
        animator.SetFloat("Movement", vel.magnitude);
    }
}